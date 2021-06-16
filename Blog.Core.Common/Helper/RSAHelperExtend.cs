using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
namespace Blog.Core.Common.Helper
{
    /// <summary>
    /// RSA加解密 使用OpenSSL的私钥加密/公钥解密 (一般不用)
    /// 公私钥请使用openssl生成
    /// </summary>
    public class RSAHelperExtend
    {
        /// <summary> 
        /// 生成公钥与私钥方法
        /// </summary>
        /// <returns></returns>
        public static string[] CreateKey(KeyType keyType)
        {
            string[] sKeys = new String[2];
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            switch (keyType)
            {
                case KeyType.XML:
                    {
                        //私钥
                        sKeys[0] = rsa.ToXmlString(true);
                        //公钥
                        sKeys[1] = rsa.ToXmlString(false);
                    }
                    break;
                case KeyType.PKS8:
                    {
                        //JAVA私钥
                        sKeys[0] = RSAPrivateKeyDotNet2Java(sKeys[0]);
                        //JAVA公钥
                        sKeys[1] = RSAPublicKeyDotNet2Java(sKeys[1]);
                    }
                    break;
                default:
                    break;
            }
            return sKeys;
        }

        /// <summary>
        /// 密钥类型
        /// </summary>
        public enum KeyType
        {
            /// <summary>
            /// xml类型
            /// </summary>
            XML,

            /// <summary>
            /// pks8类型
            /// </summary>
            PKS8
        }


