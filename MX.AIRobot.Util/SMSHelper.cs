using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using MX.AIRobot.Model;

namespace MX.AIRobot.Util
{
    public class SMSHelper
    {
        #region 发送短信参数
        /// <summary>
        /// 请求的api编号
        /// </summary>
        protected static string apikey = ConfigurationManager.AppSettings["ApiKey"].ToString();
        /// <summary>
        /// 请求的url地址
        /// </summary>
        protected static string apiurl = ConfigurationManager.AppSettings["ApiUrl"].ToString();
        /// <summary>
        /// 应用场景号(需要申请)
        /// </summary>
        protected static string profileCode = ConfigurationManager.AppSettings["ProfileCode"].ToString();
        /// <summary>
        /// 注册模板编号(需要申请)
        /// </summary>
        protected static string registerTemplateCode = ConfigurationManager.AppSettings["RegisterTemplateCode"].ToString();
        /// <summary>
        /// 找回密码模板编号(需要申请)
        /// </summary>
        protected static string findPasswordTemplateCode = ConfigurationManager.AppSettings["FindPasswordTemplateCode"].ToString();
        /// <summary>
        /// 修改手机号模板编号(需要申请)
        /// </summary>
        protected static string updatePhoneTemplateCode = ConfigurationManager.AppSettings["UpdatePhoneTemplateCode"].ToString();
        #endregion


        /// <summary>
        /// 获取模板内容
        /// </summary>
        /// <param name="id">模板编号</param>
        /// <returns></returns>
        protected static string GetSMSBody(string id)
        {
            XDocument doc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["SMSUrl"].ToString());
            return doc.Root.Elements("Template").FirstOrDefault(f => f.Attribute("id").Value == id).Value;

        }

        /// <summary>
        /// 发送短信验证码服务
        /// </summary>
        /// <param name="telephoneNum">手机号（多个以半角逗号分开）</param>
        /// <param name="code">验证码</param>
        /// <param name="tag">结果返回形式（1 xml, 2 json）</param>
        /// <returns></returns>
        public static string SendCodeService(string telephoneNum, string code, string tag)
        {
            StringBuilder apiParam = new StringBuilder();
            apiParam.Append("mobile=" + telephoneNum);
            string content = GetSMSBody("SendCode");
            content = content.Replace("@Code", code);
            content = HttpUtility.UrlEncode(content, Encoding.UTF8);
            apiParam.Append("&content=" + content);
            apiParam.Append("&tag=" + tag);
            string strURL = apiurl + '?' + apiParam;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "GET";
            // 添加header
            request.Headers.Add("apikey", apikey);
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s;
            s = response.GetResponseStream();
            string StrDate = "";
            string strValue = "";
            StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            while ((StrDate = Reader.ReadLine()) != null)
            {
                strValue += StrDate + "\r\n";
            }
            return strValue;
        }


    }
}
