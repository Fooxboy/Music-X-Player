using System;
using System.Collections.Generic;
using System.Text;
using VkNet;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public class MusicApi
    {
        public MusicApi(VkApi api)
        {
            Albums = new Albums(api);
            Artists = new Artists(api);
            Blocks = new Blocks(api);
            Recommendations = new Recommendations(api);
            Search = new Search(api);
            Tracks= new Tracks(api);
            Catalog = new Catalog(api);
        }

        public Albums Albums { get; }
        public Artists Artists { get; }
        public Blocks Blocks { get; }
        public Recommendations Recommendations { get; }
        public Search Search { get; }
        public Tracks Tracks { get; }

        public Catalog Catalog { get; }
    }
}
