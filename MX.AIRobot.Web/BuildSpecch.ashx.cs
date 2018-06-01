using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace BaiduSpeak
{
    /// <summary>
    /// BuildSpecch 的摘要说明
    /// </summary>
    public class BuildSpecch : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string Gettext = context.Request["text"];
            var file = Guid.NewGuid().ToString() + ".wav";
            var filename = AppDomain.CurrentDomain.BaseDirectory + "attachfile\\" + file;
            Tts("10849438", "psCaOeZoGfFBUMQbGhsHew0y", "4rvVnZfTBYGDUKdVkSyKrMKrDSSRN8cl", Gettext, filename);
            context.Response.ContentType = "text/plain";
            context.Response.Write(context.Request.Url.Scheme + "://" + context.Request.Url.Host +"/wechat"+ "/attachfile/" + file);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 百度语音合成
        /// </summary>
        /// <param name="APP_ID"></param>
        /// <param name="API_KEY"></param>
        /// <param name="SECRET_KEY"></param>
        /// <param name="Gettext">要合成的文字</param>
        /// <param name="filename">合成后的文件路径</param>
        public void Tts(string APP_ID, string API_KEY, string SECRET_KEY, string Gettext, string filename)
        {
            var client = new Baidu.Aip.Speech.Tts(API_KEY, SECRET_KEY);
            // 可选参数
            var option = new Dictionary<string, object>()
            {
                {"spd", 5}, // 语速
                {"vol", 5}, // 音量
                {"per", 0}  // 发音人:0为女声，1为男声，3为情感合成-度逍遥，4为情感合成-度丫丫
            };
            var result = client.Synthesis(Gettext, option);
            if (result.ErrorCode == 0)  // 或 result.Success
            {
                File.WriteAllBytes(filename, result.Data);
            }
        }
    }
}