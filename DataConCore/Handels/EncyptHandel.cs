using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DataConCore.Handels
{
    public class EncyptHandel
    {

        /// <summary>
        /// MD5
        /// </summary>
        /// <param name="str">Raw string</param>
        /// <returns>returns</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            byte[] encryptdata = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < encryptdata.Length; i++)
                ret += encryptdata[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

        public static string AESEncrypt(string content, string aeskey, string iv)
        {
            try
            {
                byte[] keyByte = GetKeyByte(aeskey, 32);
                byte[] keyByte2 = GetKeyByte(iv, 16);
                return AESEncrypt(content, keyByte, keyByte2);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Decrypt the string using AES
        /// </summary>
        /// <param name="content">content</param>
        /// <param name="aeskey">aeskey</param>
        /// <param name="iv">iv</param>
        /// <returns>UTF8 returns</returns>
        public static string AESDecrypt(string content, string aeskey, string iv)
        {
            try
            {
                byte[] _dESKey = GetKeyByte(aeskey, 32);
                byte[] _dESIv = GetKeyByte(iv, 16);
                return AESDecrypt(content, _dESKey, _dESIv);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        /// <summary>
        /// AES Decrypt
        /// </summary>
        /// <param name="strSource">The string to decrypt</param>
        /// <param name="key">key</param>
        /// <param name="iv">iv</param>
        /// <returns>returns</returns>
        private static string AESDecrypt(string strSource, byte[] key, byte[] iv)
        {
            AesCryptoServiceProvider sa = new AesCryptoServiceProvider();
            sa.Key = key;
            sa.IV = iv;
            byte[] byt = Convert.FromBase64String(strSource);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, sa.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private static string AESEncrypt(string strSource, byte[] key, byte[] iv)
        {
            AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider();
            aesCryptoServiceProvider.Key = key;
            aesCryptoServiceProvider.IV = iv;
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] bytes = Encoding.UTF8.GetBytes(strSource);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            cryptoStream.Close();
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        private static byte[] GetKeyByte(string key, int length)
        {
            while (key.Length < length)
            {
                key += "0";
            }

            return Encoding.UTF8.GetBytes(key.Substring(0, length));
        }
    }
}
