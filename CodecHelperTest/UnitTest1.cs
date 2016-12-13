using CodecHelper;
using CodecHelper.Base32Codec;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace CodecHelperTest {

    [TestClass]
    public class UnitTest1 {
        private IDecryptor decryptor = new Base32Decryptor();
        private IEncryptor encryptor = new Base32Encryptor();
        private Random random = new Random();

        [TestMethod]
        public void TestMethod1() {
            byte[] encryptByteArray = encryptor.Encrypt(Encoding.ASCII.GetBytes("=zasxi10021824 ajfd 22s"));
            string encryptStr = Base32Object.GetEncodeStrResult(encryptByteArray);
            byte[] decryptByteArray = decryptor.Decrypt(encryptStr);
            string decryptStr = Base32Object.GetDecodeStrResult(decryptByteArray);
            Assert.AreEqual("=zasxi10021824 ajfd 22s", decryptStr);
        }

        [TestMethod]
        public void Codec() {
            int inputLength = 20;
            int counter = 1000000000;

            byte[] input = new byte[inputLength];

            for (int i = 0; i < counter; i++) {
                for (int j = 0; j < inputLength; j++) {
                    input[j] = Convert.ToByte(random.Next(0, 127));
                }

                string randomStr = Encoding.ASCII.GetString(input);
                byte[] encryptByteArray = encryptor.Encrypt(input);
                string encryptStr = Base32Object.GetEncodeStrResult(encryptByteArray);
                byte[] decryptByteArray = decryptor.Decrypt(encryptStr);
                string decryptStr = Base32Object.GetDecodeStrResult(decryptByteArray);

                Assert.AreEqual(randomStr, decryptStr);
            }
        }
    }
}