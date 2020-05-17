using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class LoggerService:ILoggerService
    {
        public void Info(object msg)
        {
            Write($"[INFO]=> {msg}");
        }

        public void Trace(object msg)
        {
            Write($"[TRACE]=> {msg}");

        }

        public void Error(object msg, Exception e)
        {
            Write($"[ERROR]=> {msg} \n EXCEPTION: {e}");

        }

        private void Write(string msg)
        {
            Debug.WriteLine($"({DateTime.Now}){msg}");
        }
    }
}
