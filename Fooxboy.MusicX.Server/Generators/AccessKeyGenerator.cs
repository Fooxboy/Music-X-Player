using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Server.Generators
{
    public class AccessKeyGenerator:IGenerator<string, string[]>
    {

        public string Generate(string[] data)
        {
            string login = data[0];
            var r = new Random();
            string key = $"{login}_{r.Next(1,124)}{r.Next(1, 124)}{r.Next(1, 124)}{r.Next(1, 124)}{r.Next(1, 124)}{r.Next(1, 124)}{r.Next(1, 124)}{r.Next(1, 124)}";
            return key;
        }
    }
}
