using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Server.Generators
{
    public class LogGenerator:IGenerator<string, string>
    {
        public string Generate(string data)
        {
            return "ebat log";
        }
    }
}
