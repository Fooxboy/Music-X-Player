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

            foreach(var block in response.Items)
            {
                if(block.Source == "artist_info")
                {
                    artist.Name = block.Artist.Name;
                    artist.Domain = block.Artist.Domain;
                    artist.Id = long.Parse(block.Artist.Id);
                    try
                    {
                        artist.Banner = block.Artist.Photo[2].Url.ToString();
                    }
                    catch
                    {
                        artist.Banner = "no";
                    }
                }else if(block.Source == "artist_top_audios")
                {
                    artist.PopularTracks = block.Audios.ToIAudioFileList();
                    artist.BlockPoularTracksId = block.Id;
                }else if(block.Source == "artist_new_album")
                {
                    var music = await StaticContent.VkApi.Audio.GetAsync(new VkNet.Model.RequestParams.AudioGetParams()
                    {
                        PlaylistId = block.Playlist.Id
                    });

                    IList<IAudioFile> tracks = new List<IAudioFile>();
                    foreach (var track in music) tracks.Add(track.ToIAudioFile());

                    artist.LastRelease = block.Playlist.ToIPlaylistFile(tracks, artist.Name);
                }else if(block.Source == "artist_main_albums")
                {
                    var plists = block.Playlists;
                    var list = new List<IPlaylistFile>();

                    foreach (var plist in plists)
                    {
                        list.Add(plist.ToIPlaylistFile(new List<IAudioFile>(), artist.Name));
                    }

                    artist.BlockAlbumsId = block.Id;
                    artist.Albums = list;
                }else if(block.Source == "artist_pages")
                {
                    //TODO
                }
            }

            return artist;
        }

        public async static Task<List<IAudioFile>> GetPopularTracks(string blockId, long count = 100, long offset=0)
        {
            var block = await Block.GetById(blockId, count, offset);
            var tracks = block.Audios;
            return tracks.ToIAudioFileList();
        }

        public async static Task<List<IPlaylistFile>> GetAlbums(string blockId, long count = 100, long offset = 0)
        {
            var block = await Block.GetById(blockId, count);
            var albums = block.Playlists;
            var plists = new List<IPlaylistFile>();
            foreach (var album in albums)
            {
                plists.Add(album.ToIPlaylistFile(new List<IAudioFile>(), block.Title ));
            }

            return plists;
        }
    }
}
