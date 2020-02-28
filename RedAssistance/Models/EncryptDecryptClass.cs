using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
namespace RedAssistance.Models
{
    public static class EncryptDecryptClass
    {
        public static string Crypt(string text)
        {
            //byte xorConstant = 0x53;
            //byte[] data = Encoding.UTF8.GetBytes(text);
            //for (int i = 0; i < data.Length; i++)
            //{
            //    data[i] = (byte)(data[i] ^ xorConstant);
            //}
            //string output = Convert.ToBase64String(data);

            string output = "";
            char[] readChar = text.ToCharArray();
            for (int i = 0; i < readChar.Length; i++)
            {
                int no = Convert.ToInt32(readChar[i]) + 10;
                string r = Convert.ToChar(no).ToString();
                output += r;
            }
            return output;
        }
        public static string Decrypt(string text)
        {
            //byte xorConstant = 0x53;
            //byte[] data = Convert.FromBase64String(text);
            //for (int i = 0; i < data.Length; i++)
            //{
            //    data[i] = (byte)(data[i] ^ xorConstant);
            //}
            //string plainText = Encoding.UTF8.GetString(data);

            string output = "";
            char[] readChar = text.ToCharArray();
            for (int i = 0; i < readChar.Length; i++)
            {
                int no = Convert.ToInt32(readChar[i]) - 10;
                string r = Convert.ToChar(no).ToString();
                output += r;
            }
            return output;
        }
    }
}
