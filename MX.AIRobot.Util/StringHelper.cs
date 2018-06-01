using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MX.AIRobot.Util
{
    /// <summary>
    /// DES3加密解密
    /// </summary>
    public static class StringHelper
    {
        //private static byte[] key = { 0x11, 0x22, 0x4F, 0x58, (byte) 0x83,
        //    0x10, 0x21, 0x38, 0x21, 0x25, 0x79, 0x51, (byte) 0xCB, (byte) 0xDD,
        //    0x55, 0x66, 0x77, 0x29, 0x79, (byte) 0x98, 0x21, 0x54, 0x36,
        //    (byte) 0xE2 };
        //private static byte[] iv = new byte[] { 0x1A, 0x2B, 0x3C, 0x4D, 0x5E, 0x6F, 0x11, 0x99 };
        private static byte[] key = Encoding.ASCII.GetBytes("6B4E0AC9618772E3F9DC1099");
        private static byte[] iv = Encoding.ASCII.GetBytes("C4659F7A");
        public const string READ_CHARLIST = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const string AES_IV = "1234567890123456"; 
        private const string AES_KEY = "1234567890123456"; 

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string EncryptString(string encryptString)
        {
            try
            {
                byte[] Buffer = Encoding.UTF8.GetBytes(encryptString);
                MemoryStream mStream = new MemoryStream();
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;             //默认值
                tdsp.Padding = PaddingMode.PKCS7;       //默认值

                CryptoStream cStream = new CryptoStream(mStream,
                    tdsp.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write);

                cStream.Write(Buffer, 0, Buffer.Length);
                cStream.FlushFinalBlock();

                byte[] ret = mStream.ToArray();

                cStream.Close();
                mStream.Close();

                return Convert.ToBase64String(ret);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string DecryptString(string decryptString)
        {
            try
            {
                byte[] Buffer = Convert.FromBase64String(decryptString);
                MemoryStream msDecrypt = new MemoryStream(Buffer);
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;
                tdsp.Padding = PaddingMode.PKCS7;

                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    tdsp.CreateDecryptor(key, iv),
                    CryptoStreamMode.Read);

                byte[] fromEncrypt = new byte[Buffer.Length];

                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

                return Encoding.UTF8.GetString(fromEncrypt);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns>随机字符串</returns>
        /// <remarks>
        /// 缺省使用ASCII从33到126共94个字符作为取值范围
        /// </remarks>
        public static string GetRandomString(int length)
        {
            return GetRandomString(length, READ_CHARLIST);
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <param name="charList">字符串取值范围（如果为Null或为空，则返回空字符串）</param>
        /// <returns>随机字符串</returns>
        public static string GetRandomString(int length, string charList)
        {
            if (length <= 0 || CheckEmptyString("charList", charList, false))
            {
                return string.Empty;
            }
            var num = charList.Length;
            var ret = new char[length];
            var rnd = GetRandomBytes(length);
            for (var i = 0; i < rnd.Length; i++)
            {
                ret[i] = charList[rnd[i] % num];
            }
            return new string(ret);
        }

        /// <summary>
        /// 获取随机字节序列
        /// </summary>
        /// <param name="length">字节序列的长度</param>
        /// <returns>字节序列</returns>
        public static byte[] GetRandomBytes(int length)
        {
            return GetRandomBytes(length, false);
        }
        /// <summary>
        /// 获取随机字节序列
        /// </summary>
        /// <param name="length">字节序列的长度</param>
        /// <param name="nonZero">生成的数字是否可为0</param>
        /// <returns>字节序列</returns>
        public static byte[] GetRandomBytes(int length, bool nonZero)
        {
            if (length <= 0)
            {
                return new byte[0];
            }
            var rng = new RNGCryptoServiceProvider();
            var ret = new byte[length];
            if (nonZero)
            {
                rng.GetNonZeroBytes(ret);
            }
            else
            {
                rng.GetBytes(ret);
            }
            return ret;
        }

        /// <summary>
        /// 检查字符串是否为空（null或者长度为0）
        /// </summary>
        /// <param name="argName">字符串名</param>
        /// <param name="argValue">被检查的字符串</param>
        /// <param name="throwError">为空时是否抛出异常</param>
        /// <returns>为空则返回true</returns>
        public static bool CheckEmptyString(string argName, string argValue, bool throwError)
        {
            CheckArgumentNull("argName", argName, true);
            bool ret = string.IsNullOrEmpty(argValue);
            if (ret && throwError)
            {
                throw new ArgumentException("字符串为空", argName);
            }
            return ret;
        }

        /// <summary>
        /// 检查参数是否为空引用（null）
        /// </summary>
        /// <param name="argName">参数名</param>
        /// <param name="argValue">被检查的参数</param>
        /// <param name="throwError">为空引用时是否抛出异常</param>
        /// <returns>为空则返回true</returns>
        public static bool CheckArgumentNull(string argName, object argValue, bool throwError)
        {
            if (argName == null)
            {
                throw new ArgumentNullException("argName");
            }
            if (argValue == null)
            {
                if (throwError)
                {
                    throw new ArgumentNullException(argName);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// MD5两次加密
        /// </summary>
        /// <param name="argInput"></param>
        /// <returns></returns>
        public static string GetMd5Hash(string argInput)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                string hashCode = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(argInput)))
                    .Replace("-", "");
                hashCode = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(hashCode)))
                    .Replace("-", "");
                return hashCode;
            }
        }

        /// <summary>
        /// MD5单次加密
        /// </summary>
        /// <param name="argInput"></param>
        /// <returns></returns>
        public static string GetMd5HashByOnce(string argInput)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                string hashCode = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(argInput)))
                    .Replace("-", "");
                return hashCode;
            }
        }

        /// <summary>
        /// 去掉字符串包含的html标记
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string StripHT(string strHtml)
        {

            string strStyle = Regex.Replace(strHtml, @"<style[\s\S]*?</style>", "");
            strHtml = Regex.Replace(strStyle, @"<[^>]*>", "");
            strHtml = Regex.Replace(strHtml, @"\r", "").Replace("\t", "").Replace("\n", "");
            string[] aryReg =
                {
                    @"<script[^>]*?>.*?</script>",
                    @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                    @"([\r\n])[\s]+",
                    @"&(quot|#34);", 
                    @"&(amp|#38);",
                    @"&(lt|#60);", 
                    @"&(gt|#62);", 
                    @"&(nbsp|#160);", 
                    @"&(iexcl|#161);",
                    @"&(cent|#162);",
                    @"&(pound|#163);",
                    @"&(copy|#169);", 
                    @"&#(\d+);", 
                    @"-->", 
                    @"<!--.*\n"
                };

            string[] aryRep =
                {
                  "", 
                  "", 
                  "", 
                  "\"", 
                  "&",
                  "<", 
                  ">", 
                  "", 
                  "\xa1",  //chr(161),
                  "\xa2",  //chr(162),
                  "\xa3",  //chr(163),
                  "\xa9",  //chr(169),
                  "", 
                  "\r\n",
                  ""
                };

            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, aryRep[i]);
            }
            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");
            return strOutput.Trim();


        }

        /// <summary>
        /// 获取字符串中包含的img标签
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string StripIMG(string strHtml)
        {
            string img = "<[img|IMG].*?src=[\'|\"](.*?(?:[\\.gif|\\.jpg|\\.png]))[\'|\"].*?[\\/]?>";
            string mc = Regex.Match(strHtml, img).Value;
            return mc;
        }

        /// <summary>
        /// 字符串值为null的时候输出空
        /// </summary>
        /// <param name="_input"></param>
        /// <returns></returns>
        public static string ToEmpty(this string _input)
        {
            if (_input == null)
                return "";
            else
                return _input.ToString();
        }

        /// <summary>
        /// 日期月份去0 （2012-01-01变成2012-1-01）
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string FormatMonthForDate(string date)
        {
            var result = string.Empty;
            if (!string.IsNullOrEmpty(date))
            {
                var month = "-"+int.Parse(date.Split('-')[1]);
                result = date.Replace("-" + date.Split('-')[1], month);
            }
            return result;
        }

        /// <summary> 
        /// 获取时间戳 
        /// </summary> 
        /// <returns></returns> 
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="input">加密内容</param>
        /// <param name="key">加密key</param>
        /// <returns></returns>
        public static string EncryptByAES(string input)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(AES_KEY);
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }
                        byte[] bytes = msEncrypt.ToArray();

                        //return ByteArrayToHexString(bytes);

                        return Convert.ToBase64String(bytes);
                    }
                }
            }
        } 
    }
}