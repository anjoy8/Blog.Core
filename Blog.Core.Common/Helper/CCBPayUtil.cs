using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Blog.Core.Common
{
    /// <summary>
    /// 建行支付助手(根据官方提供的dll反编译过来的)
    /// </summary>
    public class CCBPayUtil
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public string makeCCBParam(string param, string pubkey)
		{
			string text = this.dicSort(param);
			text += this.MD5KEY;
			string str = new MessageDigest_MD5().Md5_32(text);
			param = param + "&SIGN=" + str;
			if (pubkey.Length >= 30)
			{
				pubkey = pubkey.Substring(pubkey.Length - 30);
			}
			if (pubkey.Length >= 8)
			{
				pubkey = pubkey.Substring(0, 8);
			}
			string text2 = new DES_ENCRY_DECRY().doEncrypt(param, pubkey);
			text2 = text2.Replace("+", ",");
			return HttpUtility.UrlEncode(text2, Encoding.GetEncoding("ISO-8859-1"));
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002104 File Offset: 0x00000304
		public bool verifyNotifySign(string src, string sign, string pubKey)
		{
			return new RSASign().verifySigature(src, sign, pubKey);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002124 File Offset: 0x00000324
		private string dicSort(string param)
		{
			return this.GetSignContent(this.strToMap(param));
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002144 File Offset: 0x00000344
		private IDictionary<string, string> strToMap(string param)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			string[] array = param.Split(new char[]
			{
				'&'
			});
			for (int i = 0; i < array.Length; i++)
			{
				if (!"".Equals(array[i]))
				{
					string[] array2 = array[i].Split(new char[]
					{
						'='
					});
					if (array2.Length == 1)
					{
						dictionary.Add(array2[0], "");
					}
					else
					{
						dictionary.Add(array2[0], array2[1]);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021F0 File Offset: 0x000003F0
		private string GetSignContent(IDictionary<string, string> parameters)
		{
			IDictionary<string, string> dictionary = new SortedDictionary<string, string>(parameters);
			IEnumerator<KeyValuePair<string, string>> enumerator = dictionary.GetEnumerator();
			StringBuilder stringBuilder = new StringBuilder("");
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, string> keyValuePair = enumerator.Current;
				string key = keyValuePair.Key;
				keyValuePair = enumerator.Current;
				string value = keyValuePair.Value;
				if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
				{
					stringBuilder.Append(key).Append("=").Append(value).Append("&");
				}
			}
			return stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
		}

		// Token: 0x04000001 RID: 1
		//private string VERSION = "1.0.0";

		// Token: 0x04000002 RID: 2
		private string MD5KEY = "20120315201809041004";
	}
	internal class RSASign
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000022C4 File Offset: 0x000004C4
		protected internal bool verifySigature(string signContent, string sign, string pubKey)
		{
			byte[] inArray = this.hexStrToBytes(pubKey);
			pubKey = Convert.ToBase64String(inArray);
			string text = "-----BEGIN PUBLIC KEY-----\r\n";
			text += pubKey;
			text += "-----END PUBLIC KEY-----\r\n\r\n";
			byte[] sign2 = this.hexStrToBytes(sign);
			byte[] bytes = Encoding.GetEncoding(RSASign.DEFAULT_CHARSET).GetBytes(signContent);
			return this.RSACheckContent(bytes, sign2, text, "MD5");
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002330 File Offset: 0x00000530
		private bool RSACheckContent(byte[] signContent, byte[] sign, string publicKeyPem, string signType)
		{
			bool result;
			try
			{
				RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
				rsacryptoServiceProvider.PersistKeyInCsp = false;
				RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsacryptoServiceProvider, publicKeyPem);
				bool flag = rsacryptoServiceProvider.VerifyData(signContent, signType, sign);
				result = flag;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000237C File Offset: 0x0000057C
		private byte[] hexStrToBytes(string s)
		{
			s = s.Replace(" ", "");
			if (s.Length % 2 != 0)
			{
				s += " ";
			}
			byte[] array = new byte[s.Length / 2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Convert.ToByte(s.Substring(i * 2, 2), 16);
			}
			return array;
		}

		// Token: 0x04000003 RID: 3
		private static string DEFAULT_CHARSET = "GBK";
	}
	public class DES_ENCRY_DECRY
	{
		protected internal string doEncrypt(string param, string pubkey)
		{
			this.tdesKey = ((pubkey.Length > 8) ? pubkey.Substring(0, 8) : pubkey);
			byte[] bytes = this.DESEncrypt(this.UTF_16BE, param, this.ISO_8859_1, this.tdesKey);
			return this.Base64Encode(bytes);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002684 File Offset: 0x00000884
		protected internal string doDecrypt(string param, string pubkey)
		{
			this.tdesKey = ((pubkey.Length > 8) ? pubkey.Substring(0, 8) : pubkey);
			return this.DESDecrypt(this.UTF_16BE, param, this.ISO_8859_1, this.tdesKey);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000026CC File Offset: 0x000008CC
		private byte[] DESEncrypt(string dataCharset, string data, string keyCharset, string key)
		{
			byte[] result;
			try
			{
				byte[] bytes = Encoding.GetEncoding(keyCharset).GetBytes(key);
				byte[] rgbIV = bytes;
				byte[] bytes2 = Encoding.GetEncoding(dataCharset).GetBytes(data);
				var descryptoServiceProvider = DES.Create();
				descryptoServiceProvider.Mode = CipherMode.ECB;
				descryptoServiceProvider.Padding = PaddingMode.PKCS7;
				MemoryStream memoryStream = new MemoryStream();
				CryptoStream cryptoStream = new CryptoStream(memoryStream, descryptoServiceProvider.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
				cryptoStream.Write(bytes2, 0, bytes2.Length);
				cryptoStream.FlushFinalBlock();
				result = memoryStream.ToArray();
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002764 File Offset: 0x00000964
		private string DESDecrypt(string dataCharset, string data, string keyCoding, string key)
		{
			string result;
			try
			{
				byte[] bytes = Encoding.GetEncoding(keyCoding).GetBytes(key);
				byte[] rgbIV = bytes;
				byte[] array = this.Base64Decode(data);
				var descryptoServiceProvider = DES.Create();
				descryptoServiceProvider.Mode = CipherMode.ECB;
				descryptoServiceProvider.Padding = PaddingMode.PKCS7;
				MemoryStream memoryStream = new MemoryStream();
				CryptoStream cryptoStream = new CryptoStream(memoryStream, descryptoServiceProvider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write);
				cryptoStream.Write(array, 0, array.Length);
				cryptoStream.FlushFinalBlock();
				result = Encoding.GetEncoding(dataCharset).GetString(memoryStream.ToArray());
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002800 File Offset: 0x00000A00
		private string Base64Encode(byte[] bytes)
		{
			string result = string.Empty;
			try
			{
				result = Convert.ToBase64String(bytes);
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000283C File Offset: 0x00000A3C
		private byte[] Base64Decode(string source)
		{
			byte[] result = null;
			try
			{
				result = Convert.FromBase64String(source);
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x04000031 RID: 49
		private string tdesKey = "12345678";

		// Token: 0x04000032 RID: 50
		private string UTF_16BE = "utf-16BE";

		// Token: 0x04000033 RID: 51
		private string ISO_8859_1 = "ISO-8859-1";
	}
	internal class MessageDigest_MD5
	{
		// Token: 0x06000020 RID: 32 RVA: 0x000028A0 File Offset: 0x00000AA0
		protected internal string Md5_32(string src)
		{
			var md = MD5.Create();
			byte[] bytes = Encoding.UTF8.GetBytes(src);
			byte[] array = md.ComputeHash(bytes);
			string text = "";
			for (int i = 0; i < array.Length; i++)
			{
				text += array[i].ToString("x2");
			}
			return text;
		}
	}
	internal class RSACryptoServiceProviderExtension
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002408 File Offset: 0x00000608
		private static void LoadPublicKeyDER(RSACryptoServiceProvider provider, byte[] DERData)
		{
			byte[] rsafromDER = RSACryptoServiceProviderExtension.GetRSAFromDER(DERData);
			byte[] publicKeyBlobFromRSA = RSACryptoServiceProviderExtension.GetPublicKeyBlobFromRSA(rsafromDER);
			provider.ImportCspBlob(publicKeyBlobFromRSA);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000242C File Offset: 0x0000062C
		internal static void LoadPublicKeyPEM(RSACryptoServiceProvider provider, string sPEM)
		{
			byte[] derfromPEM = RSACryptoServiceProviderExtension.GetDERFromPEM(sPEM);
			RSACryptoServiceProviderExtension.LoadPublicKeyDER(provider, derfromPEM);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000244C File Offset: 0x0000064C
		private static byte[] GetPublicKeyBlobFromRSA(byte[] RSAData)
		{
			byte[] array = null;
			uint num = 0U;
			if (!RSACryptoServiceProviderExtension.CryptDecodeObject((RSACryptoServiceProviderExtension.CRYPT_ENCODING_FLAGS)65537U, new IntPtr(19), RSAData, (uint)RSAData.Length, RSACryptoServiceProviderExtension.CRYPT_DECODE_FLAGS.NONE, array, ref num))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			array = new byte[num];
			if (!RSACryptoServiceProviderExtension.CryptDecodeObject((RSACryptoServiceProviderExtension.CRYPT_ENCODING_FLAGS)65537U, new IntPtr(19), RSAData, (uint)RSAData.Length, RSACryptoServiceProviderExtension.CRYPT_DECODE_FLAGS.NONE, array, ref num))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			return array;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000024C4 File Offset: 0x000006C4
		internal static byte[] GetRSAFromDER(byte[] DERData)
		{
			byte[] array = null;
			byte[] array2 = null;
			uint num = 0U;
			IntPtr zero = IntPtr.Zero;
			if (!RSACryptoServiceProviderExtension.CryptDecodeObject((RSACryptoServiceProviderExtension.CRYPT_ENCODING_FLAGS)65537U, new IntPtr(8), DERData, (uint)DERData.Length, RSACryptoServiceProviderExtension.CRYPT_DECODE_FLAGS.NONE, array, ref num))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			array = new byte[num];
			if (RSACryptoServiceProviderExtension.CryptDecodeObject((RSACryptoServiceProviderExtension.CRYPT_ENCODING_FLAGS)65537U, new IntPtr(8), DERData, (uint)DERData.Length, RSACryptoServiceProviderExtension.CRYPT_DECODE_FLAGS.NONE, array, ref num))
			{
				GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
				try
				{
					RSACryptoServiceProviderExtension.CERT_PUBLIC_KEY_INFO cert_PUBLIC_KEY_INFO = (RSACryptoServiceProviderExtension.CERT_PUBLIC_KEY_INFO)Marshal.PtrToStructure(gchandle.AddrOfPinnedObject(), typeof(RSACryptoServiceProviderExtension.CERT_PUBLIC_KEY_INFO));
					array2 = new byte[cert_PUBLIC_KEY_INFO.PublicKey.cbData];
					Marshal.Copy(cert_PUBLIC_KEY_INFO.PublicKey.pbData, array2, 0, array2.Length);
				}
				finally
				{
					gchandle.Free();
				}
				return array2;
			}
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000025C0 File Offset: 0x000007C0
		internal static byte[] GetDERFromPEM(string sPEM)
		{
			uint num = 0U;
			uint num2;
			uint num3;
			if (!RSACryptoServiceProviderExtension.CryptStringToBinary(sPEM, (uint)sPEM.Length, RSACryptoServiceProviderExtension.CRYPT_STRING_FLAGS.CRYPT_STRING_BASE64HEADER, null, ref num, out num2, out num3))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			byte[] array = new byte[num];
			if (!RSACryptoServiceProviderExtension.CryptStringToBinary(sPEM, (uint)sPEM.Length, RSACryptoServiceProviderExtension.CRYPT_STRING_FLAGS.CRYPT_STRING_BASE64HEADER, array, ref num, out num2, out num3))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			return array;
		}

		// Token: 0x06000011 RID: 17
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptDestroyKey(IntPtr hKey);

		// Token: 0x06000012 RID: 18
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptImportKey(IntPtr hProv, byte[] pbKeyData, uint dwDataLen, IntPtr hPubKey, uint dwFlags, ref IntPtr hKey);

		// Token: 0x06000013 RID: 19
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptReleaseContext(IntPtr hProv, int dwFlags);

		// Token: 0x06000014 RID: 20
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptAcquireContext(ref IntPtr hProv, string pszContainer, string pszProvider, RSACryptoServiceProviderExtension.CRYPT_PROVIDER_TYPE dwProvType, RSACryptoServiceProviderExtension.CRYPT_ACQUIRE_CONTEXT_FLAGS dwFlags);

		// Token: 0x06000015 RID: 21
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptStringToBinary(string sPEM, uint sPEMLength, RSACryptoServiceProviderExtension.CRYPT_STRING_FLAGS dwFlags, [Out] byte[] pbBinary, ref uint pcbBinary, out uint pdwSkip, out uint pdwFlags);

		// Token: 0x06000016 RID: 22
		[DllImport("crypt32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptDecodeObjectEx(RSACryptoServiceProviderExtension.CRYPT_ENCODING_FLAGS dwCertEncodingType, IntPtr lpszStructType, byte[] pbEncoded, uint cbEncoded, RSACryptoServiceProviderExtension.CRYPT_DECODE_FLAGS dwFlags, IntPtr pDecodePara, ref byte[] pvStructInfo, ref uint pcbStructInfo);

		// Token: 0x06000017 RID: 23
		[DllImport("crypt32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptDecodeObject(RSACryptoServiceProviderExtension.CRYPT_ENCODING_FLAGS dwCertEncodingType, IntPtr lpszStructType, byte[] pbEncoded, uint cbEncoded, RSACryptoServiceProviderExtension.CRYPT_DECODE_FLAGS flags, [In][Out] byte[] pvStructInfo, ref uint cbStructInfo);

		// Token: 0x02000005 RID: 5
		internal enum CRYPT_ACQUIRE_CONTEXT_FLAGS : uint
		{
			// Token: 0x04000005 RID: 5
			CRYPT_NEWKEYSET = 8U,
			// Token: 0x04000006 RID: 6
			CRYPT_DELETEKEYSET = 16U,
			// Token: 0x04000007 RID: 7
			CRYPT_MACHINE_KEYSET = 32U,
			// Token: 0x04000008 RID: 8
			CRYPT_SILENT = 64U,
			// Token: 0x04000009 RID: 9
			CRYPT_DEFAULT_CONTAINER_OPTIONAL = 128U,
			// Token: 0x0400000A RID: 10
			CRYPT_VERIFYCONTEXT = 4026531840U
		}

		// Token: 0x02000006 RID: 6
		internal enum CRYPT_PROVIDER_TYPE : uint
		{
			// Token: 0x0400000C RID: 12
			PROV_RSA_FULL = 1U
		}

		// Token: 0x02000007 RID: 7
		internal enum CRYPT_DECODE_FLAGS : uint
		{
			// Token: 0x0400000E RID: 14
			NONE,
			// Token: 0x0400000F RID: 15
			CRYPT_DECODE_ALLOC_FLAG = 32768U
		}

		// Token: 0x02000008 RID: 8
		internal enum CRYPT_ENCODING_FLAGS : uint
		{
			// Token: 0x04000011 RID: 17
			PKCS_7_ASN_ENCODING = 65536U,
			// Token: 0x04000012 RID: 18
			X509_ASN_ENCODING = 1U
		}

		// Token: 0x02000009 RID: 9
		internal enum CRYPT_OUTPUT_TYPES
		{
			// Token: 0x04000014 RID: 20
			X509_PUBLIC_KEY_INFO = 8,
			// Token: 0x04000015 RID: 21
			RSA_CSP_PUBLICKEYBLOB = 19,
			// Token: 0x04000016 RID: 22
			PKCS_RSA_PRIVATE_KEY = 43,
			// Token: 0x04000017 RID: 23
			PKCS_PRIVATE_KEY_INFO
		}

		// Token: 0x0200000A RID: 10
		internal enum CRYPT_STRING_FLAGS : uint
		{
			// Token: 0x04000019 RID: 25
			CRYPT_STRING_BASE64HEADER,
			// Token: 0x0400001A RID: 26
			CRYPT_STRING_BASE64,
			// Token: 0x0400001B RID: 27
			CRYPT_STRING_BINARY,
			// Token: 0x0400001C RID: 28
			CRYPT_STRING_BASE64REQUESTHEADER,
			// Token: 0x0400001D RID: 29
			CRYPT_STRING_HEX,
			// Token: 0x0400001E RID: 30
			CRYPT_STRING_HEXASCII,
			// Token: 0x0400001F RID: 31
			CRYPT_STRING_BASE64_ANY,
			// Token: 0x04000020 RID: 32
			CRYPT_STRING_ANY,
			// Token: 0x04000021 RID: 33
			CRYPT_STRING_HEX_ANY,
			// Token: 0x04000022 RID: 34
			CRYPT_STRING_BASE64X509CRLHEADER,
			// Token: 0x04000023 RID: 35
			CRYPT_STRING_HEXADDR,
			// Token: 0x04000024 RID: 36
			CRYPT_STRING_HEXASCIIADDR,
			// Token: 0x04000025 RID: 37
			CRYPT_STRING_HEXRAW,
			// Token: 0x04000026 RID: 38
			CRYPT_STRING_NOCRLF = 1073741824U,
			// Token: 0x04000027 RID: 39
			CRYPT_STRING_NOCR = 2147483648U
		}

		// Token: 0x0200000B RID: 11
		internal class CRYPT_OBJID_BLOB
		{
			// Token: 0x04000028 RID: 40
			internal uint cbData = default;

			// Token: 0x04000029 RID: 41
			internal IntPtr pbData = default;
		}

		// Token: 0x0200000C RID: 12
		internal class CRYPT_ALGORITHM_IDENTIFIER
		{
			// Token: 0x0400002A RID: 42
			internal IntPtr pszObjId = default;

			// Token: 0x0400002B RID: 43
			internal RSACryptoServiceProviderExtension.CRYPT_OBJID_BLOB Parameters = default;
		}

		// Token: 0x0200000D RID: 13
		private class CRYPT_BIT_BLOB
		{
			// Token: 0x0400002C RID: 44
			internal uint cbData = default;

			// Token: 0x0400002D RID: 45
			internal IntPtr pbData = default;

			// Token: 0x0400002E RID: 46
			internal uint cUnusedBits = default;
		}

		// Token: 0x0200000E RID: 14
		private class CERT_PUBLIC_KEY_INFO
		{
			// Token: 0x0400002F RID: 47
			internal RSACryptoServiceProviderExtension.CRYPT_ALGORITHM_IDENTIFIER Algorithm = default;

			// Token: 0x04000030 RID: 48
			internal RSACryptoServiceProviderExtension.CRYPT_BIT_BLOB PublicKey = default;
		}
	}
}

