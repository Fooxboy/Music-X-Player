using DryIoc;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class NavigationRootViewModel:BaseViewModel
    {
        public bool VisibilitySelectorHome { get; set; }
        public bool VisibilitySelectorRecommendations { get; set; }
        public bool VisibilitySelectorFavoriteArtists { get; set; }
        public bool VisibilitySelectorDownloads { get; set; }
        public RelayCommand GoToHome { get; set; }
        public RelayCommand GoToRecommendations { get; set; }
        public RelayCommand GoToFavoriteArtists { get; set; }
        public RelayCommand GoToDownloads { get; set; }
        private NavigationService _navigationService;

        public NavigationRootViewModel()
        {
            GoToHome = new RelayCommand(ToHome);
            GoToRecommendations = new RelayCommand(ToRecommendations);
            GoToFavoriteArtists = new RelayCommand(ToFavoriteArtists);
            GoToDownloads = new RelayCommand(ToDownloads);
            _navigationService = Container.Get.Resolve<NavigationService>();
        }

        public void Changed()
        {
            if (VisibilitySelectorHome == true)
            {
                VisibilitySelectorRecommendations = false;
                VisibilitySelectorFavoriteArtists = false;
                VisibilitySelectorDownloads = false;
            } else if (VisibilitySelectorRecommendations == true)
            {
                VisibilitySelectorHome = false;
                VisibilitySelectorFavoriteArtists = false;
                VisibilitySelectorDownloads = false;
            } else if (VisibilitySelectorFavoriteArtists == true)
            {
                VisibilitySelectorRecommendations = false;
                VisibilitySelectorDownloads = false;
                VisibilitySelectorHome = false;
            } else if (VisibilitySelectorDownloads == true)
            {
                VisibilitySelectorRecommendations = false;
                VisibilitySelectorHome = false;
                VisibilitySelectorFavoriteArtists = false;
            }

            Changed("VisibilitySelectorHome");
            Changed("VisibilitySelectorRecommendations");
            Changed("VisibilitySelectorFavoriteArtists");
            Changed("VisibilitySelectorDownloads");
        }

        public void ToHome()
        {
            //if (VisibilitySelectorHome == true) return;
            VisibilitySelectorHome = true;
            Changed();
            _navigationService.Go(typeof(HomeView), null, 1);
        }

        public void ToRecommendations()
        {
            if (VisibilitySelectorRecommendations == true) return;
            VisibilitySelectorRecommendations = true;
            Changed();
        }

        public void ToFavoriteArtists()
        {
            if (VisibilitySelectorFavoriteArtists == true) return;
            VisibilitySelectorFavoriteArtists = true;
            Changed();
        }

        public void ToDownloads()
        {
            if (VisibilitySelectorDownloads == true) return;
            VisibilitySelectorDownloads = true;
            Changed();
        }
    }
}
