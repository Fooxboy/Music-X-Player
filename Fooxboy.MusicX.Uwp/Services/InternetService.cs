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
        public static bool CurrentUIInConnected { get; set; }

        public static DispatcherTimer timer;
        public static bool CheckConnection()
        {
            Connected = NetworkInterface.GetIsNetworkAvailable();
            return Connected;
        }

        public static void Init()
        {
            timer = new DispatcherTimer();
            timer.Tick += CheckConnectionAuto;
            timer.Interval = TimeSpan.FromMilliseconds(500);
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
                }
            }else
            {
                if(CurrentUIInConnected)
                {
                    GoToOfflineMode();
                }
            }
        }

        
        public static void GoToOnlineMode()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                var player = ViewModels.PlayerMenuViewModel.Instanse;
                player.VkontaktePages = Visibility.Visible;
                StaticContent.NavigationContentService.Go(typeof(Views.VKontakte.HomeView));
            });
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
