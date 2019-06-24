using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.VKontakte.Music;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
{
    public class RecommendationsViewModel:BaseViewModel
    {
        private static RecommendationsViewModel instanse;
        
        public static RecommendationsViewModel Instanse
        {
            get
            {
                if (instanse == null) instanse = new RecommendationsViewModel();

                return instanse;
            }
        }
        private PlaylistFile playlistCurrent;
        private RecommendationsViewModel()
        {
            Tracks = new LoadingCollection<AudioFile>();
            Tracks.HasMoreItemsRequested = HasMoreLoading;
            Tracks.OnMoreItemsRequested = GetMoreAudio;
            playlistCurrent = new PlaylistFile()
            {
                Artist = "",
                Cover = "ms-appx:///Assets/Images/playlist-placeholder.png",
                Id = 666,
                IsLocal = false,
                Name = "Рекомендации"
            };
        }

        private bool firstLoading = true;
        private bool hasMoreLoading = true;
        public LoadingCollection<AudioFile> Tracks { get; set; }

        public bool HasMoreLoading() => hasMoreLoading;

        public async Task<List<AudioFile>> GetMoreAudio(CancellationToken token, uint offset)
        {
            if(firstLoading)
            {
                IsLoading = true;
                Changed("IsLoading");
            }
            IList<IAudioFile> tracks = new List<IAudioFile>();
            List<AudioFile> music = new List<AudioFile>();
            try
            {
                tracks = await Recommendations.Tracks(20, Tracks.Count);
                music = await MusicService.ConvertToAudioFile(tracks);
            }
            catch (Flurl.Http.FlurlHttpException)
            {
                music = new List<AudioFile>();
                
                //TODO: переход в оффлайн режим
                await ContentDialogService.Show(new ErrorConnectContentDialog());
            }

            if (music.Count < 20) hasMoreLoading = false;

            firstLoading = false;

            IsLoading = false;
            Changed("IsLoading");
            return music;
        }

        public async Task MusicListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (SelectedAudio == null) return;

            playlistCurrent.TracksFiles = Tracks.ToList();

            await MusicService.PlayMusic(SelectedAudio, 2, playlistCurrent);
        }

        public async Task MusicListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SelectedAudio == null) return;

            playlistCurrent.TracksFiles = Tracks.ToList();

            await MusicService.PlayMusic(SelectedAudio, 2, playlistCurrent);
        }


        public bool IsLoading { get; set; }

        public AudioFile SelectedAudio { get; set; }

    }
}
