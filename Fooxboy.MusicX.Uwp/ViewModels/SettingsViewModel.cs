using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class SettingsViewModel: BaseViewModel
    {
        private static SettingsViewModel instanse;

        public static SettingsViewModel Instanse
        {
            get
            {
                if (instanse == null) instanse = new SettingsViewModel();
                return instanse;
            }
            set
            {
                instanse = value;
            }
        }

        private SettingsViewModel()
        {
            //var musicPath = KnownFolders.MusicLibrary.Path;
            //var documentsPath = KnownFolders.DocumentsLibrary.Path;

            musicLib = Config.FindInMusicLibrary;
            StreamMusic = Config.StreamMusic;
            SaveImage = Config.SaveImage;
            Changed("StreamMusic");
            Changed("MusicLibraryIsOn");

            if(StaticContent.IsPro)
            {
                EnableDarkTheme = true;
                ContentDarkTheme = "Темная";
                Changed("EnableDarkTheme");
                Changed("ContentDarkTheme");
            }
            else
            {
                EnableDarkTheme = false;
                ContentDarkTheme = "Темная (Темная тема доступна только в Music X Pro)";
                Changed("EnableDarkTheme");
                Changed("ContentDarkTheme");
            }

            if(Config.ThemeApp == 0)
            {
                SelectDarkTheme = false;
                SelectLightTheme = true;
                Changed("SelectDarkTheme");
                Changed("SelectLightTheme");
            }else
            {
                SelectDarkTheme = true;
                SelectLightTheme = false;
                Changed("SelectDarkTheme");
                Changed("SelectLightTheme");
            }
        }

        private bool musicLib;
        public bool MusicLibraryIsOn
        {
            get => musicLib;
            set 
            {
                if(value != musicLib)
                {
                    StaticContent.Config.FindInMusicLibrary = value;

                    ConfigService.SaveConfig(StaticContent.Config);
                    musicLib = value;
                    Changed("MusicLibraryIsOn");
                }
            }
        }

        public string VersionApp
        {
            get => $"Версия: {SystemInformation.ApplicationVersion.Major}.{SystemInformation.ApplicationVersion.Minor}.{SystemInformation.ApplicationVersion.Build}";
            set
            {

            }
        }


        public ConfigApp Config
        {
            get => StaticContent.Config;
            set
            {
                StaticContent.Config = value;
            }
        }

        public string ContentDarkTheme { get; set; }
        public bool EnableDarkTheme { get; set; }

        public bool SaveImage { get; set; }
        public bool SelectDarkTheme { get; set; }
        public bool SelectLightTheme { get; set; }
        public  bool StreamMusic { get; set; }

        public async Task RadioButton_ClickLight(object sender, RoutedEventArgs e)
        {
            if(Config.ThemeApp != 0)
            {
                SelectDarkTheme = false;
                SelectLightTheme = true;
                Config.ThemeApp = 0;
                await ConfigService.SaveConfig(Config);
                var settings = ApplicationData.Current.LocalSettings;
                settings.Values["themeApp"] = 0;
                await new MessageDialog("Тема будет изменена при следующем запуске приложения").ShowAsync();
                Changed("SelectDarkTheme");
                Changed("SelectLightTheme");
            }
            
        }

        public Visibility VisibilityAds
        {
            get
            {
                return StaticContent.IsPro ? Visibility.Collapsed : Visibility.Visible;
            }
            set
            {
                //support x:bind
            }
        }

        public Visibility VisibilityPro
        {
            get
            {
                return StaticContent.IsPro ? Visibility.Visible : Visibility.Collapsed;
            }
            set
            {
                //support x:bind
            }
        }


        public async void SaveImage_OnToggled(object sender, RoutedEventArgs e)
        {
            Config.SaveImage = !SaveImage;
            await ConfigService.SaveConfig(Config);
        }

        public async void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            Config.StreamMusic = !StreamMusic;
            await ConfigService.SaveConfig(Config);
        }

        public async void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            
            //hrow new NotImplementedException();
        }

        public async Task RadioButton_ClickDark(object sender, RoutedEventArgs e)
        {
            if (Config.ThemeApp != 1)
            {
                SelectDarkTheme = true;
                SelectLightTheme = false;
                Config.ThemeApp = 1;
                await ConfigService.SaveConfig(Config);
                var settings = ApplicationData.Current.LocalSettings;
                settings.Values["themeApp"] = 1;
                await new MessageDialog("Тема будет изменена при следующем запуске приложения").ShowAsync();
                Changed("SelectDarkTheme");
                Changed("SelectLightTheme");
            }
        }
    }
}
