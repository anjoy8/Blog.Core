using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Mapster;

namespace Blog.Core.Common.Helper
{
    #region 动态linq帮助类，连接符号，运算符号

    /// <summary>
    /// 动态linq工厂
    /// </summary>
    public static class DynamicLinqFactory
    {
        private static readonly Dictionary<string, OperationSymbol> _operatingSystems = new Dictionary<string, OperationSymbol>();
        public static Dictionary<string, OperationSymbol> OperatingSystems => GetOperationSymbol();

        private static readonly Dictionary<string, LinkSymbol> _linkSymbols = new Dictionary<string, LinkSymbol>();
        public static Dictionary<string, LinkSymbol> LinkSymbols => GetLinkSymbol();

        /// <summary>
        /// 生成lambd表达式(如:CompanyID != 1 & CompanyID == 1)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="propertyStr"></param>
        /// <returns></returns>
        public static Expression<Func<TSource, bool>> CreateLambda<TSource>(string propertyStr)
        {
            // 设置自定义lanbd
            // 定义 lanbd 种子（p=> xxxxxx）中的 p
            if (string.IsNullOrWhiteSpace(propertyStr))
                return LinqHelper.True<TSource>(); //为空就返回空的表达式

            var parameter = Expression.Parameter(typeof(TSource), "p");
            var strArr = SplitOperationSymbol(propertyStr);


            // 第一个判断条件，固定一个判断条件作为最左边
            Expression mainExpressin = ExpressionStudio(null, strArr[0], parameter);
            // 将需要放置在最左边的判断条件从列表中去除，因为已经合成到表达式最左边了
            strArr.RemoveAt(0);

            foreach (var x in strArr)
            {
                mainExpressin = ExpressionStudio(mainExpressin, x, parameter);
            }

            return mainExpressin.ToLambda<Func<TSource, bool>>(parameter);
        }

        /// <summary>
        /// 组合条件判断表达式
        /// </summary>
        /// <param name="left">左边的表达式</param>
        /// <param name="dynamicLinq"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Expression ExpressionStudio(Expression left, DynamicLinqHelper dynamicLinq, ParameterExpression key)
        {
            Expression mainExpression = key;

            if (!dynamicLinq.Left.IsNullOrEmpty())
            {
                var properties = dynamicLinq.Left.Split('.');

                int index = 0;
                foreach (var t in properties)
                {
                    if (mainExpression.Type.HasImplementedRawGeneric(typeof(IEnumerable<>)))
                    {
                        return ExpressionStudioEnumerable(left, mainExpression, dynamicLinq.Adapt<DynamicLinqHelper>(),
                            properties.Skip(index).ToArray());
                    }

                    mainExpression = mainExpression.Property(t);
                    index++;
                }
            }

            Expression right = null;
            if (dynamicLinq.IsMerge && dynamicLinq.Child.Any())
            {
                right = ExpressionStudio(null, dynamicLinq.Child[0], key);
                for (var i = 1; i < dynamicLinq.Child.Count; i++)
                {
                    right = ChangeLinkSymbol(dynamicLinq.Child[i].LinkSymbol, right, ExpressionStudio(null, dynamicLinq.Child[i], key));
                }
            }
            else
            {
                right = ChangeOperationSymbol(dynamicLinq.OperationSymbol, mainExpression, dynamicLinq.Right);
            }

            left = left == null
                // 如果左边表达式为空，则当前的表达式就为最左边
                ? right
                // 如果不为空，则将当前的表达式连接到左边
                : ChangeLinkSymbol(dynamicLinq.LinkSymbol, left, right);
            return left;
        }

