using Fooxboy.MusicX.Core.VKontakte.Music;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core.VKontakte
{
    public class Vk
    {
        public Vk()
        {

        }
        public Auth Auth { get; set; }
        public MusicApi Music { get; set; }
    }
}
