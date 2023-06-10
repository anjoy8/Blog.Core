using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

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
            Expression mainExpressin = ExpressionStudio(null, strArr.FirstOrDefault(x => x.LinkSymbol == LinkSymbol.Empty), parameter);

            // 将需要放置在最左边的判断条件从列表中去除，因为已经合成到表达式最左边了
            strArr.Remove(strArr.FirstOrDefault(x => x.LinkSymbol == LinkSymbol.Empty));

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
        /// <param name="DynamicLinq"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Expression ExpressionStudio(Expression left, DynamicLinqHelper DynamicLinq, ParameterExpression key)
        {
            Expression mainExpression = key;

            var properties = DynamicLinq.Left.Split('.');

            int index = 0;
            foreach (var t in properties)
            {
                if (mainExpression.Type.HasImplementedRawGeneric(typeof(IEnumerable<>)))
                {
                    return ExpressionStudioEnumerable(left, mainExpression, DynamicLinq.Clone(), properties.Skip(index).ToArray());
                }

                mainExpression = mainExpression.Property(t);
                index++;
            }

            left = left == null
                // 如果左边表达式为空，则当前的表达式就为最左边
                ? ChangeOperationSymbol(DynamicLinq.OperationSymbol, mainExpression, DynamicLinq.Right)
                // 如果不为空，则将当前的表达式连接到左边
                : ChangeLinkSymbol(DynamicLinq.LinkSymbol, left, ChangeOperationSymbol(DynamicLinq.OperationSymbol, mainExpression, DynamicLinq.Right));
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

            mainExpression = Expression.Call(typeof(Enumerable), "Any", new[] { realType }, property, lambda);

            left = left == null
                ? mainExpression
                : ChangeLinkSymbol(dynamicLinq.LinkSymbol, left, mainExpression);

            return left;
        }


        /// <summary>
        /// 将字符串装换成动态帮助类（内含递归）
        /// </summary>
        public static List<DynamicLinqHelper> SpiltStrings(string propertyStr)
        {
            // 定义返回用List
            var outList = new List<DynamicLinqHelper>();

            // 当最后已经没有连接运算符的时候，进入该条件
            if (!propertyStr.Contains("&") & !propertyStr.Contains("|"))
            {
                // 当前的条件是不具备连接符号的
                var lastStr = propertyStr.Trim().Split(' ');
                outList.Add(new DynamicLinqHelper
                {
                    LinkSymbol = LinkSymbol.Empty,
                    Left = lastStr[0],
                    Right = lastStr[2],
                    OperationSymbol = ChangeOperationSymbol(lastStr[1])
                });
                return outList;
            }

            // 判断当前 & | 哪个符号在最后一个判断逻辑内
            var key = propertyStr.LastIndexOf('&') > propertyStr.LastIndexOf('|') ? '&' : '|';

            var nowStrArr = propertyStr.Substring(propertyStr.LastIndexOf(key)).Trim().Split(' ');

            outList.Add(new DynamicLinqHelper
            {
                LinkSymbol = ChangeLinkSymbol(nowStrArr[0]),
                Left = nowStrArr[1],
                OperationSymbol = ChangeOperationSymbol(nowStrArr[2]),
                Right = nowStrArr[3]
            });
            // 将剩余部分继续切割
            propertyStr = propertyStr.Substring(0, propertyStr.LastIndexOf(key)).Trim();
            // 递归 由后彺前
            outList.AddRange(SpiltStrings(propertyStr));

            return outList;
        }

        public static List<DynamicLinqHelper> SplitOperationSymbol(string str)
        {
            var outList = new List<DynamicLinqHelper>();
            var tokens = Regex.Matches(FormatString(str), _pattern, RegexOptions.Compiled)
                .Cast<Match>()
                .Select(m => m.Groups[1].Value.Trim())
                .ToList();

            int lastIndex = tokens.Count - 1;
            int lastOperatingSymbolIndex = -1;
            for (int i = tokens.Count - 1; i >= 0; i--)
            {
                var token = tokens[i].ToLower();

                if (OperatingSystems.ContainsKey(token))
                {
                    //比较运算符
                    lastOperatingSymbolIndex = i;
                }
                else if (LinkSymbols.ContainsKey(token))
                {
                    var left = "";
                    for (int j = i + 1; j < lastOperatingSymbolIndex; j++)
                    {
                        left += tokens[j];
                    }

                    var right = "";
                    for (int j = lastOperatingSymbolIndex + 1; j <= lastIndex; j++)
                    {
                        right += tokens[j];
                    }

                    outList.Add(GetDynamicLinqHelper(LinkSymbols[token],
                        OperatingSystems[tokens[lastOperatingSymbolIndex]],
                        left,
                        right));
                    lastIndex = i - 1;
                    lastOperatingSymbolIndex = -1;
                }
                else if (i == 0 && lastOperatingSymbolIndex != -1)
                {
                    var left = "";
                    for (int j = i; j < lastOperatingSymbolIndex; j++)
                    {
                        left += tokens[j];
                    }

                    var right = "";
                    for (int j = lastOperatingSymbolIndex + 1; j <= lastIndex; j++)
                    {
                        right += tokens[j];
                    }


                    outList.Add(GetDynamicLinqHelper(LinkSymbol.Empty,
                        OperatingSystems[tokens[lastOperatingSymbolIndex]],
                        left,
                        right));
                }
            }

            outList.Reverse();
            return outList;
        }

        public static DynamicLinqHelper GetDynamicLinqHelper(LinkSymbol linkSymbol, OperationSymbol operationSymbol, string left, string right)
        {
            var dynamic = new DynamicLinqHelper
            {
                LinkSymbol = linkSymbol,
                OperationSymbol = operationSymbol,
                Left = left,
                Right = right
            };

            if (dynamic.Right.StartsWith("\"") && dynamic.Right.EndsWith("\""))
            {
                dynamic.Right = dynamic.Right.Remove(0, 1)
                    .Remove(dynamic.Right.Length - 2, 1)
                    .Replace(@"\""", @"""");
            }

            return dynamic;
        }


        /// <summary>
        /// 将字符串符号转成运算枚举符号
        /// </summary>
        public static LinkSymbol ChangeLinkSymbol(string str)
        {
            // 这里判断链接符号
            // 当链接符号为Empty，则说明当前对象为表达式的最左边
            // 如果一个表达式出现两次链接符号为空，则说明输入的字符串格式有问题
            switch (str)
            {
                case "|":
                    return LinkSymbol.OrElse;
                case "&":
                    return LinkSymbol.AndAlso;
                default:
                    return LinkSymbol.Empty;
            }
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
                        _operatingSystems.Add(name.ToLower(), (OperationSymbol)item.GetValue(null));
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
                        _linkSymbols.Add(name, (LinkSymbol)item.GetValue(null));
                    }
                }
            }

            return _linkSymbols;
        }


        public static string FormatString(string str)
        {
            StringBuilder sb = new StringBuilder();
            int firstIndex = -1;
            int lastIndex = -1;
            for (int i = 0; i < str.Length; i++)
            {
                var character = str[i];

                if (firstIndex == -1)
                {
                    if (character.IsNullOrEmpty() && i < str.Length - 2)
                    {
                        if ('"'.Equals(str[i + 1]))
                        {
                            firstIndex = i + 1;
                        }
                    }
                }
                else
                {
                    if ('\"'.Equals(character))
                    {
                        var andIndex = str.IndexOf("\" &", firstIndex);
                        var orIndex = str.IndexOf("\" |", firstIndex);
                        var andOrIndex = andIndex > 0 ? andIndex : orIndex;
                        if (andOrIndex != -1)
                        {
                            lastIndex = andOrIndex;
                        }
                        else
                        {
                            if (i == firstIndex) continue;
                            if (i == str.Length - 1 || str[i + 1].IsNullOrEmpty())
                            {
                                lastIndex = i;
                            }
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

                if (firstIndex != -1)
                {
                    continue;
                }

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
                "startslike", "startscontains", "%>",
                "endlike", "endcontains", "%<",
            }.Select(Regex.Escape)),
            @"""(?:\\.|[^""])*""", // string
            @"\d+(?:\.\d+)?",      // number with optional decimal part
            @"\w+",                // word
            @"\S",                 // other 1-char tokens (or eat up one character in case of an error)
        }) + @")\s*";

        /// <summary>
        /// 将字符串符号转成运算枚举符号
        /// </summary>
        public static OperationSymbol ChangeOperationSymbol(string str)
        {
            switch (str.ToLower())
            {
                case "<":
                    return OperationSymbol.LessThan;
                case "<=":
                    return OperationSymbol.LessThanOrEqual;
                case ">":
                    return OperationSymbol.GreaterThan;
                case ">=":
                    return OperationSymbol.GreaterThanOrEqual;
                case "==":
                case "=":
                    return OperationSymbol.Equal;
                case "!=":
                    return OperationSymbol.NotEqual;
                case "contains":
                case "like":
                case "%=":
                    return OperationSymbol.Contains;
                case "startslike":
                case "startscontains":
                case "%>":
                    return OperationSymbol.StartsContains;
                case "endlike":
                case "endcontains":
                case "%<":
                    return OperationSymbol.EndContains;
            }

            throw new Exception("OperationSymbol IS NULL");
        }

        /// <summary>
        /// 将运算枚举符号转成具体使用方法
        /// </summary>
        public static Expression ChangeOperationSymbol(OperationSymbol symbol, Expression key, object right)
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
                    else
                        return key.GreaterThan(Expression.Constant((newTypeRight)));
                }
                case OperationSymbol.GreaterThanOrEqual:
                {
                    if (key.Type == typeof(string))
                        return key.Contains(Expression.Constant(newTypeRight, typeof(string)));
                    else
                        return key.GreaterThanOrEqual(Expression.Constant(newTypeRight));
                }

                case OperationSymbol.LessThan:
                {
                    if (key.Type == typeof(string))
                        return key.Contains(Expression.Constant(newTypeRight, typeof(string)));
                    else
                        return key.LessThan(Expression.Constant((newTypeRight)));
                }
                case OperationSymbol.LessThanOrEqual:
                {
                    if (key.Type == typeof(string))
                        return key.Contains(Expression.Constant(newTypeRight, typeof(string)));
                    else
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
                    var contains = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .Single(x => x.Name == "Contains" && x.GetParameters().Length == 2)
                        .MakeGenericMethod(key.Type);
                    return Expression.Constant(newTypeRight).Contains(key);

                //return Expression.Call(contains, , key);
            }

            throw new NotImplementedException("OperationSymbol IS NULL");
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
        public OperationSymbol OperationSymbol { get; set; }

        [Display(Name = "连接符")]
        public LinkSymbol LinkSymbol { get; set; }

        public DynamicLinqHelper Clone()
        {
            return new DynamicLinqHelper()
            {
                Left = this.Left,
                Right = this.Right,
                OperationSymbol = this.OperationSymbol,
                LinkSymbol = this.LinkSymbol,
            };
        }
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
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));
            return (IOrderedQueryable<TSource>)source.Provider.CreateQuery<TSource>(resultExpression);
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
                new Type[] { source.ElementType },
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
                new Type[] { source.ElementType },
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
                target.GetType().InvokeMember(property.Name, BindingFlags.SetProperty, null, target, new object[] { source.GetType().GetProperty(property.Name).GetValue(source, null) });
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