        public static Expression ExpressionStudioEnumerable(Expression left, Expression property, DynamicLinqHelper dynamicLinq, string[] properties)
        {
            var realType = property.Type.GenericTypeArguments[0];

            var parameter = Expression.Parameter(realType, "z");
            Expression mainExpression = property;
            if (!properties.Any())
            {
                throw new ApplicationException("条件表达式错误,属性为集合时,需要明确具体属性");
            }

            dynamicLinq.Left = string.Join(".", properties);
            mainExpression = ExpressionStudio(null, dynamicLinq, parameter);

            var lambda = Expression.Lambda(mainExpression, parameter);

            mainExpression = Expression.Call(typeof(Enumerable), "Any", new[] {realType}, property, lambda);

            left = left == null
                ? mainExpression
                : ChangeLinkSymbol(dynamicLinq.LinkSymbol, left, mainExpression);

            return left;
        }


        public static List<DynamicLinqHelper> SplitOperationSymbol(string str)
        {
            var outList = new List<DynamicLinqHelper>();
            var tokens = Regex.Matches(FormatString(str), _pattern, RegexOptions.Compiled)
                .Select(m => m.Groups[1].Value.Trim())
                .ToList();
            SplitOperationSymbol(tokens, outList);
            return outList;
        }

        private static void SplitOperationSymbol(List<string> tokens, List<DynamicLinqHelper> outList, int start = 0, int end = 0)
        {
            var dys = new Stack<DynamicLinqHelper>();
            var dynamicLinqHelper = new DynamicLinqHelper();
            if (end == 0)
            {
                end = tokens.Count - 1;
            }

            for (int i = start; i <= end; i++)
            {
                var token = tokens[i];

                if (LinkSymbols.TryGetValue(token, out var symbol))
                {
                    if (dys.Count > 0)
                    {
                        var linqHelper = dys.Peek();
                        linqHelper.Child.Add(dynamicLinqHelper);
                    }
                    else
                    {
                        outList.Add(dynamicLinqHelper);
                    }

                    dynamicLinqHelper = new DynamicLinqHelper()
                    {
                        LinkSymbol = symbol,
                    };
                    continue;
                }

                if (OperatingSystems.TryGetValue(token.ToLower(), out var system))
                {
                    dynamicLinqHelper!.OperationSymbol = system;
                    continue;
                }


                if (dynamicLinqHelper!.OperationSymbol != OperationSymbol.In)
                {
                    if (string.Equals(token.Trim(), "("))
                    {
                        dynamicLinqHelper!.IsMerge = true;
                        dynamicLinqHelper.Child = new List<DynamicLinqHelper>();
                        dys.Push(dynamicLinqHelper);
                        dynamicLinqHelper = new DynamicLinqHelper();
                        continue;
                    }

                    if (string.Equals(token.Trim(), ")"))
                    {
                        if (dys.Count > 1)
                        {
                            var dya = dys.Pop();
                            dya.Child.Add(dynamicLinqHelper);

                            dynamicLinqHelper = dya;
                            continue;
                        }
                        else
                        {
                            var dya = dys.Pop();
                            dya.Child.Add(dynamicLinqHelper);
                            outList.Add(dya);
                            dynamicLinqHelper = null;
                            continue;
                        }
                    }
                }


                if (dynamicLinqHelper!.OperationSymbol is null)
                {
                    dynamicLinqHelper.Left += token;
                }
                else
                {
                    dynamicLinqHelper.Right += FormatValue(token);
                }

                if (i == end)
                {
                    outList.Add(dynamicLinqHelper);
                    dynamicLinqHelper = null;
                }
            }
        }

        public static string FormatValue(string str)
        {
            return str.TrimStart('"').TrimEnd('"');
            // return str.TrimStart('"').TrimEnd('"').Replace(@"\""", @"""");
        }


        /// <summary>
        /// 将运算枚举符号转成具体使用方法
        /// </summary>
        public static Expression ChangeLinkSymbol(LinkSymbol symbol, Expression left, Expression right)
        {
            switch (symbol)
            {
                case LinkSymbol.OrElse:
                    return left.OrElse(right);
                case LinkSymbol.AndAlso:
                    return left.AndAlso(right);
                default:
                    return left;
            }
        }

