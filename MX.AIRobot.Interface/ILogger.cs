using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MX.AIRobot.Interface
{
    public enum LogLevel
    {
        Error,
        Fatal,
        Info, 
        Debug, 
        Warn
    }

    public interface ILogger
    {
        void WriteLog(LogLevel level, string message);
        void WriteLog(LogLevel level, Exception x);
        void WriteLog(LogLevel level, string[] categories, string message);
 
    }


}
