using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace Fooxboy.MusicX.Core
{
    public static class Log
    {
        public static void Run()
        {
            StaticContent.Logger = LogManager.GetCurrentClassLogger();
        }

        public static void Trace(string text)
        {
            StaticContent.Logger.Trace(text);
        }

        public static void Error(Exception e)
        {
            StaticContent.Logger.Error(e);
        }

        public static void Debug(string text)
        {
            StaticContent.Logger.Debug(text);
        }

        public static void Info(string text)
        {
            StaticContent.Logger.Info(text);
        }

        public static NLog.Logger GetCurrentLogger() => StaticContent.Logger;

    }
}
