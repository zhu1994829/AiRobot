using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MX.AIRobot.Model;
using System.Collections;
using System.Web;
using System.Text.RegularExpressions;

namespace MX.AIRobot.Util
{
    public static class PageHelper
    {
        #region webconfig配置读取
        /// <summary>
        /// 数据显示的条数
        /// </summary>
        public static int PageSize
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
            }
        }

        /// <summary>
        /// 数据字典配置文件路径
        /// </summary>
        public static string CodeValDictUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["CodeValDict"].ToString();
            }
        }

        /// <summary>
        /// 网站路径
        /// </summary>
        public static string WebSiteUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["WebSiteUrl"].ToString();
            }
        }

        /// <summary>
        /// 是否启用验证码
        /// </summary>
        public static int CheckVerifyCode
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["CheckVerifyCode"].ToString());
            }
        }

        /// <summary>
        /// 数据字典配置文件路径
        /// </summary>
        public static string DomainName
        {
            get
            {
                return ConfigurationManager.AppSettings["DomainName"].ToString();
            }
        }

        /// <summary>
        /// 专利缴费提醒天数
        /// </summary>
        public static int WarnDay
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["WarnDay"].ToString());
            }
        }

        /// <summary>
        /// api开关
        /// </summary>
        public static bool ApiSwitch
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["ApiSwitch"].ToString());
            }
        }

        /// <summary>
        /// api开关
        /// </summary>
        public static bool MailSwitch
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["MailSwitch"].ToString());
            }
        }

        /// <summary>
        /// 专题数据库用户注册开关
        /// </summary>
        public static bool ThematicSwitch
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["ThematicSwitch"].ToString());
            }
        }
        #endregion

        #region 邮件

        /// <summary>
        /// 邮件模板路径
        /// </summary>
        public static string MailTemplateUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["MailTemplate"].ToString();
            }
        }

        /// <summary>
        /// 邮件发送方用户名
        /// </summary>
        public static string MailName
        {
            get
            {
                return ConfigurationManager.AppSettings["MailName"].ToString();
            }
        }

        /// <summary>
        /// 邮件发送方密码
        /// </summary>
        public static string MailPwd
        {
            get
            {
                return ConfigurationManager.AppSettings["MailPwd"].ToString();
            }
        }

        /// <summary>
        /// 邮件发送方SMTP
        /// </summary>
        public static string MailSmtp
        {
            get
            {
                return ConfigurationManager.AppSettings["MailSmtp"].ToString();
            }
        }

        /// <summary>
        /// 邮件发送方显示名称
        /// </summary>
        public static string MailFromName
        {
            get
            {
                return ConfigurationManager.AppSettings["MailFromName"].ToString();
            }
        }

        /// <summary>
        /// 是否开启邮件功能
        /// </summary>
        public static string IsMailOpen
        {
            get
            {
                return ConfigurationManager.AppSettings["IsMailOpen"].ToString();
            }
        }

        /// <summary>
        /// 举报收件人
        /// </summary>
        public static string ReportAddressee
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportAddressee"].ToString();
            }
        }

        /// <summary>
        /// 邮件发送 开关
        /// </summary>
        public static string OpenSendMail
        {
            get
            {
                return ConfigurationManager.AppSettings["OpenSendMail"].ToString();
            }
        }
        /// <summary>
        /// 邮件发送 页头图片地址
        /// </summary>
        public static string HeadImgUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["HeadImgUrl"].ToString();
            }
        }
        #endregion

        #region 不同案件类型 详报到结案等待天数
        /// <summary>
        ///市场清扫
        /// </summary>
        public static string MarketClearing
        {
            get
            {
                return ConfigurationManager.AppSettings["MarketClearing"].ToString();
            }
        }

        /// <summary>
        /// 行政执法案件
        /// </summary>
        public static string AdministrativeEnforcement
        {
            get
            {
                return ConfigurationManager.AppSettings["AdministrativeEnforcement"].ToString();
            }
        }
        /// <summary>
        /// 刑事司法案件
        /// </summary>
        public static string CriminalJustice
        {
            get
            {
                return ConfigurationManager.AppSettings["CriminalJustice"].ToString();
            }
        }

        public static string GetDayByCaseType(string caseType)
        {
            return ConfigurationManager.AppSettings[caseType].ToString();
        }
        #endregion

        #region 手机短信验证码开关
        /// <summary>
        /// 数据字典配置文件路径
        /// </summary>
        public static string OpenSendMsg
        {
            get
            {
                return ConfigurationManager.AppSettings["OpenSendMsg"].ToString();
            }
        }
        #endregion

        /// <summary>
        /// 格式化时间格式
        /// </summary>
        /// <param name="dt">datetime日期格式</param>
        /// <param name="isLong">是否需要时分秒,true:yyyy-MM-dd hh24:mm:ss,false:yyyy-MM-dd</param>
        /// <returns></returns>
        public static string FormatDateTime(DateTime? dt, Boolean isLong = false)
        {
            if (dt == null)
            {
                return "";
            }
            else
            {
                if (isLong)
                    return dt.Value.ToString("yyyy-MM-dd HH:mm:ss");
                else
                    return dt.Value.ToString("yyyy-MM-dd");
            }
        }
        public static string FormatDateTimeMinute(DateTime? dt)
        {
            if (dt == null)
            {
                return "";
            }
            else
            {

                return dt.Value.ToString("yyyy-MM-dd HH:mm");
            }
        }

        /// <summary>
        /// 格式化时间格式
        /// </summary>
        /// <param name="dt">datetime日期格式</param>
        /// <param name="isLong">是否需要时分秒,true:yyyy-MM-dd hh24:mm:ss,false:yyyy-MM-dd</param>
        /// <returns></returns>
        public static string FormatDateTimeForMonth(DateTime? dt, Boolean isLong = false)
        {
            if (dt == null)
            {
                return "";
            }
            else
            {
                    return dt.Value.ToString("yyyy-MM");
            }
        }

        /// <summary>
        /// 获取日期的天数
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="isLong"></param>
        /// <returns></returns>
        public static string FormatDateTimeForDay(DateTime? dt, Boolean isLong = false)
        {
            if (dt == null)
            {
                return "";
            }
            else
            {
                return dt.Value.ToString("dd");
            }
        }
        /// <summary>
        /// 获取英文日期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string FormatEnTime(DateTime dt)
        {
            return dt.ToString("dd") + "<br>" + dt.ToString("MMM", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }
        public static string FormatAmount(int Amount)
        {
            string AmountString=string.Format("{0:N}", Amount).ToString();
            return AmountString.Substring(0, AmountString.IndexOf('.'));
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="inputString">需要截取的字符串</param>
        /// <param name="len">截取长度</param>
        /// <returns>截取后字符</returns>
        public static string CutString(string inputString, int len)
        {
            string tempString = string.Empty;
            if (!string.IsNullOrEmpty(inputString))
            {
                ASCIIEncoding ascii = new ASCIIEncoding();
                int tempLen = 0;
                byte[] s = ascii.GetBytes(inputString);
                for (int i = 0; i < s.Length; i++)
                {
                    tempLen += 1;
                    try
                    {
                        tempString += inputString.Substring(i, 1);
                    }
                    catch
                    {
                        break;
                    }
                    if (tempLen >= len)
                        break;
                }
                //如果截过则加上半个省略号 
                byte[] mybyte = ascii.GetBytes(inputString);
                if (mybyte.Length > len)
                    tempString += "…";
                return tempString;
            }
            else
            {
                return tempString;
            }
        }
        /// <summary>
        /// 截取数组性质的字符串
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="split">分割符号</param>
        /// <param name="len">显示到第几个</param>
        /// <returns></returns>
        public static string CutStringByArrayStr(string inputString, char split, int len)
        {
            var array = inputString.Split(split);
            if (!string.IsNullOrEmpty(inputString) && array.Length > len)
            {
                return string.Join(",", array.Take(len)) + "…";
            }
            else
            {
                return inputString;
            }
        }

        /// <summary>
        ///  计算下一月的值
        /// </summary>
        /// <param name="inMonthDate">当前月 (yyyy-MM)</param>
        /// <returns></returns>
        public static string GetEndMonthDate(string inMonthDate)
        {
            int year = Convert.ToInt32(inMonthDate.Substring(0, 4));
            int month = Convert.ToInt32(inMonthDate.Substring(inMonthDate.IndexOf('-') + 1, 2));
            string rsMonth = string.Empty;
            if (month == 12)
            {
                year += 1;
                month = 1;
            }
            else
            {
                month += 1;
            }
            if (month < 10)
            {
                rsMonth = "0" + month.ToString();
            }
            else
            {
                rsMonth = month.ToString();
            }
            return year.ToString() + "-" + rsMonth;
        }

        #region 附件
        /// <summary>
        /// 允许上传附件的大小
        /// </summary>
        public static int AttachmentSize
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["AttachmentSize"].ToString());
            }
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
        #endregion

        #region 时间判空初始化

        /// <summary>
        /// 初始化 起始时间
        /// </summary>
        /// <param name="dt">待初始化时间</param>
        /// <param name="longTime">true:返回带时分秒；false:返回不带时分秒</param>
        /// <returns></returns>
        public static string StartTimeInit(DateTime? dt, bool longTime = true)
        {
            string startTime = string.Empty;
            if (dt != null)
            {
                if (longTime)
                    startTime = dt.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                else
                    startTime = dt.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                if (longTime)
                    startTime = "1900-01-01" + " 00:00:00";
                else
                    startTime = "1900-01-01";
            }
            return startTime;
        }

        /// <summary>
        /// 初始化 结束时间
        /// </summary>
        /// <param name="dt">待初始化时间</param>
        /// <param name="longTime">true:返回带时分秒；false:返回不带时分秒</param>
        /// <returns></returns>
        public static string EndTimeInit(DateTime? dt, bool longTime = true)
        {
            string endTime = string.Empty;
            if (dt != null)
            {
                if (longTime)
                    endTime = dt.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                else
                    endTime = dt.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                if (longTime)
                    endTime = "2099-12-31" + " 23:59:59";
                else
                    endTime = "2099-12-31";
            }
            return endTime;
        }

        ///<summary>
        /// 取得某月的第一天
        /// </summary>
        /// <param name="datetime">要取得月份第一天的时间</param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }

        //// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的时间</param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }

        #endregion

        /// <summary>
        /// 根据URL地址返回所有参数
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static Hashtable GetURLParameterValue(string Url)
        {
            Hashtable result = new Hashtable();
            //截取出Url中的参数
            if (!string.IsNullOrEmpty(Url))
            {
                string Parameter = Url.Substring(Url.IndexOf('?') + 1);

                string[] Parameters = Parameter.Split('&');

                foreach (string s in Parameters)
                {
                    result.Add(s.Split('=')[0], HttpUtility.UrlDecode(s.Split('=')[1], Encoding.Default));//对参数进行解密
                }
            }

            return result;
        }
        /// <summary>
        /// 格式化文件地址
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FormatFilePath(string path, string fileName)
        {
            if (!path.EndsWith("/"))
                path += "/";
            return System.IO.Path.Combine(path + fileName);
        }

        /// <summary>
        /// 获取页面显示
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static string GetShowType<T>(T t, string levelField, string typeField = "")
        {
            int level = 0;
            var showname = string.Empty;
            foreach (var p in t.GetType().GetProperties())
            {
                if (!string.IsNullOrEmpty(levelField) && levelField == p.Name)
                {
                    var value = p.GetValue(t);
                    level = int.Parse(value.ToString());
                }

                if (!string.IsNullOrEmpty(typeField) && typeField == p.Name)
                {
                    var value = p.GetValue(t);
                    showname = value.ToString();
                }
            }

            StringBuilder build = new StringBuilder();
            for (var i = 0; i < level; i++)
            {
                build.Append("&nbsp;&nbsp;&nbsp;├&nbsp;&nbsp;");
            }
            build.Append(showname);
            return build.ToString();
        }

        /// <summary>
        /// 获取简介
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string StripHT(string strHtml)
        {
            if (!string.IsNullOrEmpty(strHtml))
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
            else
            {
                return string.Empty;
            }

        }

        /// <summary>
        /// 获取文本中的图片路径
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string StripIMG(string strHtml)
        {
            if (!string.IsNullOrEmpty(strHtml))
            {
                string img = "<[img|IMG].*?src=[\'|\"](.*?(?:[\\.gif|\\.jpg|\\.png]))[\'|\"].*?[\\/]?>";
                string mc = Regex.Match(strHtml, img).Value;
                return mc;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 页面图片路径处理
        /// </summary>
        /// <param name="Attachment"></param>
        /// <param name="Picture"></param>
        /// <returns></returns>
        //public static string PictureFilePath(BizAttachment Attachment, PictureType picture = PictureType.nopic)
        //{
        //    string FilePath = string.Empty;
        //    if (Attachment != null && !string.IsNullOrEmpty(Attachment.RelationID))
        //    {
        //        string path = Attachment.AttachmentPath;
        //        if (!string.IsNullOrEmpty(path))
        //        {
        //            FilePath = System.IO.Path.Combine("../../" + path + "/" + Attachment.AttachmentNewName);
        //        }
        //        else
        //        {
        //            return "../../Static/images/" + picture.ToString().ToLower() + ".jpg";
        //        }
        //    }
        //    else
        //    {
        //        return "../../Static/images/" + picture.ToString().ToLower() + ".jpg";
        //    }
        //    return FilePath;
        //}

        //public static string PictureFileCRPath(BizAttachment Attachment = null)
        //{
        //    string FilePath = string.Empty;
        //    if (Attachment != null && !string.IsNullOrEmpty(Attachment.RelationID))
        //    {
        //        string path = Attachment.AttachmentPath;
        //        if (!string.IsNullOrEmpty(path))
        //        {
        //            FilePath = System.IO.Path.Combine("../../" + path + Attachment.AttachmentNewName);
        //        }
        //        else
        //        {
        //            FilePath = "../../Static/images/nopic.jpg";
        //        }
        //    }
        //    else
        //    {
        //        FilePath = "../../Static/images/nopic.jpg";
        //    }
        //    return FilePath;
        //}
    }

}
