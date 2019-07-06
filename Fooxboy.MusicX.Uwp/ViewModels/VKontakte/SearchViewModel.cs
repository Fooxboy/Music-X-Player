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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
{
    public class SearchViewModel:BaseViewModel
    {

        private static SearchViewModel instanse;

        public static SearchViewModel Instanse
        {
            get
            {
                if (instanse == null) instanse = new SearchViewModel();

                return instanse;
            }
        }

        public static void Reset()
        {
            instanse = null;
        }

        private SearchViewModel()
        {
            Music = new LoadingCollection<AudioFile>();
            Music.HasMoreItemsRequested = HasMoreLoading;
            Music.OnMoreItemsRequested = GetMoreAudio;

            playlistCurrent = new PlaylistFile()
            {
                Artist = "",
                Cover = "ms-appx:///Assets/Images/playlist-placeholder.png",
                Id = 666,
                IsLocal = false,
                Name = "Результаты поиска"
            };
        }

        public LoadingCollection<AudioFile> Music { get; set; }

        public AudioFile SelectTrack { get; set; }

        private string request;
        public string Request
        {
            get => request;
            set
            {
                if (request == value) return;
                request = value;
                HeaderText = $"Результаты поиска для {request}";
                Changed("HeaderText");
            }
        }

        public bool IsLoading { get; set; }
        public bool hasLoading = true;

        public string HeaderText { get; set; }
        private PlaylistFile playlistCurrent;


        public async void MusicListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //TODO: проигрование трека ебаный врот
            playlistCurrent.TracksFiles = Music.ToList();

            await MusicService.PlayMusic(SelectTrack, 2, playlistCurrent);
        }

        public async void MusicListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            playlistCurrent.TracksFiles = Music.ToList();

            await MusicService.PlayMusic(SelectTrack, 2, playlistCurrent);
        }

        public async Task<List<AudioFile>> GetMoreAudio(CancellationToken token, uint offset)
        {
            IsLoading = true;
            Changed("IsLoading");
            IList<IAudioFile> tracks = new List<IAudioFile>();
            List<AudioFile> music = new List<AudioFile>();

            if (InternetService.Connected)
            {
                try
                {
                    try
                    {
                        tracks = await Search.Tracks(Request, 20, Music.Count);
                        music = await MusicService.ConvertToAudioFile(tracks);
                    }
                    catch (Flurl.Http.FlurlHttpException)
                    {
                        music = new List<AudioFile>();
                        hasLoading = false;

                        await ContentDialogService.Show(new ErrorConnectContentDialog());
                        InternetService.GoToOfflineMode();
                        return music;
                    }

                    if (music.Count < 20) hasLoading = false;
                    IsLoading = false;
                    Changed("IsLoading");
                    return music;
                }catch(Exception e)
                {
                    hasLoading = false;
                    await ContentDialogService.Show(new ExceptionDialog("Неизвестная ошибка при поиске треков", "Music X не смог получить результаты поиска", e));
                    music = new List<AudioFile>();
                    return music;
                }
               
            }else
            {
                await ContentDialogService.Show(new ErrorConnectContentDialog());
                InternetService.GoToOfflineMode();
                music = new List<AudioFile>();
                hasLoading = false;
                return music;
            }

        }

        public bool HasMoreLoading() => hasLoading;


    }
}
