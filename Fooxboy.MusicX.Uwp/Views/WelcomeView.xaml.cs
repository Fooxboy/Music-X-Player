using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Fooxboy.MusicX.Uwp.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DryIoc;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Core.Interfaces;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomeView : Page
    {
        public WelcomeView()
        {
            this.InitializeComponent();
        }

        private IContainer _container;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _container = (IContainer) e.Parameter;
            base.OnNavigatedTo(e);
        }

        private async void StartButtonClick(object sender, RoutedEventArgs e)
        {
            ButtonStart.Visibility = Visibility.Collapsed;
            ProgressRing.IsActive = true;

            Start.Text = "Music X готовится к первому запуску. Это не займет много времени.";
            /*var localpath = ApplicationData.Current.LocalFolder;
            var file = await localpath.CreateFileAsync("config.app");
            var config = new ConfigApp();
            config.IsRateMe = false;
            config.SaveImageToCache = true;
            config.SaveTracksToCache = false;
            config.StreamMusic = false;
            config.ThemeApp = 0;
            var configString = JsonConvert.SerializeObject(config);
            await FileIO.WriteTextAsync(file, configString);*/
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values["themeApp"] = 0;


             var currentFrame = Window.Current.Content as Frame;
            currentFrame?.Navigate(typeof(LoginView), _container);
        }
    }
}
