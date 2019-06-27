using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IF.Lastfm.Core.Api;

namespace Fooxboy.MusicX.Core.Last_fm
{
    public class main
    {
        public void init()
        {
            StaticContent.LastfmClient = new LastfmClient("", "");
        }
    }
}
