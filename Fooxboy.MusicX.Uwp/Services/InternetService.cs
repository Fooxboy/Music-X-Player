using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;
using Windows.UI.Xaml;

namespace Fooxboy.MusicX.Uwp.Services
{
    public static class InternetService
    {
        public static bool Connected { get; set; }

        public static bool CheckConnection()
        {
            Connected = NetworkInterface.GetIsNetworkAvailable();
            return Connected;
        }


        public static void CheckConnectionAuto()
        {

        }


        public static void GoToOfflineMode()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                var player = ViewModels.PlayerMenuViewModel.Instanse;
                player.VkontaktePages = Visibility.Collapsed;
                StaticContent.NavigationContentService.Go(typeof(Views.OfflineModeView));
            });
           
        }
    }
}
