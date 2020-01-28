//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.UI.Xaml.Input;
//using Fooxboy.MusicX.Uwp.Models;
//using Fooxboy.MusicX.Uwp.Services.VKontakte;

//namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte.Artist
//{
//    public class ArtistAllPopularTracksViewModel:BaseViewModel
//    {

//        public ArtistAllPopularTracksViewModel()
//        {
//            Tracks = new ObservableCollection<AudioFile>();
//            SelectPlaylist = new PlaylistFile()
//            {
//                Artist = "",
//                Cover = "ms-appx:///Assets/Images/playlist-placeholder.png",
//                Id = 990,
//                IsLocal = false,
//                Name = "Попопулярные треки исполнителя"
//            };
//        }

//        public ObservableCollection<AudioFile> Tracks { get; set; }
//        public PlaylistFile SelectPlaylist { get; set; }
//        public AudioFile SelectTrack { get; set; }
//        public bool IsLoading { get; set; }


//        public async Task StartLoading(string blockId)
//        {
//            IsLoading = true;
//            Changed("IsLoading");
//            var tracksVk = await MusicX.Core.VKontakte.Music.Artists.GetPopularTracks(blockId);
//            var tracks = await MusicService.ConvertToAudioFile(tracksVk);
//            Tracks = new ObservableCollection<AudioFile>(tracks);
//            Changed("Tracks");
//            IsLoading = false;
//            Changed("IsLoading");
//        }

//        public async void MusicListView_OnTapped(object sender, TappedRoutedEventArgs e)
//        {
//            if (SelectTrack == null) return;

//            SelectPlaylist.TracksFiles = Tracks.ToList();

//            await MusicService.PlayMusic(SelectTrack, 2, SelectPlaylist);
//            //throw new NotImplementedException();
//        }
//    }
//}
