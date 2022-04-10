using Microsoft.International.Converters.PinYinConverter;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Blog.Core.Common.Helper
{
    /// <summary>
    /// 汉字转换拼音
    /// </summary>
    public static class PingYinUtil
    {
        private static Dictionary<int, List<string>> GetTotalPingYinDictionary(string text)
        {
            var chs = text.ToCharArray();

            //记录每个汉字的全拼
            Dictionary<int, List<string>> totalPingYinList = new Dictionary<int, List<string>>();

            for (int i = 0; i < chs.Length; i++)
            {
                var pinyinList = new List<string>();

                //是否是有效的汉字
                if (ChineseChar.IsValidChar(chs[i]))
                {
                    ChineseChar cc = new ChineseChar(chs[i]);
                    pinyinList = cc.Pinyins.Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
                }
                else
                {
                    pinyinList.Add(chs[i].ToString());
                }

                //去除声调，转小写
                pinyinList = pinyinList.ConvertAll(p => Regex.Replace(p, @"\d", "").ToLower());

                //去重
                pinyinList = pinyinList.Where(p => !string.IsNullOrWhiteSpace(p)).Distinct().ToList();
                if (pinyinList.Any())
                {
                    totalPingYinList[i] = pinyinList;
                }
            }

            return totalPingYinList;
        }
        /// <summary>
        /// 获取汉语拼音全拼
        /// </summary>
        /// <param name="text">The string.</param>
        /// <returns></returns>
        public static List<string> GetTotalPingYin(this string text)
        {
            var result = new List<string>();
            foreach (var pys in GetTotalPingYinDictionary(text))
            {
                var items = pys.Value;
                if (result.Count <= 0)
                {
                    result = items;
                }
                else
                {
                    //全拼循环匹配
                    var newTotalPingYinList = new List<string>();
                    foreach (var totalPingYin in result)
                    {
                        newTotalPingYinList.AddRange(items.Select(item => totalPingYin + item));
                    }
                    newTotalPingYinList = newTotalPingYinList.Distinct().ToList();
                    result = newTotalPingYinList;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取汉语拼音首字母
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static List<string> GetFirstPingYin(this string text)
        {
            var result = new List<string>();
            foreach (var pys in GetTotalPingYinDictionary(text))
            {
                var items = pys.Value;
                if (result.Count <= 0)
                {
                    result = items.ConvertAll(p => p.Substring(0, 1)).Distinct().ToList();
                }
                else
                {
                    //首字母循环匹配
                    var newFirstPingYinList = new List<string>();
                    foreach (var firstPingYin in result)
                    {
                        newFirstPingYinList.AddRange(items.Select(item => firstPingYin + item.Substring(0, 1)));
                    }
                    newFirstPingYinList = newFirstPingYinList.Distinct().ToList();
                    result = newFirstPingYinList;
                }
            }
            return result;
        }
    }
}