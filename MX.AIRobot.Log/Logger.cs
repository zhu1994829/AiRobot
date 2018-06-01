using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MX.AIRobot.Interface;
using log4net;
using System.Reflection;
using log4net.Config;
using System.IO;

namespace MX.AIRobot.Log
{
    public class Logger : ILogger
    {
        ILog log = LoggingFacility.GetLoggerWithInit(MethodBase.GetCurrentMethod().DeclaringType);

        public void WriteLog(LogLevel level, string message)
        {

            switch (level)
            {
                case LogLevel.Info:
                    log.Info(message);
                    break;
                case LogLevel.Error:
                    log.Error(message);
                    break;
                case LogLevel.Warn:
                    log.Warn(message);
                    break;
                case LogLevel.Fatal:
                    log.Fatal(message);
                    break;
                case LogLevel.Debug:
                    log.Debug(message);
                    break;
                default:
                    throw new Exception("未指定日志等级!");
            }
#if DEBUG
            Console.WriteLine(string.Format("Logger.WriteLog Level:{0}  Message:{1}", level.ToString(), message));
#endif
        }

        public void WriteLog(LogLevel level, Exception ex)
        {
            WriteLog(level, string.Format("异常消息:{0}\n 异常堆栈:{1}\n 异常源:{2}\n ", ex.Message, ex.StackTrace, ex.Source));
        }

        public void WriteLog(LogLevel level, string[] categories, string message)
        {
            switch (level)
            {
                case LogLevel.Info:
                    log.InfoFormat(message, categories);
                    break;
                case LogLevel.Error:
                    log.ErrorFormat(message, categories);
                    break;
                case LogLevel.Warn:
                    log.WarnFormat(message, categories);
                    break;
                case LogLevel.Fatal:
                    log.FatalFormat(message, categories);
                    break;
                case LogLevel.Debug:
                    log.DebugFormat(message, categories);
                    break;
                default:
                    throw new Exception("未指定日志等级!");
            }
#if DEBUG
            Console.WriteLine(string.Format("Logger.WriteLog Level:{0}  Message:{1} categories:{2}", level.ToString(), message, categories));
#endif
        }
    }

    public static class LoggingFacility
    {

        private static bool _loggerIsUp = false;

        public static ILog GetLoggerWithInit(Type declaringType)
        {

            if (_loggerIsUp == false)
            {

                XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Configs\\log4net.config"));
                _loggerIsUp = true;

            }
            return LogManager.GetLogger(declaringType);
        }
    }
}
