using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;
using Fooxboy.MusicX.Core.Models.Music.ArtistInfo;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;
using Newtonsoft.Json;
using VkNet.Utils;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public static class Artists
    {
        public async static Task<IArtist> GetById(long artistId)
        {
            var parameters = new VkParameters();
            parameters.Add("artist_id", artistId);
            parameters.Add("extended", 1);
            parameters.Add("access_token", StaticContent.VkApi.Token);
            parameters.Add("v", "5.101");
            var json = await StaticContent.VkApi.InvokeAsync("audio.getCatalog", parameters);

            var r = JsonConvert.DeserializeObject<Response>(json);
            //var r = await StaticContent.VkApi.CallAsync<Response>("audio.getCatalog", parameters);
            var response = r.response;
            IArtist artist = new ArtistAnyPlatform();

            artist.Name = response.Items[0].Artist.Name;
            artist.Domain = response.Items[0].Artist.Domain;
            try
            {
                artist.Banner = response.Items[0].Artist.Photo[2].Url.ToString();

            }
            catch
            {
                artist.Banner = "no";
            }
            artist.Id = long.Parse(response.Items[0].Artist.Id);
            artist.PopularTracks = (response.Items[1].Audios).ToIAudioFileList();
            
            var music = await StaticContent.VkApi.Audio.GetAsync(new VkNet.Model.RequestParams.AudioGetParams()
            {
                PlaylistId = response.Items[2].Playlist.Id
            });

            IList<IAudioFile> tracks = new List<IAudioFile>();
            foreach (var track in music) tracks.Add(track.ToIAudioFile());

            artist.LastRelease = (response.Items[2].Playlist).ToIPlaylistFile(tracks, artist.Name);

            var plists = response.Items[3].Playlists;
            var list = new List<IPlaylistFile>();
            
            foreach (var plist in plists)
            {
                list.Add(plist.ToIPlaylistFile(new List<IAudioFile>(), artist.Name));
            }

            artist.Albums = list;
            return artist;
        }
    }
}
