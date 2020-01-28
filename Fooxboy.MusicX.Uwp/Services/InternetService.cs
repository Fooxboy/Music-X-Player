using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Networking.Connectivity;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Microsoft.Toolkit.Uwp.Helpers;

namespace Fooxboy.MusicX.Uwp.Services
{
    public static class InternetService
    {
        public static bool Connected { get; set; } = true;
        public static bool CurrentUIInConnected { get; set; } = true;

        public static DispatcherTimer timer;
        public static bool CheckConnection()
        {
            var profile = NetworkInformation.GetInternetConnectionProfile();

            if (profile == null) return false;
            return profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
        }

    

        public static void Init()
        {
            timer = new DispatcherTimer();
            timer.Tick += CheckConnectionAuto;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();

            App.Current.Resuming += Current_Resuming;
            App.Current.Suspending += Current_Suspending;
        }

        private static void Current_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            timer.Stop();
        }

        private static void Current_Resuming(object sender, object e)
        {
            timer.Start();
        }

        private static void CheckConnectionAuto(object sender, object e)
        {
            Connected = CheckConnection();

            if (Connected)
            {
                if(!CurrentUIInConnected)
                {
                    GoToOnlineMode();
                    CurrentUIInConnected = true;
                }
            }else
            {
                if(CurrentUIInConnected)
                {
                    GoToOfflineMode();
                    CurrentUIInConnected = false;
                }
            }
        }

        
        public static void GoToOnlineMode()
        {
            //DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            //{
            //    var player = ViewModels.PlayerMenuViewModel.Instanse;
            //    player.VkontaktePages = Visibility.Visible;
            //    StaticContent.NavigationContentService.Go(typeof(Views.VKontakte.HomeView));
            //});
        }

        public static void GoToOfflineMode()
        {
            //DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            //{
            //    var player = ViewModels.PlayerMenuViewModel.Instanse;
            //    player.VkontaktePages = Visibility.Collapsed;
            //    if (StaticContent.NavigationContentService == null) return;
            //    StaticContent.NavigationContentService.Go(typeof(Views.OfflineModeView));
            //});
           
        }
    }
}
