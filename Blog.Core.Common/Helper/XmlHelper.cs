using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Blog.Core.Common.Helper
{
    /// <summary>
    /// xml序列化帮助类
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// 存储序列类型,防止内存泄漏
        /// </summary>
        private static ConcurrentDictionary<Type, XmlSerializer> hasTypes = new ConcurrentDictionary<Type, XmlSerializer>();
        /// <summary>
        /// 转换对象为JSON格式数据
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="obj">对象</param>
        /// <returns>字符格式的JSON数据</returns>
        public static string GetXML<T>(object obj, string rootName = "root")
        {
            XmlSerializer xs;
            var xsType = typeof(T);
            hasTypes.TryGetValue(xsType, out xs);
            if(xs == null)
            {
                xs = new XmlSerializer(typeof(T));
                hasTypes.TryAdd(xsType, xs);
            }
            using (TextWriter tw = new StringWriter())
            {
                xs.Serialize(tw, obj);
                return tw.ObjToString();
            }
        }

        /// <summary>
        /// Xml格式字符转换为T类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T ParseFormByXml<T>(string xml, string rootName = "root")
        {
            XmlSerializer xs;
            var xsType = typeof(T);
            hasTypes.TryGetValue(xsType, out xs);
            if (xs == null)
            {
                xs = new XmlSerializer(xsType, new XmlRootAttribute(rootName));
                hasTypes.TryAdd(xsType, xs);
            }
            using (StringReader reader = new StringReader(xml))
            {
                return (T)xs.Deserialize(reader);
            }
        }
    }
}
