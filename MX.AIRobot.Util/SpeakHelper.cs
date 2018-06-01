using MX.AIRobot.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MX.AIRobot.Util
{
  public class SpeakHelper
    {
        /// <summary>
        /// 图灵机器人识别接口
        /// </summary>
        /// <param name="ans">与图灵对话的字符串</param>
        /// <returns>图灵回复</returns>
        public static TuLingResult ConnectTu(string ans)
        {
            TuLingResult tuLingResult = new TuLingResult();
            string res = string.Empty;
            string APIKEY = "32b17e3aa352423dbb6dfbc93b90f657";
            if (!string.IsNullOrEmpty(ans))
            {
                WebRequest wq = WebRequest.Create("http://www.tuling123.com/openapi/api?key=" + APIKEY + "&info=" + ans);
                WebResponse wp = wq.GetResponse();
                res = new StreamReader(wp.GetResponseStream()).ReadToEnd();
                tuLingResult = JsonConvert.DeserializeObject<TuLingResult>(res);
            }
            return tuLingResult;
        }

        /// <summary>
        /// 百度语音合成接口
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
                System.IO.File.WriteAllBytes(filename, result.Data);
            }
        }

        /// <summary>
        /// 百度语音识别接口
        /// </summary>
        /// <param name="APP_ID"></param>
        /// <param name="API_KEY"></param>
        /// <param name="SECRET_KEY"></param>
        private void AsrData(string APP_ID, string API_KEY, string SECRET_KEY, string OutputFile, HttpContext context)
        {
            var client = new Baidu.Aip.Speech.Asr(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间
            var data = System.IO.File.ReadAllBytes(OutputFile);
            client.Timeout = 120000; // 若语音较长，建议设置更大的超时时间. ms
            var result = client.Recognize(data, "pcm", 16000);
            context.Response.ContentType = "application/Json";
            context.Response.Write(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 录音文件格式转换
        /// </summary>
        /// <param name="inputFile"></param>
        private void AudioConvert(string inputFile, string outputFile)
        {
            Process process = new Process();
            try
            {
                process.StartInfo.FileName = @"D:\baiduSpeak\PostFile\ffmpeg-20180508-293a6e8-win64-static\bin\ffmpeg.exe";  // 这里也可以指定ffmpeg的绝对路径
                process.StartInfo.Arguments = " -i " + inputFile + " -y " + outputFile;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardError = true;
                process.ErrorDataReceived += new DataReceivedEventHandler(Output);  // 捕捉ffmpeg.exe的错误信息
                DateTime beginTime = DateTime.Now;
                process.Start();
                process.BeginErrorReadLine();   // 开始异步读取
                process.WaitForExit();    // 等待转码完成

                if (process.ExitCode == 0)
                {
                    int exitCode = process.ExitCode;
                    DateTime endTime = DateTime.Now;
                    TimeSpan t = endTime - beginTime;
                    double seconds = t.TotalSeconds;
                }
                // ffmpeg.exe 发生错误
                else
                {
                    Console.WriteLine("\nffmpeg.exe 程序发生错误，转码失败！");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n错误！！" + ex.ToString());
            }
            finally
            {
                process.Close();
            }
        }

        private void Output(object sendProcess, DataReceivedEventArgs output)
        {
            Process p = sendProcess as Process;
            if (p.HasExited && p.ExitCode == 1) // 在ffmpeg发生错误的时候才输出信息
            {
                Console.WriteLine(output.Data);
            }
        }

        //语音识别
        public void SaveToRead(HttpContext context)
        {
            if (context.Request.Files.Count < 1)
            {
                return;
            }
            //获取上传的文件，保存到 应用程序基目录
            HttpPostedFile file = context.Request.Files[0];
            string sFileName = System.AppDomain.CurrentDomain.BaseDirectory + "\\PostFile\\" + file.FileName;
            file.SaveAs(sFileName);
            string outputFile = sFileName.Substring(0, sFileName.LastIndexOf('.')) + ".wav";
            AudioConvert(sFileName, outputFile);
            AsrData("10849438", "psCaOeZoGfFBUMQbGhsHew0y", "4rvVnZfTBYGDUKdVkSyKrMKrDSSRN8cl", outputFile, context);
        }
    }
}
