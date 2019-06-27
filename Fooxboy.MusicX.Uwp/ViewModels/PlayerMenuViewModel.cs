using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
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
            SettingsSelector = Visibility.Collapsed;
            MainSelector = Visibility.Collapsed;
            SearchSelector = Visibility.Collapsed;
            AccountSelector = Visibility.Collapsed;
            ProSelector = Visibility.Collapsed;
            HomeVkontakteSelector = Visibility.Collapsed;
            RecommendationsSelector = Visibility.Collapsed;
            PopularSelector = Visibility.Collapsed;

           

            if (StaticContent.IsAuth)
            {
                HomeVkontakteSelector = Visibility.Visible;
            }
            else MainSelector = Visibility.Visible;

            Changed("SettingsSelector");
            Changed("MainSelector");
            Changed("SearchSelector");
            Changed("AccountSelector");
            Changed("ProSelector");
            Changed("HomeVkontakteSelector");
            Changed("RecommendationsSelector");
            Changed("PopularSelector");

            NavigateToSettings = new RelayCommand(
                () =>
                {

                    SettingsSelector = Visibility.Visible;
                    MainSelector = Visibility.Collapsed;
                    SearchSelector = Visibility.Collapsed;
                    AccountSelector = Visibility.Collapsed;
                    ProSelector = Visibility.Collapsed;
                    HomeVkontakteSelector = Visibility.Collapsed;
                    RecommendationsSelector = Visibility.Collapsed;
                    PopularSelector = Visibility.Collapsed;

                    Changed("SettingsSelector");
                    Changed("MainSelector");
                    Changed("SearchSelector");
                    Changed("AccountSelector");
                    Changed("ProSelector");
                    Changed("HomeVkontakteSelector");
                    Changed("RecommendationsSelector");
                    Changed("PopularSelector");

                    StaticContent.NavigationContentService.Go(typeof(Views.SettingsView));
                }
                );

            NavigateToMain = new RelayCommand(
                () =>
                {

                    SettingsSelector = Visibility.Collapsed;
                    MainSelector = Visibility.Visible;
                    SearchSelector = Visibility.Collapsed;
                    AccountSelector = Visibility.Collapsed;
                    ProSelector = Visibility.Collapsed;
                    HomeVkontakteSelector = Visibility.Collapsed;
                    RecommendationsSelector = Visibility.Collapsed;
                    PopularSelector = Visibility.Collapsed;

                    Changed("SettingsSelector");
                    Changed("MainSelector");
                    Changed("SearchSelector");
                    Changed("AccountSelector");
                    Changed("ProSelector");
                    Changed("HomeVkontakteSelector");
                    Changed("RecommendationsSelector");
                    Changed("PopularSelector");

                    StaticContent.NavigationContentService.Go(typeof(Views.HomeLocalView));
                }
                );

            NavigateToSearch = new RelayCommand(
                () =>
                {
                    SettingsSelector = Visibility.Collapsed;
                    MainSelector = Visibility.Collapsed;
                    SearchSelector = Visibility.Visible;
                    AccountSelector = Visibility.Collapsed;
                    ProSelector = Visibility.Collapsed;
                    HomeVkontakteSelector = Visibility.Collapsed;
                    RecommendationsSelector = Visibility.Collapsed;
                    PopularSelector = Visibility.Collapsed;

                    Changed("SettingsSelector");
                    Changed("MainSelector");
                    Changed("SearchSelector");
                    Changed("AccountSelector");
                    Changed("ProSelector");
                    Changed("HomeVkontakteSelector");
                    Changed("RecommendationsSelector");
                    Changed("PopularSelector");
                }
            );

            NavigateToPro = new RelayCommand(
                () =>
                {

                    SettingsSelector = Visibility.Collapsed;
                    MainSelector = Visibility.Collapsed;
                    SearchSelector = Visibility.Collapsed;
                    AccountSelector = Visibility.Collapsed;
                    ProSelector = Visibility.Visible;
                    HomeVkontakteSelector = Visibility.Collapsed;
                    RecommendationsSelector = Visibility.Collapsed;
                    PopularSelector = Visibility.Collapsed;

                    Changed("SettingsSelector");
                    Changed("MainSelector");
                    Changed("SearchSelector");
                    Changed("AccountSelector");
                    Changed("ProSelector");
                    Changed("HomeVkontakteSelector");
                    Changed("RecommendationsSelector");
                    Changed("PopularSelector");

                    StaticContent.NavigationContentService.Go(typeof(Views.ProVersionView));

                }
            );

            NavigateToLogin = new RelayCommand(
                () =>
                {


                    SettingsSelector = Visibility.Collapsed;
                    MainSelector = Visibility.Collapsed;
                    SearchSelector = Visibility.Collapsed;
                    AccountSelector = Visibility.Visible;
                    ProSelector = Visibility.Collapsed;
                    HomeVkontakteSelector = Visibility.Collapsed;
                    RecommendationsSelector = Visibility.Collapsed;
                    PopularSelector = Visibility.Collapsed;

                    Changed("SettingsSelector");
                    Changed("MainSelector");
                    Changed("SearchSelector");
                    Changed("AccountSelector");
                    Changed("ProSelector");
                    Changed("HomeVkontakteSelector");
                    Changed("RecommendationsSelector");
                    Changed("PopularSelector");


                    if (StaticContent.IsAuth) StaticContent.NavigationContentService.Go(typeof(Views.VKontakte.AccountView));
                    else StaticContent.NavigationContentService.Go(typeof(Views.VKontakte.AuthView));

                }
            );

            NavigateToRecommendations = new RelayCommand(() =>
            {
                SettingsSelector = Visibility.Collapsed;
                MainSelector = Visibility.Collapsed;
                SearchSelector = Visibility.Collapsed;
                AccountSelector = Visibility.Collapsed;
                ProSelector = Visibility.Collapsed;
                HomeVkontakteSelector = Visibility.Collapsed;
                RecommendationsSelector = Visibility.Visible;
                PopularSelector = Visibility.Collapsed;

                Changed("SettingsSelector");
                Changed("MainSelector");
                Changed("SearchSelector");
                Changed("AccountSelector");
                Changed("ProSelector");
                Changed("HomeVkontakteSelector");
                Changed("RecommendationsSelector");
                Changed("PopularSelector");

                StaticContent.NavigationContentService.Go(typeof(Views.VKontakte.RecommendationsView));

                
            });

            NavigateToPopular = new RelayCommand(() =>
            {
                SettingsSelector = Visibility.Collapsed;
                MainSelector = Visibility.Collapsed;
                SearchSelector = Visibility.Collapsed;
                AccountSelector = Visibility.Collapsed;
                ProSelector = Visibility.Collapsed;
                HomeVkontakteSelector = Visibility.Collapsed;
                RecommendationsSelector = Visibility.Collapsed;
                PopularSelector = Visibility.Visible;

                Changed("SettingsSelector");
                Changed("MainSelector");
                Changed("SearchSelector");
                Changed("AccountSelector");
                Changed("ProSelector");
                Changed("HomeVkontakteSelector");
                Changed("RecommendationsSelector");
                Changed("PopularSelector");

                StaticContent.NavigationContentService.Go(typeof(Views.VKontakte.PopularView));
            });

            NavigateToHomeVkontakte = new RelayCommand(() =>
            {
                SettingsSelector = Visibility.Collapsed;
                MainSelector = Visibility.Collapsed;
                SearchSelector = Visibility.Collapsed;
                AccountSelector = Visibility.Collapsed;
                ProSelector = Visibility.Collapsed;
                HomeVkontakteSelector = Visibility.Visible;
                RecommendationsSelector = Visibility.Collapsed;
                PopularSelector = Visibility.Collapsed;

                Changed("SettingsSelector");
                Changed("MainSelector");
                Changed("SearchSelector");
                Changed("AccountSelector");
                Changed("ProSelector");
                Changed("HomeVkontakteSelector");
                Changed("RecommendationsSelector");
                Changed("PopularSelector");

                StaticContent.NavigationContentService.Go(typeof(Views.VKontakte.HomeView));
            });

            if (StaticContent.IsAuth)
            {
                VkontaktePages = Visibility.Visible;
            }
            else VkontaktePages = Visibility.Collapsed;
        }

        private string nameAccount = "Войти в аккаунт";
        public string NameAccount
        {
            get => nameAccount;
            set
            {
                if (nameAccount == value) return;

                nameAccount = value;
                Changed("NameAccount");
            }
        }

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
        private Visibility vkontaktePages;
        public Visibility VkontaktePages
        {
            get => vkontaktePages;
            set
            {
                if (vkontaktePages == value) return;
                vkontaktePages = value;
                Changed("VkontaktePages");
            }
        }
    }
}
