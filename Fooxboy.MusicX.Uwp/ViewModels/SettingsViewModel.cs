using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.Storage;
using Windows.UI.Xaml;

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
            Changed("MusicLibraryIsOn");
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
    }
}