        public static Dictionary<string, OperationSymbol> GetOperationSymbol()
        {
            if (_operatingSystems.Any()) return _operatingSystems;

            var fielding = typeof(OperationSymbol).GetFields();
            foreach (var item in fielding)
            {
                if (item.GetCustomAttribute(typeof(DisplayAttribute)) is DisplayAttribute attr && !attr.Name.IsNullOrEmpty())
                {
                    foreach (var name in attr.Name.Split(';'))
                    {
                        _operatingSystems.Add(name.ToLower(), (OperationSymbol) item.GetValue(null));
                    }
                }
            }

            return _operatingSystems;
        }

        public static Dictionary<string, LinkSymbol> GetLinkSymbol()
        {
            if (_linkSymbols.Any()) return _linkSymbols;

            var fielding = typeof(LinkSymbol).GetFields();
            foreach (var item in fielding)
            {
                if (item.GetCustomAttribute(typeof(DisplayAttribute)) is DisplayAttribute attr && !attr.Name.IsNullOrEmpty())
                {
                    foreach (var name in attr.Name.Split(';'))
                    {
                        _linkSymbols.Add(name, (LinkSymbol) item.GetValue(null));
                    }
                }
            }

            return _linkSymbols;
        }


        public static string FormatString(string str)
        {
            var sb = new StringBuilder();
            var firstIndex = -1;
            var lastIndex = -1;
            for (var i = 0; i < str.Length; i++)
            {
                var character = str[i];

                if (firstIndex == -1)
                {
                    if (character.IsNullOrEmpty() && i < str.Length - 2)
                        if ('"'.Equals(str[i + 1]))
                            firstIndex = i + 1;
                }
                else
                {
                    if ('\"'.Equals(character))
                    {
                        var andIndex = str.IndexOf("\" &", firstIndex);
                        var orIndex = str.IndexOf("\" |", firstIndex);
                        var andOrIndex = Math.Min(andIndex, orIndex);
                        andOrIndex = andOrIndex == -1 ? Math.Max(andOrIndex, orIndex) : andOrIndex;

                        if (andOrIndex != -1)
                        {
                            lastIndex = andOrIndex;
                        }
                        else
                        {
                            if (i == firstIndex) continue;
                            if (i == str.Length - 1 || str[i + 1].IsNullOrEmpty()) lastIndex = i;
                        }
                    }

                    if (lastIndex != -1)
                    {
                        var temp = str.Substring(firstIndex + 1, lastIndex - firstIndex - 1).Replace(@"""", @"\""");
                        sb.Append($" \"{temp}\" ");

                        i = lastIndex;
                        firstIndex = -1;
                        lastIndex = -1;
                        continue;
                    }
                }

                if (firstIndex != -1) continue;

                sb.Append(character);
            }

            return sb.ToString();
        }

        /// <summary>tokenizer pattern: Optional-SpaceS...Token...Optional-Spaces</summary>
        public static readonly string _pattern = @"\s*(" + string.Join("|", new string[]
        {
            // operators and punctuation that are longer than one char: longest first
            string.Join("|", new[]
            {
                "||", "&&", "==", "!=", "<=", ">=",
                "in",
                "like", "contains", "%=",
                "startslike", "StartsLike", "startscontains", "StartsContains", "%>",
                "endlike", "EndLike", "endcontains", "EndContains", "%<",
            }.Select(Regex.Escape)),
            @"""(?:\\.|[^""])*""", // string
            @"\d+(?:\.\d+)?",      // number with optional decimal part
            @"\w+",                // word
            @"\S",                 // other 1-char tokens (or eat up one character in case of an error)
        }) + @")\s*";


        /// <summary>
        /// 将运算枚举符号转成具体使用方法
        /// </summary>
        public static Expression ChangeOperationSymbol(OperationSymbol? symbol, Expression key, object right)
        {
            // 将右边数据类型强行转换成左边一样的类型
            // 两者如果Type不匹配则无法接下去的运算操作，抛出异常
            object newTypeRight;
            if (right == null || string.IsNullOrEmpty(right.ToString()) || right.ToString() == "null")
            {
                newTypeRight = null;
            }
            else
            {
                if (symbol == OperationSymbol.In)
                {
                    newTypeRight = right.ChangeTypeList(key.Type);
                }
                else
                {
                    newTypeRight = right.ChangeType(key.Type);
                }
            }


            // 根据当前枚举类别判断使用那种比较方法
            switch (symbol)
            {
                case OperationSymbol.Equal:
                    return key.Equal(Expression.Constant(newTypeRight));
                case OperationSymbol.GreaterThan:
                {
                    if (key.Type == typeof(string))
                        return key.Contains(Expression.Constant(newTypeRight)); //对string 特殊处理  由于string
                    return key.GreaterThan(Expression.Constant((newTypeRight)));
                }
                case OperationSymbol.GreaterThanOrEqual:
                {
                    if (key.Type == typeof(string))
                        return key.Contains(Expression.Constant(newTypeRight, typeof(string)));
                    return key.GreaterThanOrEqual(Expression.Constant(newTypeRight));
                }

                case OperationSymbol.LessThan:
                {
                    if (key.Type == typeof(string))
                        return key.Contains(Expression.Constant(newTypeRight, typeof(string)));
                    return key.LessThan(Expression.Constant((newTypeRight)));
                }
                case OperationSymbol.LessThanOrEqual:
                {
                    if (key.Type == typeof(string))
                        return key.Contains(Expression.Constant(newTypeRight, typeof(string)));
                    return key.LessThanOrEqual(Expression.Constant((newTypeRight)));
                }
                case OperationSymbol.NotEqual:
                    return key.NotEqual(Expression.Constant(newTypeRight));
                case OperationSymbol.Contains:
                    return key.Contains(Expression.Constant(newTypeRight));
                case OperationSymbol.StartsContains:
                    return key.StartContains(Expression.Constant(newTypeRight));
                case OperationSymbol.EndContains:
                    return key.EndContains(Expression.Constant(newTypeRight));
                case OperationSymbol.In:
                    return Expression.Constant(newTypeRight).Contains(key);
                default:
                    throw new ArgumentException("OperationSymbol IS NULL");
            }
        }
    }

    /// <summary>
    /// 动态linq帮助类
    /// </summary>
    public class DynamicLinqHelper
    {
        [Display(Name = "左")]
        public string Left { get; set; }

        [Display(Name = "右")]
        public string Right { get; set; }

        [Display(Name = "运算符")]
        public OperationSymbol? OperationSymbol { get; set; }

        [Display(Name = "连接符")]
        public LinkSymbol LinkSymbol { get; set; }

        /// <summary>
        /// 是否是合并 用于括号
        /// </summary>
        public bool IsMerge { get; set; } = false;

        /// <summary>
        /// 再有括号时候使用
        /// </summary>
        public List<DynamicLinqHelper> Child { get; set; }
    }

    /// <summary>
    /// 连接符枚举（将来可能会包含 括号 ）
    /// </summary>
    public enum LinkSymbol
    {
        [Display(Name = "&&;&")]
        AndAlso,

        [Display(Name = "||;|")]
        OrElse,

        Empty
    }

    /// <summary>
    /// 常用比较运算符 > , >= , == , < , <= , != ,Contains
    /// </summary>
    public enum OperationSymbol
    {
        [Display(Name = "in")]
        In,

        [Display(Name = "like;contains;%=")]
        Contains,

        [Display(Name = "StartsLike;StartsContains;%>")]
        StartsContains,

        [Display(Name = "EndLike;EndContains;%<")]
        EndContains,

        [Display(Name = ">")]
        GreaterThan,

        [Display(Name = ">=")]
        GreaterThanOrEqual,

        [Display(Name = "<")]
        LessThan,

        [Display(Name = "<=")]
        LessThanOrEqual,

        [Display(Name = "==;=")]
        Equal,

        [Display(Name = "!=")]
        NotEqual
    }

    #endregion


    /// <summary>
    /// Queryable扩展
    /// </summary>
    public static class QueryableExtensions
    {
        #region 自定义扩展Queryable

        /// <summary>
        /// Where扩展
        /// </summary>
        public static IEnumerable<TSource> IWhere<TSource>(this IEnumerable<TSource> source, string linqStr)
        {
            return source.Where(DynamicLinqFactory.CreateLambda<TSource>(linqStr).Compile());
        }

        /// <summary>
        /// FirstOrDefault扩展
        /// </summary>
        public static TSource IFirstOrDefault<TSource>(this IEnumerable<TSource> source, string linqStr)
        {
            return source.FirstOrDefault(DynamicLinqFactory.CreateLambda<TSource>(linqStr).Compile());
        }

        /// <summary>
        /// Count扩展
        /// </summary>
        public static Int32 ICount<TSource>(this IEnumerable<TSource> source, string linqStr)
        {
            return source.Count(DynamicLinqFactory.CreateLambda<TSource>(linqStr).Compile());
        }

        /// <summary>
        /// 自定义排序
        /// </summary>
        public static IOrderedQueryable<TSource> ISort<TSource>(this IQueryable<TSource> source, string orderByProperty, bool asc)
        {
            string command = asc ? "OrderBy" : "OrderByDescending";
            var type = typeof(TSource);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] {type, property.PropertyType}, source.Expression,
                Expression.Quote(orderByExpression));
            return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>(resultExpression);
        }

        /// <summary>
        /// 自定义分页
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="nowPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IQueryable<TSource> IPaging<TSource>(this IQueryable<TSource> source, int nowPage, int pageSize)
        {
            return source.ISkip((nowPage - 1) * pageSize).ITake(pageSize);
        }

        /// <summary>
        /// 自定义Skip
        /// </summary>
        public static IQueryable<TSource> ISkip<TSource>(this IQueryable<TSource> source, int count)
        {
            return source.Provider.CreateQuery<TSource>(Expression.Call(
                // 类别
                typeof(Queryable),
                // 调用的方法
                "Skip",
                // 元素类别
                new Type[] {source.ElementType},
                // 调用的表达树
                source.Expression,
                // 参数
                Expression.Constant(count)));
        }

        /// <summary>
        /// 自定义Take
        /// </summary>
        public static IQueryable<TSource> ITake<TSource>(this IQueryable<TSource> source, int count)
        {
            return source.Provider.CreateQuery<TSource>(Expression.Call(
                // 类别
                typeof(Queryable),
                // 调用的方法
                "Take",
                // 元素类别
                new Type[] {source.ElementType},
                // 调用的表达树
                source.Expression,
                // 参数
                Expression.Constant(count)));
        }

        /// <summary>
        /// 自定义去重复
        /// </summary>
        public static IEnumerable<TSource> IDistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }

        /// <summary>
        /// 动态赋值
        /// </summary>
        public static void CopyTo<T>(this object source, T target) where T : class, new()
        {
            if (source == null)
                return;

            if (target == null)
            {
                target = new T();
            }

            foreach (var property in target.GetType().GetProperties())
            {
                // 这里可以判断一下当前属性值是否为空的 source.GetType().GetProperty(property.Name).GetValue(source, null)
                target.GetType().InvokeMember(property.Name, BindingFlags.SetProperty, null, target,
                    new object[] {source.GetType().GetProperty(property.Name).GetValue(source, null)});
            }
        }

        /// <summary>
        /// 移除特殊字段数据
        /// </summary>
        public static void RemoveSpecialPropertyValue(this object source)
        {
            var properties = source.GetType().GetProperties();
            foreach (var x in properties)
            {
                if (x.GetAccessors().Any(y => y.IsVirtual))
                {
                    source.GetType().GetProperty(x.Name).SetValue(source, null, null);
                }
            }
        }

        #endregion
    }
}