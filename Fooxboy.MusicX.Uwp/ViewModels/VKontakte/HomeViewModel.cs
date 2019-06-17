using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.VKontakte.Music;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using Windows.UI.Xaml.Input;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
{
    public class HomeViewModel
    {
        private static HomeViewModel instanse;
        private long maxCountElements = 0;
        const int countTracksLoading = 20;

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
        }


        public LoadingCollection<AudioFile> Music { get; set; }

        public async Task<List<AudioFile>> GetMoreAudio(CancellationToken token, uint offset)
        {
            var tracks = await Library.Tracks(countTracksLoading, Convert.ToInt32(offset));
            var music = MusicService.ConvertToAudioFile(tracks);
            return music;
        }

        public AudioFile SelectAudio { get; set; }

        public bool HasMoreGetAudio()
        {
            return true;
        }

        public async void MusicListView_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
