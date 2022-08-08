using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Blog.Core.Common.Helper.SM
{
    public class SM4Helper
    {
        public String secretKey = "1234567890123456";// 16位
        public String iv = "";
        public bool hexString = false;

        private SByte[] Byte2SByte(byte[] myByte)
        {
            sbyte[] mySByte = new sbyte[myByte.Length];

            for (int i = 0; i < myByte.Length; i++)
            {
                if (myByte[i] > 127)
                    mySByte[i] = (sbyte)(myByte[i] - 256);
                else
                    mySByte[i] = (sbyte)myByte[i];
            }

            return mySByte;
        }
        private byte[] SByte2Byte(sbyte[] orig)
        {
            byte[] arr = new byte[orig.Length];
            Buffer.BlockCopy(orig, 0, arr, 0, orig.Length);

            return arr;
        }

        public String Encrypt_ECB(String plainText)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4.SM4_ENCRYPT;

            SByte[] keyBytes;
            if (hexString)
            {
                keyBytes = null;// Hex.Decode(secretKey);
            }
            else
            {

                keyBytes = Byte2SByte(Encoding.UTF8.GetBytes(secretKey));
            }

            SM4 sm4 = new SM4();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            SByte[] bytes = Byte2SByte(Encoding.UTF8.GetBytes(plainText));
            SByte[] encrypted = sm4.sm4_crypt_ecb(ctx, bytes);

            //String cipherText = Encoding.UTF8.GetString(Hex.Encode(SByte2Byte(encrypted)));
            String cipherText = new Base64Encoder().GetEncoded(SByte2Byte(encrypted));

            if ((cipherText != null) && (cipherText.Trim().Length > 0))
            {
                var matchCol = Regex.Matches(cipherText, "\\s*|\t|\r|\n", RegexOptions.Multiline);
                for (int i = matchCol.Count - 1; i >= 0; i--)
                {
                    Match item = matchCol[i];
                    cipherText.Remove(item.Index, item.Length);
                }
            }
            return cipherText;
        }

        public String Decrypt_ECB(String cipherText)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4.SM4_DECRYPT;

            SByte[] keyBytes;
            if (hexString)
            {
                keyBytes = null;// Hex.Decode(secretKey);
            }
            else
            {
                keyBytes = Byte2SByte(Encoding.UTF8.GetBytes(secretKey));
            }

            SM4 sm4 = new SM4();
            sm4.sm4_setkey_dec(ctx, keyBytes);
            SByte[] decrypted = sm4.sm4_crypt_ecb(ctx, Byte2SByte(new Base64Decoder().GetDecoded(cipherText)));
            return Encoding.UTF8.GetString(SByte2Byte(decrypted));
        }

    }
}
