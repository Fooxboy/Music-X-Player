using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Server.Databases.DataModels
{
    public class User
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string CurrentAccountToken { get; set; }
        public string CurrentVkToken { get; set; }
    }
}
