using System;

namespace CodecHelper.Base32Codec {

    public class Base32Encryptor : Base32Object, IEncryptor {

        /// <summary>
        /// base32加密
        /// </summary>
        /// <param name="input">byte array</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] input) {
            if (input == null || input.Length == 0) {
                return null;
            }
            //加密後的字元數
            int resultLength = (input.Length * FixedByteCounter / FixedDrawCounter) % 8 == 0
                ? input.Length * FixedByteCounter / FixedDrawCounter
                : input.Length * FixedByteCounter / FixedDrawCounter + 1;
            //回傳的byte array
            byte[] byteResult = new byte[resultLength];
            //紀錄已使用的byte
            int usedByte = 0;
            //剩餘的位元數
            int reminderBitCounter = 0;
            //取出來使用的byte
            byte reminderByte = 0;
            //輸出的byte
            byte outputByte = 0;
            int counter = 0;

            while (counter < resultLength) {
                if (reminderBitCounter == 0) {
                    reminderByte = input[usedByte];
                    reminderBitCounter = FixedByteCounter;
                    usedByte++;
                }
                //清空
                outputByte &= 0x0;
                //可提取的位元數大於要提取的位元數
                if (FixedDrawCounter < reminderBitCounter) {
                    outputByte = (byte)(reminderByte >> (reminderBitCounter - FixedDrawCounter));
                    //更新可提取的位元數
                    reminderBitCounter -= FixedDrawCounter;
                    //更新剩下可提取的位元組
                    reminderByte &= Convert.ToByte(Math.Pow(2, reminderBitCounter) - 1);
                }
                //可提取的位元數等於要提取的位元數
                else if (FixedDrawCounter == reminderBitCounter) {
                    outputByte = reminderByte;
                    reminderBitCounter = 0;
                }
                //可提取的位元數小於要提取的位元數
                else if (FixedDrawCounter > reminderBitCounter) {
                    //從下一個byte還需要提取的位元數
                    int outputByteReminder = FixedDrawCounter - reminderBitCounter;
                    //取出剩下的位元並留下需補足的空位
                    outputByte |= (byte)(reminderByte << (FixedDrawCounter - reminderBitCounter));
                    //還有byte可提取
                    if (input.Length > usedByte) {
                        //取出新的byte
                        reminderByte = input[usedByte];
                        reminderBitCounter = FixedByteCounter;
                        //從新byte提取不足的位元數
                        outputByte |= (byte)(reminderByte >> (reminderBitCounter - outputByteReminder));
                        //再更新剩餘的可用位元數
                        reminderBitCounter -= outputByteReminder;
                        //提取位元後，更新byte
                        reminderByte &= Convert.ToByte(Math.Pow(2, reminderBitCounter) - 1);
                        usedByte++;
                    }
                }

                byteResult[counter] = outputByte;
                counter++;
            }

            return byteResult;
        }
    }
}