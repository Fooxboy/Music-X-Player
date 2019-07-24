using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using Microsoft.Advertising.Ads.Requests.AdBroker;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
{
    public class ArtistViewModel:BaseViewModel
    {
        public string NameArtist { get; set; }
        public bool IsLoading { get; set; }
        public List<AudioFile> PopularTracks { get; set; }
        public List<PlaylistFile> Albums { get; set; }
        public string Banner { get; set; }

        public PlaylistFile LastRelease { get; set; } =
            new PlaylistFile
            {
                Artist = "MusicX",
                Cover = "ms-appx:///Assets/Images/placeholder.png",
                Id = -1,
                Name = "MusicX",
                TracksFiles = new List<AudioFile>(),
                IsLocal = true,
                
            };

    public async Task StartLoading(long artistId, string artistName)
        {
            IsLoading = true;
            Changed("IsLoading");
            NameArtist = artistName;
            Changed("NameArtist");
            var artist = await Fooxboy.MusicX.Core.VKontakte.Music.Artists.GetById(artistId);
            NameArtist = artist.Name;
            Changed("NameArtist");
            PopularTracks =  await MusicService.ConvertToAudioFile(artist.PopularTracks);
            Changed("PopularTracks");
            var albums = new List<PlaylistFile>();
            foreach (var plist in artist.Albums) { albums.Add(await PlaylistsService.ConvertToPlaylistFile(plist));}
            Albums = albums;
            Changed("Albums");
            if(artist.Banner != "no")
            {
                //todo: загрузка баннера.
                Banner = await ImagesService.BannerArtist(artist.Banner);
                Changed("Banner");
            }
            else
            {
                //TODO: поставить playholder artist.
            }

            LastRelease = await PlaylistsService.ConvertToPlaylistFile(artist.LastRelease);
            Changed("LastRelease");
            IsLoading = false;
            Changed("IsLoading");
            
        }
    }
}
