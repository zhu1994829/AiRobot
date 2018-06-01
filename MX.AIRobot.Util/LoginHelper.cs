using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace MX.AIRobot.Util
{
    public class LoginHelper
    {

        /// <summary>
        /// 获取本地服务器的外网IP
        /// </summary>
        public string UserInternetIP
        {
            get
            {
                string ip = string.Empty;
                try
                {
                    string url = "http://ip.qq.com/";
                    WebClient wc = new WebClient();
                    wc.Credentials = CredentialCache.DefaultCredentials;
                    Byte[] pageData = wc.DownloadData(url);
                    string MyUrl = Encoding.UTF8.GetString(pageData);
                    Regex regex = new Regex(@"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))");
                    foreach (Match m in regex.Matches(MyUrl))
                    {
                        ip = m.Value;
                    }
                }
                catch
                {
                    ip = "0:0:0:0";
                }
                return ip;
            }
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        public string UserLocalIP
        {
            get
            {
                HttpRequest request = HttpContext.Current.Request;
                string result = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(result))
                {
                    result = request.ServerVariables["REMOTE_ADDR"];
                }
                if (string.IsNullOrEmpty(result))
                {
                    result = request.UserHostAddress;
                }
                if (string.IsNullOrEmpty(result))
                {
                    result = "0.0.0.0";
                }
                return result;
            }
        }

        /// <summary>
        /// 用户Mac地址
        /// </summary>
        //public string UserMac
        //{
        //    get
        //    {
        //        string mac = string.Empty;
        //        try
        //        {
        //            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
        //            ManagementObjectCollection moc = mc.GetInstances();
        //            foreach (ManagementObject mo in moc)
        //            {
        //                if ((bool)mo["IPEnabled"] == true)
        //                {
        //                    mac += mo["MacAddress"].ToString();
        //                }
        //                mo.Dispose();
        //            }
        //        }
        //        catch
        //        {
        //            mac = "00-00-00-00-00-00";
        //        }
        //        return mac;
        //    }
        //}

        /// <summary>
        /// 用户浏览器信息
        /// </summary>
        public string UserBrowser
        {
            get
            {
                HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
                return bc.Browser + " " + bc.Version;
            }
        }
    }
}
