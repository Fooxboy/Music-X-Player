//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Input;
//using Fooxboy.MusicX.Core.Interfaces;
//using Fooxboy.MusicX.Uwp.Models;
//using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
//using Fooxboy.MusicX.Uwp.Services;
//using Fooxboy.MusicX.Uwp.Services.VKontakte;
//using Fooxboy.MusicX.Uwp.Views.VKontakte.Artist;
//using Microsoft.Advertising.Ads.Requests.AdBroker;
//using PlaylistsService = Fooxboy.MusicX.Uwp.Services.VKontakte.PlaylistsService;
//using Windows.Devices.Radios;
//using Windows.UI.Popups;

//namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
//{
//    public class ArtistViewModel:BaseViewModel
//    {

//        public ArtistViewModel()
//        {
//            ShowPopularTracksCommand = new RelayCommand(() =>
//            {
//                StaticContent.NavigationContentService.Go(typeof(ArtistAllPopularTracks), Artist.BlockPoularTracksId);
//            });

//            ShowAblumsCommand = new RelayCommand((() =>
//            {
//                StaticContent.NavigationContentService.Go(typeof(ArtistAllAlbums), Artist.BlockAlbumsId);
//            }));

//            LikeArtist = new RelayCommand(async() =>
//            {
//                try
//                {
//                    await LikedArtistsService.AddLikedArtist(Artist.Id, Artist.Name, Artist.Banner);
//                    var msg = new MessageDialog("Музыкант был добавлен в избранное!");
//                    await msg.ShowAsync();
//                }catch(Exception e)
//                {
//                    await ContentDialogService.Show(new ExceptionDialog("Невозможно добавить исполнителя в избранное", "Повторите позже, если это не поможет перезапустите приложение.", e));
//                }
//            });
//        }
//        public string NameArtist { get; set; }
//        public bool IsLoading { get; set; }
//        public Visibility PopularTracksVisibility { get; set; } = Visibility.Collapsed;
//        public Visibility LastAlbumVisibility { get; set; } = Visibility.Collapsed;
//        public Visibility AlbumsVisibility { get; set; } = Visibility.Collapsed;

//        public bool EnableAddToLikedArtists { get; set; }
//        public Visibility VisibilityAds => StaticContent.IsPro ? Visibility.Collapsed : Visibility.Visible;
//        public List<AudioFile> PopularTracks { get; set; }
//        public List<PlaylistFile> Albums { get; set; }
//        public string Banner { get; set; }

//        public PlaylistFile LastRelease { get; set; } =
//            new PlaylistFile
//            {
//                Artist = "MusicX",
//                Cover = "ms-appx:///Assets/Images/placeholder.png",
//                Id = -1,
//                Name = "MusicX",
//                TracksFiles = new List<AudioFile>(),
//                IsLocal = true,
                
//            };


//        public IArtist Artist { get; set; }
//        public async Task StartLoading(long artistId, string artistName)
//        {
//            try
//            {
//                IsLoading = true;
//                Changed("IsLoading");
//                NameArtist = artistName;
//                Changed("NameArtist");
//                var artist = await Fooxboy.MusicX.Core.VKontakte.Music.Artists.GetById(artistId);
//                Artist = artist;
//                NameArtist = artist.Name;
//                Changed("NameArtist");
//                var tracks = new List<IAudioFile>();
//                for (int i =0; i<6; i++) tracks.Add(artist.PopularTracks[i]);
                
//                PopularTracks = await MusicService.ConvertToAudioFile(tracks);
//                PopularTracksVisibility = Visibility.Visible;
//                Changed("PopularTracksVisibility");
//                Changed("PopularTracks");
//                var albums = new List<PlaylistFile>();
//                foreach (var plist in artist.Albums)
//                {
//                    albums.Add(await PlaylistsService.ConvertToPlaylistFile(plist));
//                }

//                Albums = albums;
//                AlbumsVisibility = Visibility.Visible;
//                Changed("AlbumsVisibility");
//                Changed("Albums");
//                if (artist.Banner != "no")
//                {
//                    Banner = await ImagesService.BannerArtist(artist.Banner);
//                    Changed("Banner");
//                }
//                else
//                {
//                    //Assets/Images/placeholder-artist.jpg
//                }

//                if(artist.LastRelease != null)
//                {
//                    LastRelease = await PlaylistsService.ConvertToPlaylistFile(artist.LastRelease);
//                    LastAlbumVisibility = Visibility.Visible;
//                    Changed("LastAlbumVisibility");
//                    Changed("LastRelease");
//                }
                
//                if(await LikedArtistsService.IsLikedArtist(artist.Id))
//                {
//                    EnableAddToLikedArtists = false;
//                    Changed("EnableAddToLikedArtists");
//                }else
//                {
//                    EnableAddToLikedArtists = true;
//                    Changed("EnableAddToLikedArtists");
//                }

//                IsLoading = false;
//                Changed("IsLoading");
//            }
//            catch (Exception e)
//            {
//                await ContentDialogService.Show(new ExceptionDialog("Ошибка при загрузке карточки исполнителя",
//                    "Возможно, исполнитель недоступен в Вашей стране или ВКонтакте не вернул необходимую информацию",
//                    e));
//            }
            
            
//        }

//        public AudioFile SelectPopularAudioFile { get; set; }

//        public PlaylistFile SelectPlaylist { get; set; }
//        public RelayCommand ShowPopularTracksCommand { get; set; }
//        public RelayCommand ShowAblumsCommand { get; set; }

//        public RelayCommand LikeArtist { get; set; }

//        public  async void UIElement_OnTappedTracks(object sender, TappedRoutedEventArgs e)
//        {
//            if (SelectPopularAudioFile != null)
//            {
//                var playlistCurrent = new PlaylistFile()
//                {
//                    Artist = NameArtist,
//                    Cover = "ms-appx:///Assets/Images/playlist-placeholder.png",
//                    Id = 666,
//                    IsLocal = false,
//                    Name = "Популярные треки исполнителя"
//                };

//                playlistCurrent.TracksFiles = PopularTracks;

//                await MusicService.PlayMusic(SelectPopularAudioFile, 2, playlistCurrent);
//            }
//        }

//        public void UIElement_OnTappedPlaylist(object sender, TappedRoutedEventArgs e)
//        {
//            if (SelectPlaylist == null) return;
//            StaticContent.NavigationContentService.Go(typeof(Views.PlaylistView), SelectPlaylist);
//        }
//        public void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
//        {
//            if (LastRelease == null) return;
//            StaticContent.NavigationContentService.Go(typeof(Views.PlaylistView), LastRelease);
//            //throw new NotImplementedException();
//        }

//    }
//}
