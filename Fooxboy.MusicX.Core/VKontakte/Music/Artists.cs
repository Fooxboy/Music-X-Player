using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;
using Fooxboy.MusicX.Core.Models.Music.ArtistInfo;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;
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
            var response = await StaticContent.VkApi.CallAsync<Response>("audio.getCatalog", parameters);

            IArtist artist = new ArtistAnyPlatform();

            artist.Name = response.Items[0].Artist.Name;
            artist.Domain = response.Items[0].Artist.Domain;
            artist.Banner = response.Items[0].Artist.Photo[2].Url.ToString();
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
            
            //TODO: сделать треки плейлистов по запросу.
            foreach (var plist in plists)
            {
                var musicPlist = await StaticContent.VkApi.Audio.GetAsync(new VkNet.Model.RequestParams.AudioGetParams()
                {
                    PlaylistId = response.Items[2].Playlist.Id
                });
                IList<IAudioFile> tracksPlist = new List<IAudioFile>();
                foreach (var track in musicPlist) tracksPlist.Add(track.ToIAudioFile());
                list.Add(plist.ToIPlaylistFile(tracksPlist, artist.Name));
            }

            artist.Albums = list;
            return artist;
        }
    }
}
