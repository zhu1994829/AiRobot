using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MX.AIRobot.Util
{
    public static class ObjectHelper
    {
        /// <summary>
        /// 对象转decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal? ObjectToDecimal(this object obj)
        {
            decimal? result = null;
            try
            {
                result = Convert.ToDecimal(obj);
            }
            catch
            {
                //throw new Exception();
            }
            return result;
        }

        /// <summary>
        /// 对象转Int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int? ObjectToInt(this object obj)
        {
            int? result = null;
            try
            {
                result = Convert.ToInt32(obj.ToString());
            }
            catch
            {
                //throw new Exception();
            }
            return result;
        }

        public static double? ObjectToDouble(this object obj)
        {
            double? result = null;
            try
            {
                result = Convert.ToDouble(obj);
            }
            catch
            {
                //throw new Exception();
            }
            return result;
        }

        /// <summary>
        /// 对象转DateTime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? ObjectToDateTime(this object obj)
        {
            DateTime? result = null;
            try
            {
                result = Convert.ToDateTime(obj);
            }
            catch
            {
                //throw new Exception();
            }
            return result;
        }

        /// <summary>
        /// 日起转String
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="FormatType"></param>
        /// <returns></returns>
        public static string DateTimeToString(this DateTime? obj, string FormatType = "yyyy-MM-dd HH:mm:ss")
        {
            string result = string.Empty;
            try
            {
                result = obj.Value.ToString(FormatType);
            }
            catch
            {
                result = DateTime.MinValue.ToString(FormatType);
                //throw new Exception();
            }
            return result;
        }

        /// <summary>
        /// 日起转String
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="FormatType"></param>
        /// <returns></returns>
        public static string DateTimeToString(this DateTime obj, string FormatType = "yyyy-MM-dd HH:mm:ss")
        {
            string result = string.Empty;
            try
            {
                result = obj.ToString(FormatType);
            }
            catch
            {
                result = DateTime.MinValue.ToString(FormatType);
                //throw new Exception();
            }
            return result;
        }

        /// <summary>
        /// 对象转字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToString(this object obj)
        {
            string result = null;
            try
            {
                result = obj.ToString();
            }
            catch
            {
                //throw new Exception();
            }
            return result;
        }

        /// <summary>
        /// 字符串转decimal
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal? StringToDecimal(this string str)
        {
            decimal? result = null;
            try
            {
                result = Convert.ToDecimal(str);
            }
            catch
            {
                //throw new Exception();
            }
            return result;
        }

        public static long? ObjectToInt64(this object obj)
        {
            long? result = null;
            try
            {
                result = Convert.ToInt64(obj);
            }
            catch
            {
                //throw new Exception();
            }
            return result;
        }

        //
        // 摘要:
        //     连接两个序列。
        //
        // 参数:
        //   first:
        //     要连接的第一个序列。
        //
        //   second:
        //     要与第一个序列连接的序列。
        //
        // 类型参数:
        //   TSource:
        //     输入序列中的元素的类型。
        //
        // 返回结果:
        //     一个 System.Collections.Generic.IEnumerable<T>，包含两个输入序列的连接元素。
        //
        // 异常:
        //   System.ArgumentNullException:
        //     first 或 second 为 null。
        public static IEnumerable<TSource> ConcatSwithNotNull<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            if (second != null)
            {
                first = first.Concat(second);
            }
            return first;
        }

        /// <summary>
        /// 获取请求完整域名或ip地址端口号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetRequestAuthPath(this HttpRequest request)
        {
            return request.Url.Scheme + "://" + request.Url.Authority;
        }

        /// <summary>
        /// 获取请求完整域名或ip地址端口号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetRequestAuthPath(this HttpRequestBase request)
        {
            return request.Url.Scheme + "://" + request.Url.Authority;
        }

        /// <summary>
        /// 附件大小格式转换
        /// </summary>
        /// <param name="attachmentSize">附件大小(Byte)</param>
        /// <returns></returns>
        public static string FormatAttachmentSize(int? attachmentSize)
        {
            if (attachmentSize != null)
            {
                if (attachmentSize / 1024 >= 1)
                {
                    if (attachmentSize / (1024 * 1024) >= 1)
                    {
                        return Math.Round((decimal)attachmentSize / (1024 * 1024)) + "M";
                    }
                    else
                    {
                        return Math.Round((decimal)attachmentSize / 1024) + "K";
                    }
                }
                else
                {
                    return Math.Round((decimal)attachmentSize) + "B";
                }
            }
            else
            {
                return null;
            }
        }
    }
}
