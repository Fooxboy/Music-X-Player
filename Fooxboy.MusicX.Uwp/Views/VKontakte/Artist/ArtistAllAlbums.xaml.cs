using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Fooxboy.MusicX.Uwp.ViewModels.VKontakte.Artist;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Views.VKontakte.Artist
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class ArtistAllAlbums : Page
    {

        public ArtistAllAlbumsViewModel ViewModel { get; set; }

        public ArtistAllAlbums()
        {
            this.InitializeComponent();

            ViewModel = new ArtistAllAlbumsViewModel();
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var blockId = (string)e.Parameter;
            await ViewModel.StartLoading(blockId);
            //base.OnNavigatedTo(e);
        }

       
    }
}
