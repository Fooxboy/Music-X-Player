using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.ViewModels.VKontakte.Blocks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

namespace Fooxboy.MusicX.Uwp.Views.VKontakte.Blocks
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AllPlaylistsView : Page
    {

        public AllPlaylistsViewModel ViewModel { get; set; } 
        public AllPlaylistsView()
        {
            this.InitializeComponent();
            ViewModel = new AllPlaylistsViewModel();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.StartLoading((string)e.Parameter, "Плейлисты");
            //base.OnNavigatedTo(e);
        }

    }
}
