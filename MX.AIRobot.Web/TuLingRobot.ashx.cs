using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;


namespace BaiduSpeak
{
    /// <summary>
    /// TuLingRobot 的摘要说明
    /// </summary>
    public class TuLingRobot : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string Gettext = context.Request["text"];
            context.Response.ContentType = "text/plain";
            ConnectTu(Gettext,context);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        public void ConnectTu(string ans,HttpContext context)
        {
            string res = string.Empty;
            string APIKEY ="32b17e3aa352423dbb6dfbc93b90f657";
            if (!string.IsNullOrEmpty(ans))
            {
                WebRequest wq = WebRequest.Create("http://www.tuling123.com/openapi/api?key=" + APIKEY + "&info=" + ans);
                WebResponse wp = wq.GetResponse();
                res = new StreamReader(wp.GetResponseStream()).ReadToEnd();
            }
            context.Response.ContentType = "application/Json";
            context.Response.Write(res);
        }
    }
}