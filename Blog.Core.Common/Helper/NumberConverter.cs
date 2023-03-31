using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Common.Helper
{
    /// <inheritdoc />
    /// <summary>
    /// 大数据json序列化重写
    /// </summary>
    public sealed class NumberConverter : JsonConverter
    {
        /// <summary>
        /// 转换成字符串的类型
        /// </summary>
        private readonly NumberConverterShip _ship;

        /// <summary>
        /// 大数据json序列化重写实例化
        /// </summary>
        public NumberConverter()
        {
            _ship = (NumberConverterShip)0xFF;
        }

        /// <summary>
        /// 大数据json序列化重写实例化
        /// </summary>
        /// <param name="ship">转换成字符串的类型</param>
        public NumberConverter(NumberConverterShip ship)
        {
            _ship = ship;
        }

        /// <inheritdoc />
        /// <summary>
        /// 确定此实例是否可以转换指定的对象类型。
        /// </summary>
        /// <param name="objectType">对象的类型。</param>
        /// <returns>如果此实例可以转换指定的对象类型，则为：<c>true</c>，否则为：<c>false</c></returns>
        public override bool CanConvert(Type objectType)
        {
            var typecode = Type.GetTypeCode(objectType.Name.Equals("Nullable`1") ? objectType.GetGenericArguments().First() : objectType);
            switch (typecode)
            {
                case TypeCode.Decimal:
                    return (_ship & NumberConverterShip.Decimal) == NumberConverterShip.Decimal;
                case TypeCode.Double:
                    return (_ship & NumberConverterShip.Double) == NumberConverterShip.Double;
                case TypeCode.Int64:
                    return (_ship & NumberConverterShip.Int64) == NumberConverterShip.Int64;
                case TypeCode.UInt64:
                    return (_ship & NumberConverterShip.UInt64) == NumberConverterShip.UInt64;
                case TypeCode.Single:
                    return (_ship & NumberConverterShip.Single) == NumberConverterShip.Single;
                default: return false;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 读取对象的JSON表示。
        /// </summary>
        /// <param name="reader">从 <see cref="T:Newtonsoft.Json.JsonReader" /> 中读取。</param>
        /// <param name="objectType">对象的类型。</param>
        /// <param name="existingValue">正在读取的对象的现有值。</param>
        /// <param name="serializer">调用的序列化器实例。</param>
        /// <returns>对象值。</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return AsType(reader.Value.ToString(), objectType);
        }

        /// <summary>
        /// 字符串格式数据转其他类型数据
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="destinationType">目标格式</param>
        /// <returns>转换结果</returns>
        public static object AsType(string input, Type destinationType)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(destinationType);
                if (converter.CanConvertFrom(typeof(string)))
                {
                    return converter.ConvertFrom(null, null, input);
                }

                converter = TypeDescriptor.GetConverter(typeof(string));
                if (converter.CanConvertTo(destinationType))
                {
                    return converter.ConvertTo(null, null, input, destinationType);
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        /// <inheritdoc />
        /// <summary>
        /// 写入对象的JSON表示形式。
        /// </summary>
        /// <param name="writer">要写入的 <see cref="T:Newtonsoft.Json.JsonWriter" /> 。</param>
        /// <param name="value">要写入对象值</param>
        /// <param name="serializer">调用的序列化器实例。</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                var objectType = value.GetType();
                var typeCode = Type.GetTypeCode(objectType.Name.Equals("Nullable`1") ? objectType.GetGenericArguments().First() : objectType);
                switch (typeCode)
                {
                    case TypeCode.Decimal:
                        writer.WriteValue(((decimal)value).ToString("f6"));
                        break;
                    case TypeCode.Double:
                        writer.WriteValue(((double)value).ToString("f4"));
                        break;
                    case TypeCode.Single:
                        writer.WriteValue(((float)value).ToString("f2"));
                        break;
                    default:
                        writer.WriteValue(value.ToString());
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 转换成字符串的类型
    /// </summary>
    [Flags]
    public enum NumberConverterShip
    {
        /// <summary>
        /// 长整数
        /// </summary>
        Int64 = 1,

        /// <summary>
        /// 无符号长整数
        /// </summary>
        UInt64 = 2,

        /// <summary>
        /// 浮点数
        /// </summary>
        Single = 4,

        /// <summary>
        /// 双精度浮点数
        /// </summary>
        Double = 8,

        /// <summary>
        /// 大数字
        /// </summary>
        Decimal =16
    }
}
