using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Blog.Core.Common.Extensions
{
    /// <summary>
    /// 枚举的扩展方法
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取到对应枚举的描述-没有描述信息，返回枚举值
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string EnumDescription(this Enum @enum)
        {
            Type type = @enum.GetType();
            string name = Enum.GetName(type, @enum);
            if (name == null)
            {
                return null;
            }
            FieldInfo field = type.GetField(name);
            if (!(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute))
            {
                return name;
            }
            return attribute?.Description;
        }
        public static int ToEnumInt(this Enum e)
        {
            try
            {
                return e.GetHashCode();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static List<EnumDto> EnumToList<T>()
        {
            return setEnumToList(typeof(T));
        }

        public static List<EnumDto> EnumToList(Type enumType)
        {
            return setEnumToList(enumType);
        }

        private static List<EnumDto> setEnumToList(Type enumType)
        {
            List<EnumDto> list = new List<EnumDto>();
            foreach (var e in Enum.GetValues(enumType))
            {
                EnumDto m = new();
                object[] attacheds = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(EnumAttachedAttribute), true);
                if (attacheds != null && attacheds.Length > 0)
                {
                    EnumAttachedAttribute aa = attacheds[0] as EnumAttachedAttribute;
                    //m.Attached = aa;
                    m.TagType = aa.TagType;
                    m.Description = aa.Description;
                    m.Icon = aa.Icon;
                    m.IconColor = aa.IconColor;
                }

                m.Value = Convert.ToInt32(e);
                m.Name = e.ToString();
                list.Add(m);
            }
            return list;
        }
    }

    /// <summary>
    /// 枚举对象
    /// </summary>
    public class EnumDto
    {
        /// <summary>
        /// 附加属性
        /// </summary>
        public EnumAttachedAttribute Attached { get; set; }

        /// <summary>
        /// 标签类型
        /// </summary>
        public string TagType { get; set; }
        /// <summary>
        /// 枚举描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 枚举名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 枚举值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 图标颜色
        /// </summary>
        public string IconColor { get; set; }
    }
}
