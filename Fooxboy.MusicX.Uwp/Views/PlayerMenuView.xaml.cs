using System;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Utils.Extensions;
using Fooxboy.MusicX.Uwp.ViewModels;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class PlayerMenuView : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timer1 = new DispatcherTimer();
        public PlayerMenuView()
        {
            this.InitializeComponent();
            PlayerViewModel = ViewModels.PlayerViewModel.Instanse;
            PlayerMenuViewModel = ViewModels.PlayerMenuViewModel.Instanse;
            Application.Current.Resuming += AppResuming;
            Application.Current.Suspending += AppSuspending;
        }

        public PlayerViewModel PlayerViewModel { get; set; }

        public PlayerMenuViewModel PlayerMenuViewModel { get; set; }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {

            if(!StaticContent.OpenFiles)
            {
                var lastPlayMusic = await MusicFilesService.GetLastPlayAudio();
                var track = lastPlayMusic.Track;
                if (track != null)
                {
                    try
                    {
                        try
                        {
                            track.Source = await StorageFile.GetFileFromPathAsync(track.SourceString);
                            track.Duration = TimeSpan.FromSeconds(track.DurationSeconds);
                        }
                        catch (Exception)
                        {
                            track.Source = await StorageFile.GetFileFromApplicationUriAsync(new Uri(track.SourceString));
                            track.Duration = TimeSpan.FromSeconds(track.DurationSeconds);
                        }

                        if (lastPlayMusic.Playlist != null)
                        {
                            var playlist = lastPlayMusic.Playlist.ToAudioPlaylist();
                            playlist.CurrentItem = track;
                            StaticContent.AudioService.SetCurrentPlaylist(playlist);
                        }
                        else
                        {
                            StaticContent.AudioService.CurrentPlaylist.CurrentItem = track;
                        }
                    }catch(Exception ee)
                    {
                        await new ExceptionDialog("Ошибка при инициализации страницы", "аоаоаоамм", ee).ShowAsync();
                    }
                    

                    StaticContent.Volume = lastPlayMusic.Volume;
                    if (StaticContent.AudioService.IsPlaying) StaticContent.AudioService.Pause();
                }
            }
        }

        private void trackScroll_Loaded(object sender, RoutedEventArgs e)
        {
            bool backscroll = false;
            timer.Tick += (ss, ee) =>
            {
                if (timer.Interval.Ticks == 300)
                {
                    //each time set the offset to scrollviewer.HorizontalOffset + 5

                    if (backscroll == false)
                    {
                        trackScroll.ScrollToHorizontalOffset(trackScroll.HorizontalOffset + 1);
                        if (trackScroll.HorizontalOffset == trackScroll.ScrollableWidth)
                            backscroll = true;
                    }
                    //if the scrollviewer scrolls to the end, scroll it back to the start.
                    if(backscroll == true) {
                        trackScroll.ScrollToHorizontalOffset(trackScroll.HorizontalOffset - 1);
                        if (trackScroll.HorizontalOffset == 0)
                        {
                            backscroll = false;
                        }
                    }
                }
            };
            timer.Interval = new TimeSpan(300);
            timer.Start();
        }

        private void trackScroll_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void AppResuming(object sender, object e)
        {
            timer.Start();
            timer1.Start();

        }

        private void AppSuspending(object sender, SuspendingEventArgs suspendingEventArgs)
        {
            timer.Stop();
            timer1.Stop();
        }

        private void artistScroll_Loaded(object sender, RoutedEventArgs e)
        {
            bool backscroll = false;
            timer1.Tick += (ss, ee) =>
            {
                if (timer1.Interval.Ticks == 300)
                {
                    //each time set the offset to scrollviewer.HorizontalOffset + 5

                    if (backscroll == false)
                    {
                        artistScroll.ScrollToHorizontalOffset(artistScroll.HorizontalOffset + 1);
                        if (artistScroll.HorizontalOffset == artistScroll.ScrollableWidth)
                            backscroll = true;
                    }
                    //if the scrollviewer scrolls to the end, scroll it back to the start.
                    if (backscroll == true)
                    {
                        artistScroll.ScrollToHorizontalOffset(artistScroll.HorizontalOffset - 1);
                        if (artistScroll.HorizontalOffset == 0)
                        {
                            backscroll = false;
                        }
                    }
                }
            };
            timer1.Interval = new TimeSpan(300);
            timer1.Start();
        }

        private async void artistScroll_Unloaded(object sender, RoutedEventArgs e)
        {
            timer1.Stop();
        }
    }
}
