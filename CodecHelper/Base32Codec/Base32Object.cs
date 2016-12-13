using System;
using System.Configuration;
using System.Text;

namespace CodecHelper.Base32Codec {

    public abstract class Base32Object {
        protected static readonly string Base32Alphabet = ConfigurationManager.AppSettings["Base32Alphabet"];

        /// <summary>
        /// 每次提取的位元數
        /// </summary>
        protected static readonly int FixedDrawCounter = 5;

        /// <summary>
        /// 每個位元組的位元數
        /// </summary>
        protected static readonly int FixedByteCounter = 8;

        /// <summary>
        /// 把加密位元組陣列轉為字串顯示
        /// </summary>
        /// <param name="input">加密後的byte array</param>
        /// <returns>字串值</returns>
        public static string GetEncodeStrResult(byte[] input) {
            if (input == null) {
                throw new ArgumentNullException("未提供取得加密字串值參數");
            }
            //補pad的數目
            int padLength = input.Length % 8 == 0 ? 0 : 8 - input.Length % 8;

            StringBuilder result = new StringBuilder(input.Length + padLength);

            foreach (byte chr in input) {
                result.Append(Base32Alphabet[chr]);
            }

            while (result.Length < result.Capacity) {
                result.Append("=");
            }

            return result.ToString();
        }

        /// <summary>
        /// 把解密位元組陣列轉為字串顯示
        /// </summary>
        /// <param name="input">解密後的byte array</param>
        /// <returns>字串值</returns>
        public static string GetDecodeStrResult(byte[] input) {
            if (input == null) {
                throw new ArgumentNullException("未提供取得解密字串值參數");
            }

            string result = ASCIIEncoding.ASCII.GetString(input);

            return result;
        }
    }
}