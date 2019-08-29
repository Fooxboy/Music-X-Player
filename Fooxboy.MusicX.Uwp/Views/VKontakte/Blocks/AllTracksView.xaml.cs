using Fooxboy.MusicX.Uwp.ViewModels.VKontakte.Blocks;
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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Views.VKontakte.Blocks
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AllTracksView : Page
    {

        public AllTracksViewModel ViewModel { get; set; } 
        public AllTracksView()
        {
            this.InitializeComponent();
            ViewModel = new AllTracksViewModel();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.StartLoading((string)e.Parameter, "Список треков");

            //base.OnNavigatedTo(e);
        }
    }
}
