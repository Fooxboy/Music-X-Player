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
            AccountSelector = Visibility.Collapsed;
            NavigateToSettings = new RelayCommand(
                () =>
                {
                    SettingsSelector = Visibility.Visible;
                    SearchSelector = Visibility.Collapsed;
                    MainSelector = Visibility.Collapsed;
                    StaticContent.NavigationContentService.Go(typeof(Views.SettingsView));
                    ProSelector = Visibility.Collapsed;
                    AccountSelector = Visibility.Collapsed;
                    Changed("ProSelector");
                    Changed("SettingsSelector");
                    Changed("SearchSelector");
                    Changed("MainSelector");
                    Changed("AccountSelector");
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
                    AccountSelector = Visibility.Collapsed;
                    Changed("ProSelector");
                    Changed("SettingsSelector");
                    Changed("SearchSelector");
                    Changed("MainSelector");
                    Changed("AccountSelector");
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
                    AccountSelector = Visibility.Collapsed;
                    Changed("ProSelector");
                    Changed("SettingsSelector");
                    Changed("SearchSelector");
                    Changed("MainSelector");
                    Changed("AccountSelector");
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
                    AccountSelector = Visibility.Collapsed;
                    StaticContent.NavigationContentService.Go(typeof(Views.ProVersionView));
                    Changed("SettingsSelector");
                    Changed("SearchSelector");
                    Changed("MainSelector");
                    Changed("ProSelector");
                    Changed("AccountSelector");
                }
            );

            NavigateToLogin = new RelayCommand(
                () =>
                {
                    //StaticContent.NavigationContentService.Go(typeof(Views.HomeView));
                    SettingsSelector = Visibility.Collapsed;
                    SearchSelector = Visibility.Collapsed;
                    MainSelector = Visibility.Collapsed;
                    ProSelector = Visibility.Collapsed;
                    AccountSelector = Visibility.Visible;
                    StaticContent.NavigationContentService.Go(typeof(Views.VKontakte.AuthView));
                    Changed("SettingsSelector");
                    Changed("SearchSelector");
                    Changed("MainSelector");
                    Changed("ProSelector");
                    Changed("AccountSelector");
                }
            );

            if (StaticContent.IsAuth)
            {
                VkontaktePages = Visibility.Visible;
            }
            else VkontaktePages = Visibility.Collapsed;
        }

        public string NameAccount { get; set; } = "Войти в аккаунт";

        public RelayCommand NavigateToSettings { get; set; }
        public RelayCommand NavigateToMain { get; set; }
        public RelayCommand NavigateToSearch { get; set; }
        public RelayCommand NavigateToPro { get; set; }
        public RelayCommand NavigateToLogin { get; set; }
        public RelayCommand NavigateToHomeVkontakte { get; set; }
        public RelayCommand NavigateToRecommendations { get; set; }
        public RelayCommand NavigateToPopular { get; set; }

        public Visibility SettingsSelector { get; set; }
        public Visibility MainSelector { get; set; }
        public Visibility SearchSelector { get; set; }
        public Visibility AccountSelector { get; set; }
        public Visibility ProSelector { get; set; }
        public Visibility HomeVkontakteSelector { get; set; }
        public Visibility RecommendationsSelector { get; set; }
        public Visibility PopularSelector { get; set; }

        public Visibility VkontaktePages { get; set; }
    }
}
