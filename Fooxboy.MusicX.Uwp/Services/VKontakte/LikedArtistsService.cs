using Fooxboy.MusicX.Uwp.Enums;
using Fooxboy.MusicX.Uwp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Fooxboy.MusicX.Uwp.Services.VKontakte
{
    public static class LikedArtistsService
    {

        public static async Task CreateLikedArtstsFile()
        {
            var localpath = ApplicationData.Current.LocalFolder;
            if (await localpath.TryGetItemAsync("LikedArtists.json") != null) return;
            var jsonModel = new LikedArtists()
            {
                Artists = new List<LikedArtist>()
            };

            var stringJson = JsonConvert.SerializeObject(jsonModel);

            var likedArtistsFile = await localpath.CreateFileAsync("LikedArtists.json");

            await FileIO.WriteTextAsync(likedArtistsFile, stringJson);
        }

        public static async Task<bool> IsLikedArtist(long artistId)
        {
            var artists = (await GetLikedArtists()).Artists;
            return artists.Any(a => a.Id == artistId);
        }

        public static async Task<LikedArtists> GetLikedArtists()
        {
            //TODO
            await CreateLikedArtstsFile();

            var localpath = ApplicationData.Current.LocalFolder;
            var file = await localpath.GetFileAsync("LikedArtists.json");

            var json = await FileIO.ReadTextAsync(file);

            var model = JsonConvert.DeserializeObject<LikedArtists>(json);
            return model;
        }

        public static async Task SaveLikedArtists(LikedArtists artists)
        {
            await CreateLikedArtstsFile();

            var localpath = ApplicationData.Current.LocalFolder;
            var file = await localpath.GetFileAsync("LikedArtists.json");

            var json = JsonConvert.SerializeObject(artists);
            await FileIO.WriteTextAsync(file, json);
        }

        public static async Task RemoveArtist(long artistId)
        {
            var artists = await GetLikedArtists();

            var removedArtist = artists.Artists.SingleOrDefault(a => a.Id == artistId);
            artists.Artists.Remove(removedArtist);

            await SaveLikedArtists(artists);
        }

        public static async Task AddLikedArtist(long artistId, string name, string banner)
        {
            var likedArtist = new LikedArtist()
            {
                Id = artistId,
                Name = name,
                Banner = banner
            };
            var artists = await GetLikedArtists();
            artists.Artists.Add(likedArtist);
            await SaveLikedArtists(artists);
        }
    }
}