        /// <summary>    
        /// RSA私钥格式转换，.net->java    
        /// </summary>    
        /// <param name="privateKey">.net生成的私钥</param>    
        /// <returns></returns>    
        public static string RSAPrivateKeyDotNet2Java(string privateKey)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(privateKey);
            BigInteger m = new BigInteger(Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Modulus")[0].InnerText));
            BigInteger exp = new BigInteger(Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Exponent")[0].InnerText));
            BigInteger d = new BigInteger(Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("D")[0].InnerText));
            BigInteger p = new BigInteger(Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("P")[0].InnerText));
            BigInteger q = new BigInteger(Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Q")[0].InnerText));
            BigInteger dp = new BigInteger(Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("DP")[0].InnerText));
            BigInteger dq = new BigInteger(Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("DQ")[0].InnerText));
            BigInteger qinv = new BigInteger(Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("InverseQ")[0].InnerText));
            RsaPrivateCrtKeyParameters privateKeyParam = new RsaPrivateCrtKeyParameters(m, exp, d, p, q, dp, dq, qinv);

            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKeyParam);
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetEncoded();
            return Convert.ToBase64String(serializedPrivateBytes);
        }

        /// <summary>    
        /// RSA公钥格式转换，.net->java    
        /// </summary>    
        /// <param name="publicKey">.net生成的公钥</param>    
        /// <returns></returns>    
        public static string RSAPublicKeyDotNet2Java(string publicKey)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(publicKey);
            BigInteger m = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Modulus")[0].InnerText));
            BigInteger p = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Exponent")[0].InnerText));
            RsaKeyParameters pub = new RsaKeyParameters(false, m, p);

            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pub);
            byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
            return Convert.ToBase64String(serializedPublicBytes);
        }


        /// <summary>
        /// RSA私钥格式转换，java->.net
        /// </summary>
        /// <param name="privateKey">java生成的RSA私钥</param>
        /// <returns></returns>
        public static string RSAPrivateKeyJavaToDotNet(string privateKey)
        {
            RsaPrivateCrtKeyParameters privateKeyParam = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));

            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                Convert.ToBase64String(privateKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.PublicExponent.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.P.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.Q.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.DP.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.DQ.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.QInv.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.Exponent.ToByteArrayUnsigned()));

        }

        /// <summary>
        /// RSA公钥格式转换，java->.net
        /// </summary>
        /// <param name="publicKey">java生成的公钥</param>
        /// <returns></returns>
        public static string RSAPublicKeyJavaToDotNet(string publicKey)
        {
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                Convert.ToBase64String(publicKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(publicKeyParam.Exponent.ToByteArrayUnsigned()));
        }


        /// <summary>
        /// 最大加密长度
        /// </summary>
        private static int MAX_ENCRYPT_BLOCK = 245;

        /// <summary>
        /// 最大解密长度
        /// </summary>
        private static int MAX_DECRYPT_BLOCK = 256;


        /// <summary>
        /// 用私钥给数据进行RSA加密
        /// </summary>
        /// <param name="xmlPrivateKey"></param>
        /// <param name="strEncryptString"></param>
        /// <returns></returns>
        public static string PrivateKeyEncrypt(string xmlPrivateKey, string strEncryptString)
        {

            RSAHelper rs = new RSAHelper(RSAType.RSA, Encoding.UTF8, xmlPrivateKey);
            var xml = rs._privateKeyRsaProvider.ToXmlString(true);
            MAX_ENCRYPT_BLOCK = rs._privateKeyRsaProvider.KeySize / 8 - 11;
            //加载私钥
            RSACryptoServiceProvider privateRsa = new RSACryptoServiceProvider();
            privateRsa.FromXmlString(xml);

            //转换密钥
            AsymmetricCipherKeyPair keyPair = DotNetUtilities.GetKeyPair(privateRsa);
            IBufferedCipher c = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding"); //使用RSA/ECB/PKCS1Padding格式

            c.Init(true, keyPair.Private);//第一个参数为true表示加密，为false表示解密；第二个参数表示密钥
            byte[] DataToEncrypt = Encoding.UTF8.GetBytes(strEncryptString);//获取字节

            byte[] cache;
            int time = 0;//次数
            int inputLen = DataToEncrypt.Length;
            int offSet = 0;
            using (MemoryStream outStream = new MemoryStream())
            {
                while (inputLen - offSet > 0)
                {
                    if (inputLen - offSet > MAX_ENCRYPT_BLOCK)
                    {
                        cache = c.DoFinal(DataToEncrypt, offSet, MAX_ENCRYPT_BLOCK);
                    }
                    else
                    {
                        cache = c.DoFinal(DataToEncrypt, offSet, inputLen - offSet);
                    }
                    //写入
                    outStream.Write(cache, 0, cache.Length);

                    time++;
                    offSet = time * MAX_ENCRYPT_BLOCK;
                }
                byte[] resData = outStream.ToArray();

                string strBase64 = Convert.ToBase64String(resData);
                return strBase64;
            }

        }

        /// <summary>
        /// 用公钥给数据进行RSA解密 
        /// </summary>
        /// <param name="xmlPublicKey"> 公钥(XML格式字符串) </param>
        /// <param name="strDecryptString"> 要解密数据 </param>
        /// <returns> 解密后的数据 </returns>
        public static string PublicKeyDecrypt(string xmlPublicKey, string strDecryptString)
        {
            RSAHelper rs = new RSAHelper(RSAType.RSA, Encoding.UTF8, "", xmlPublicKey);
            var xml = rs._publicKeyRsaProvider.ToXmlString(false);
            MAX_DECRYPT_BLOCK = rs._publicKeyRsaProvider.KeySize / 8;

            //加载公钥
            RSACryptoServiceProvider publicRsa = new RSACryptoServiceProvider();
            publicRsa.FromXmlString(xml);
            RSAParameters rp = publicRsa.ExportParameters(false);

            //转换密钥
            AsymmetricKeyParameter pbk = DotNetUtilities.GetRsaPublicKey(rp);

            IBufferedCipher c = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            //第一个参数为true表示加密，为false表示解密；第二个参数表示密钥
            c.Init(false, pbk);

            byte[] DataToDecrypt = Convert.FromBase64String(strDecryptString);
            //byte[] outBytes = c.DoFinal(DataToDecrypt);//解密

            //string strDec = Encoding.UTF8.GetString(outBytes);

            byte[] cache;
            int time = 0;//次数
            int inputLen = DataToDecrypt.Length;
            int offSet = 0;
            using (MemoryStream outStream = new MemoryStream())
            {
                while (inputLen - offSet > 0)
                {
                    if (inputLen - offSet > MAX_DECRYPT_BLOCK)
                    {
                        cache = c.DoFinal(DataToDecrypt, offSet, MAX_DECRYPT_BLOCK);
                    }
                    else
                    {
                        cache = c.DoFinal(DataToDecrypt, offSet, inputLen - offSet);
                    }
                    //写入
                    outStream.Write(cache, 0, cache.Length);

                    time++;
                    offSet = time * MAX_DECRYPT_BLOCK;
                }
                byte[] resData = outStream.ToArray();

                string strDec = Encoding.UTF8.GetString(resData);
                return strDec;
            } 
        }

        /// <summary>
        /// 私钥签名
        /// </summary>
        /// <param name="str">需签名的数据</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="signAlgType">hash算法</param>
        /// <returns>签名后的值</returns>
        public static string Sign(string str, string privateKey, SignAlgType signAlgType)
        {
            //根据需要加签时的哈希算法转化成对应的hash字符节
            byte[] bt = Encoding.UTF8.GetBytes(str);
            byte[] rgbHash = null;
            switch (signAlgType)
            {
                case SignAlgType.SHA1:
                    {
                        var csp = new SHA1CryptoServiceProvider();
                        rgbHash = csp.ComputeHash(bt);
                    }
                    break;
                case SignAlgType.SHA256:
                    {
                        var csp = new SHA256CryptoServiceProvider();
                        rgbHash = csp.ComputeHash(bt);
                    }
                    break;
                case SignAlgType.MD5:
                    {
                        var csp = new MD5CryptoServiceProvider();
                        rgbHash = csp.ComputeHash(bt);
                    }
                    break;
                default:
                    break;
            }
            RSACryptoServiceProvider key = new RSACryptoServiceProvider();

            RSAHelper rs = new RSAHelper(RSAType.RSA, Encoding.UTF8, privateKey);
            var xml = rs._privateKeyRsaProvider.ToXmlString(true);
            key.FromXmlString(xml);
            RSAPKCS1SignatureFormatter formatter = new RSAPKCS1SignatureFormatter(key);
            formatter.SetHashAlgorithm(signAlgType.ToString());//此处是你需要加签的hash算法，需要和上边你计算的hash值的算法一致，不然会报错。
            byte[] inArray = formatter.CreateSignature(rgbHash);
            return Convert.ToBase64String(inArray);
        }

        /// <summary>
        /// 公钥验签
        /// </summary>
        /// <param name="str">待验证的字符串</param>
        /// <param name="sign">加签之后的字符串</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="signAlgType">hash算法</param>
        /// <returns>签名是否符合</returns>
        public static bool Verify(string str, string sign, string publicKey, SignAlgType signAlgType)
        {

            byte[] bt = Encoding.UTF8.GetBytes(str);
            byte[] rgbHash = null;
            switch (signAlgType)
            {
                case SignAlgType.SHA1:
                    {
                        var csp = new SHA1CryptoServiceProvider();
                        rgbHash = csp.ComputeHash(bt);
                    }
                    break;
                case SignAlgType.SHA256:
                    {
                        var csp = new SHA256CryptoServiceProvider();
                        rgbHash = csp.ComputeHash(bt);
                    }
                    break;
                case SignAlgType.MD5:
                    {
                        var csp = new MD5CryptoServiceProvider();
                        rgbHash = csp.ComputeHash(bt);
                    }
                    break;
                default:
                    break;
            }
            RSACryptoServiceProvider key = new RSACryptoServiceProvider();
            RSAHelper rs = new RSAHelper(RSAType.RSA, Encoding.UTF8, "", publicKey);
            var xml = rs._publicKeyRsaProvider.ToXmlString(false);
            key.FromXmlString(xml);
            RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(key);
            deformatter.SetHashAlgorithm(signAlgType.ToString());
            byte[] rgbSignature = Convert.FromBase64String(sign);
            if (deformatter.VerifySignature(rgbHash, rgbSignature))
                return true;
            return false;
        }

        /// <summary>
        /// 签名算法类型
        /// </summary>
        public enum SignAlgType
        {
            /// <summary>
            /// sha1
            /// </summary>
            SHA1,
            /// <summary>
            /// sha256
            /// </summary>
            SHA256,

            /// <summary>
            /// md5
            /// </summary>
            MD5
        }
    }
}