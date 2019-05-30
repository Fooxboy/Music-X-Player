using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Utils.Extensions;
using Fooxboy.MusicX.Uwp.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class PlayerMenuView : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        public PlayerMenuView()
        {
            this.InitializeComponent();
            PlayerViewModel = ViewModels.PlayerViewModel.Instanse;
            PlayerMenuViewModel = ViewModels.PlayerMenuViewModel.Instanse;
        }

        public PlayerViewModel PlayerViewModel { get; set; }

        public PlayerMenuViewModel PlayerMenuViewModel { get; set; }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var lastPlayMusic = await MusicFilesService.GetLastPlayAudio();
            
            StaticContent.AudioService.CurrentPlaylist.CurrentItem = lastPlayMusic;
            if (StaticContent.AudioService.IsPlaying) StaticContent.AudioService.Pause();
        }

        private async void scrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            bool backscroll = false;
            timer.Tick += (ss, ee) =>
            {
                if (timer.Interval.Ticks == 300)
                {
                    //each time set the offset to scrollviewer.HorizontalOffset + 5

                    if (backscroll == false)
                    {
                        scrollviewer.ScrollToHorizontalOffset(scrollviewer.HorizontalOffset + 2);
                        if (scrollviewer.HorizontalOffset == scrollviewer.ScrollableWidth)
                            backscroll = true;
                    }
                    //if the scrollviewer scrolls to the end, scroll it back to the start.
                    if(backscroll == true) { 
                        scrollviewer.ScrollToHorizontalOffset(scrollviewer.HorizontalOffset - 2);
                        if (scrollviewer.HorizontalOffset == 0)
                            backscroll = false;
                    }
                }
            };
            timer.Interval = new TimeSpan(300);
            timer.Start();
        }

        private async void scrollviewer_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
    }
}
