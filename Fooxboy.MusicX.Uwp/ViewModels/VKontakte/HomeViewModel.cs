using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.VKontakte.Music;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
{
    public class HomeViewModel:BaseViewModel
    {
        private static HomeViewModel instanse;
        private long maxCountElements = 0;
        const int countTracksLoading = 20;
        private bool loadingPlaylists = true;

        public static HomeViewModel Instanse
        {
            get
            {
                if (instanse == null) instanse = new HomeViewModel();

                return instanse;
            }
        }

        

        private HomeViewModel()
        {
            Music = new LoadingCollection<AudioFile>();
            Music.OnMoreItemsRequested = GetMoreAudio;
            Music.HasMoreItemsRequested = HasMoreGetAudio;

            Playlists = new LoadingCollection<PlaylistFile>();
            Playlists.OnMoreItemsRequested = GetMorePlaylist;
            Playlists.HasMoreItemsRequested = HasMoreGetPlaylists;


            VisibilityNoTracks = Visibility.Collapsed;
            VisibilityLoading = Visibility.Visible;
            Changed("VisibilityNoTracks");
            Changed("VisibilityLoading");
            Changed("Music");
        }


        public LoadingCollection<AudioFile> Music { get; set; }
        public LoadingCollection<PlaylistFile> Playlists { get; set; }

        public async Task<List<AudioFile>> GetMoreAudio(CancellationToken token, uint offset)
        {
            var tracks = await Library.Tracks(countTracksLoading, Convert.ToInt32(offset));
            var music = MusicService.ConvertToAudioFile(tracks);
            return music;
        }

        public async Task<List<PlaylistFile>> GetMorePlaylist(CancellationToken token, uint offset)
        {
            var playlistsVk = await Library.Playlists(10, 0);
            var playlists = new List<PlaylistFile>();
            foreach (var playlist in playlistsVk) playlists.Add(PlaylistsService.ConvertToPlaylistFile(playlist));
            return playlists;
        }

        public PlaylistFile SelectPlaylist { get; set; }

        public AudioFile SelectAudio { get; set; }
        public bool HasMoreGetAudio()
        {
            return true;
        }

        public bool HasMoreGetPlaylists()
        {
            return loadingPlaylists;
        }

        public async void MusicListView_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        public Visibility VisibilityNoTracks { get; set; }
        public Visibility VisibilityLoading { get; set; }
        public Visibility VisibilityPlaylists { get; set; }
    }
}
