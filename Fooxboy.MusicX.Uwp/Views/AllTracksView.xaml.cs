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
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.ViewModels;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Views
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
            
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var array = (object[]) e.Parameter;
            var player = (PlayerService) array[0];
            var api = (Api) array[1];
            var notification = (NotificationService) array[4];
            var logger = (LoggerService) array[5];
            ViewModel = new AllTracksViewModel(player, api, notification, logger);


            var type = (string) array[2];
            if (type == "block")
            {
                var blockId = (string) array[3];
                await ViewModel.StartLoading(new object[] { type, blockId });
            }



            base.OnNavigatedTo(e);
        }

        private async void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            await ViewModel.PlayTrack((Track)e.ClickedItem);
        }
    }
}
