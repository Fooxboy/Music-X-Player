using Fooxboy.MusicX.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MethodsTest
{
    public class LoggerService : ILoggerService
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

        public async Task SaveLog()
        {
            
        }


        private void Write(string msg)
        {
            var s = $"({DateTime.Now}){msg}";
            Debugger.Log(1, null, s);
        }

    }
}
