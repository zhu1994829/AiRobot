using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.IO;

namespace BaiduSpeak
{
    /// <summary>
    /// Upload 的摘要说明
    /// </summary>
    public class Upload : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            SaveToRead(context);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 百度语音识别接口
        /// </summary>
        /// <param name="APP_ID"></param>
        /// <param name="API_KEY"></param>
        /// <param name="SECRET_KEY"></param>
        public void AsrData(string APP_ID, string API_KEY, string SECRET_KEY, string OutputFile, HttpContext context)
        {
            var client = new Baidu.Aip.Speech.Asr(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间
            var data = File.ReadAllBytes(OutputFile);
            client.Timeout = 120000; // 若语音较长，建议设置更大的超时时间. ms
            var result = client.Recognize(data, "pcm", 16000);
            context.Response.ContentType = "application/Json";
            context.Response.Write(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 录音文件格式转换
        /// </summary>
        /// <param name="inputFile"></param>
        public void AudioConvert(string inputFile, string outputFile)
        {
            Process process = new Process();
            try
            {
                process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "PostFile\\ffmpeg\\ffmpeg-20180508-293a6e8-win64-static\\bin\\ffmpeg.exe";// 这里也可以指定ffmpeg的绝对路径
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

        //
        public void SaveToRead(HttpContext context)
        {
            if (context.Request.Files.Count < 1)
            {
                return;
            }
            //获取上传的文件，保存到 应用程序基目录
            HttpPostedFile file = context.Request.Files[0];
            string sFileName =AppDomain.CurrentDomain.BaseDirectory + "\\PostFile\\" + file.FileName;
            file.SaveAs(sFileName);
            string outputFile = sFileName.Substring(0, sFileName.LastIndexOf('.')) + ".wav";
            AudioConvert(sFileName, outputFile);
            AsrData("10849438", "psCaOeZoGfFBUMQbGhsHew0y", "4rvVnZfTBYGDUKdVkSyKrMKrDSSRN8cl", outputFile, context);
        }
    }
}