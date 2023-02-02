using System;
using System.IO;
namespace Blog.Core.EventBus
{
	public class Protobuf
	{
		/// <summary>
		/// Protobuf 反序列化
		/// </summary>
		public static T Deserialize<T>(ReadOnlySpan<byte> data)
		{
			Stream stream = new MemoryStream(data.ToArray());
			var info = ProtoBuf.Serializer.Deserialize<T>(stream);
			return info;
		}
		/// <summary>
		/// 通过Protobuf 转字节
		/// </summary>
		public static byte[] Serialize<T>(T data)
		{
			byte[] datas;
			using (var stream = new MemoryStream())
			{
				ProtoBuf.Serializer.Serialize(stream, data);
				datas = stream.ToArray();
			}
			return datas;


		}
	}
}
