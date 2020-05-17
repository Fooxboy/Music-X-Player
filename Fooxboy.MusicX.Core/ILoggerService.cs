using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core
{
    public interface ILoggerService
    {
        void Info(object msg);
        void Trace(object msg);
        void Error(object msg, Exception e);
    }
}
