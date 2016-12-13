using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodecHelper.Base32Codec {

    public class Base32Decryptor : Base32Object, IDecryptor {

        public byte[] Decrypt(string input) {
            byte newChar = 0x0;
            //填充新字所缺位元數
            int leaNewCharBitCounter = FixedByteCounter;
            //拿來使用的位元組所剩的位元數
            int reminderBitCounter = 0;
            //用剩下的位元組
            byte reminderByte = 0x0;

            List<byte> result = new List<byte>();
            //去掉padding
            input = input.Replace("=", "");
            byte[] byteResult = new byte[input.Length];
            //把base32alphabet結果轉換成byte array
            for (int k = 0; k < input.Length; k++) {
                byteResult[k] = Convert.ToByte(Base32Alphabet.IndexOf(input[k]));
            }

            for (int i = 0; i < byteResult.Length; i++) {
                //空出newChar所需的空間
                newChar |= (reminderByte <<= (FixedByteCounter - reminderBitCounter));
                //埴充newChar所缺的bit數
                leaNewCharBitCounter -= reminderBitCounter;

                if (leaNewCharBitCounter > FixedDrawCounter) {
                    newChar |= (byte)(byteResult[i] << (leaNewCharBitCounter - FixedDrawCounter));
                    leaNewCharBitCounter -= FixedDrawCounter;
                    reminderBitCounter = 0;
                } else if (leaNewCharBitCounter == FixedDrawCounter) {
                    newChar |= byteResult[i];
                    leaNewCharBitCounter = 0;
                    reminderBitCounter = 0;
                } else if (leaNewCharBitCounter < FixedDrawCounter) {
                    newChar |= (byte)(byteResult[i] >> (FixedDrawCounter - leaNewCharBitCounter));
                    reminderBitCounter = FixedDrawCounter - leaNewCharBitCounter;
                    reminderByte = byteResult[i] &= Convert.ToByte(Math.Pow(2, reminderBitCounter) - 1);
                    leaNewCharBitCounter = 0;
                }

                if (leaNewCharBitCounter == 0) {
                    result.Add(newChar);
                    newChar = 0x0;
                    leaNewCharBitCounter = FixedByteCounter;
                }
            }

            return result.ToArray();
        }
    }
}