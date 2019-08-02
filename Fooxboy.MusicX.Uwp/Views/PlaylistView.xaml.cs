using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlaylistView : Page
    {
        public PlaylistView()
        {
            this.InitializeComponent();
            PlaylistViewModel = PlaylistViewModel.Instanse;
        }

        public PlaylistViewModel PlaylistViewModel { get; set; }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null) return;

            var playlist = (PlaylistFile)e.Parameter;
            if(playlist.IsLocal)
            {
                
                if (((PlaylistFile)e.Parameter).Id != 1000)
                {
                    PlaylistViewModel.Playlist = await PlaylistsService.GetById(((PlaylistFile)e.Parameter).Id);
                }
                else
                {
                    PlaylistViewModel.Playlist = (PlaylistFile)e.Parameter;
                }
            }else
            {
                PlaylistViewModel.Playlist = playlist;
            }



            if (playlist.OnRequest) PlaylistViewModel.LoadingTracks();
            
            
        }
    }
}
