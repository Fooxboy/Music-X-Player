using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class PlayerMenuViewModel : BaseViewModel
    {

        private static PlayerMenuViewModel instanse;
        public static PlayerMenuViewModel Instanse
        {
            get
            {
                if (instanse != null) return instanse;
                instanse = new PlayerMenuViewModel();
                return instanse;
            }
           
        }
        private PlayerMenuViewModel()
        {
            MainSelector = Visibility.Visible;
            SettingsSelector = Visibility.Collapsed;
            SearchSelector = Visibility.Collapsed;
            ProSelector = Visibility.Collapsed;
            NavigateToSettings = new RelayCommand(
                () =>
                {
                    SettingsSelector = Visibility.Visible;
                    SearchSelector = Visibility.Collapsed;
                    MainSelector = Visibility.Collapsed;
                    StaticContent.NavigationContentService.Go(typeof(Views.SettingsView));
                    ProSelector = Visibility.Collapsed;
                    Changed("ProSelector");
                    Changed("SettingsSelector");
                    Changed("SearchSelector");
                    Changed("MainSelector");
                }
                );

            NavigateToMain = new RelayCommand(
                () =>
                {
                    SettingsSelector = Visibility.Collapsed;
                    SearchSelector = Visibility.Collapsed;
                    MainSelector = Visibility.Visible;
                    StaticContent.NavigationContentService.Go(typeof(Views.HomeLocalView));
                    ProSelector = Visibility.Collapsed;
                    Changed("ProSelector");
                    Changed("SettingsSelector");
                    Changed("SearchSelector");
                    Changed("MainSelector");
                }
                );

            NavigateToSearch = new RelayCommand(
                () =>
                {
                    //StaticContent.NavigationContentService.Go(typeof(Views.HomeView));
                    SettingsSelector = Visibility.Collapsed;
                    SearchSelector = Visibility.Visible;
                    MainSelector = Visibility.Collapsed;
                    ProSelector = Visibility.Collapsed;
                    Changed("ProSelector");
                    Changed("SettingsSelector");
                    Changed("SearchSelector");
                    Changed("MainSelector");
                }
            );

            NavigateToPro = new RelayCommand(
                () =>
                {
                    //StaticContent.NavigationContentService.Go(typeof(Views.HomeView));
                    SettingsSelector = Visibility.Collapsed;
                    SearchSelector = Visibility.Collapsed;
                    MainSelector = Visibility.Collapsed;
                    ProSelector = Visibility.Visible;
                    StaticContent.NavigationContentService.Go(typeof(Views.ProVersionView));
                    Changed("SettingsSelector");
                    Changed("SearchSelector");
                    Changed("MainSelector");
                    Changed("ProSelector");
                }
            );

            NavigateToPro = new RelayCommand(
                () =>
                {
                    //StaticContent.NavigationContentService.Go(typeof(Views.HomeView));
                    SettingsSelector = Visibility.Collapsed;
                    SearchSelector = Visibility.Collapsed;
                    MainSelector = Visibility.Collapsed;
                    ProSelector = Visibility.Collapsed;
                    StaticContent.NavigationContentService.Go(typeof(Views.VKAuthView));
                    Changed("SettingsSelector");
                    Changed("SearchSelector");
                    Changed("MainSelector");
                    Changed("ProSelector");
                }
            );
        }

        public RelayCommand NavigateToSettings { get; set; }
        public RelayCommand NavigateToMain { get; set; }
        public RelayCommand NavigateToSearch { get; set; }
        public RelayCommand NavigateToPro { get; set; }
        public RelayCommand NavigateToLogin { get; set; }

        public Visibility SettingsSelector { get; set; }
        public Visibility MainSelector { get; set; }
        public Visibility SearchSelector { get; set; }

        public Visibility ProSelector { get; set; }
    }
}
