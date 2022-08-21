using Blog.Core.Common.Helper;
using Blog.Core.Common.Helper.SM;
using System;
using Xunit;

namespace Blog.Core.Tests.Common_Test
{
    public class SM4Helper_Should
    {

        [Fact]
        public void Encrypt_ECB_Test()
        {
            var plainText = "暗号";

            var sm4 = new SM4Helper();

            Console.Out.WriteLine("ECB模式");
            var cipherText = sm4.Encrypt_ECB(plainText);
            Console.Out.WriteLine("密文: " + cipherText);

            Assert.NotNull(cipherText);
            Assert.Equal("VhVDC0KzyZjAVMpwz0GyQA==", cipherText);
        }

        [Fact]
        public void Decrypt_ECB_Test()
        {
            var cipherText = "Y9ygWexdpuLQjW/qsnZNQw==";

            var sm4 = new SM4Helper();

            Console.Out.WriteLine("ECB模式");
            var plainText = sm4.Decrypt_ECB(cipherText);
            Console.Out.WriteLine("明文: " + plainText);

            Assert.NotNull(plainText);
            Assert.Equal("老张的哲学", plainText);
        }

    }
}
