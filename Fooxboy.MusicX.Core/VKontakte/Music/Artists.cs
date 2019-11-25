using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;
using Fooxboy.MusicX.Core.Models.Music.ArtistInfo;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;
using Newtonsoft.Json;
using VkNet;
using VkNet.Utils;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public class Artists
    {
        private readonly VkApi _api;
        public Artists(VkApi api)
        {
            _api = api;
        }
        public async Task<IArtist> GetAsync(long artistId)
        {
            var parameters = new VkParameters
            {
                {"artist_id", artistId}, {"extended", 1}, {"access_token", _api.Token}, {"v", "5.101"}
            };
            var json = await _api.InvokeAsync("audio.getCatalog", parameters);
            var r = JsonConvert.DeserializeObject<Response>(json);
            var response = r.response;
            IArtist artist = new Artist();
            artist.Blocks = new List<IBlock>();
            
            foreach(var block in response.Items)
            {
                if (block.Source == "artist_info")
                {
                    artist.Name = block.Artist.Name;
                    artist.Domain = block.Artist.Domain;
                    artist.Id = block.Artist.Id;
                    if (block.Artist.Photo != null)
                    {
                        try
                        {
                            artist.Banner = block.Artist.Photo[2].Url.ToString();
                        }catch{}
                    }
                }
                artist.Blocks.Add(block.ConvertToIBlock());
            }

            return artist;
        }
        
        public IArtist Get(long artistId)
        {
            var parameters = new VkParameters
            {
                {"artist_id", artistId}, {"extended", 1}, {"access_token", _api.Token}, {"v", "5.101"}
            };
            var json =  _api.Invoke("audio.getCatalog", parameters);
            var r = JsonConvert.DeserializeObject<Response>(json);
            var response = r.response;
            IArtist artist = new Artist();
            artist.Blocks = new List<IBlock>();
            
            foreach(var block in response.Items)
            {
                if (block.Source == "artist_info")
                {
                    artist.Name = block.Artist.Name;
                    artist.Domain = block.Artist.Domain;
                    artist.Id = block.Artist.Id;
                    if (block.Artist.Photo != null)
                    {
                        try
                        {
                            artist.Banner = block.Artist.Photo[2].Url.ToString();
                        }catch{}
                    }
                }
                artist.Blocks.Add(block.ConvertToIBlock());
            }
            return artist;
        }

        
    }
}
