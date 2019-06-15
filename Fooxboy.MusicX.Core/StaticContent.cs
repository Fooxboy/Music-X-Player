using System;
using System.Collections.Generic;
using System.Text;
using VkNet;

namespace Fooxboy.MusicX.Core
{
    public class StaticContent
    {
        public static NLog.Logger Logger { get; set; }
        public static VkApi VkApi { get; set; }
        public static long UserId { get; set; }
    }
}
