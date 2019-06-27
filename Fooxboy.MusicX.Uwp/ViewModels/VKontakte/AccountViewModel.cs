using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Services.VKontakte;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
{
    public class AccountViewModel:BaseViewModel
    {

        private static AccountViewModel instanse;

        public static AccountViewModel Instanse
        {
            get
            {
                if (instanse == null) instanse = new AccountViewModel();

                return instanse;
            }
        }


        private AccountViewModel()
        {
            NameUser = "";
            ImageUser = "ms-appx:///Assets/Images/logo.png";

            LogOutCommand = new RelayCommand(async () =>
            {
                await AuthService.LogOut();
            });
        }

        public async Task LoadingInfo()
        {
            try
            {
                var user = await Fooxboy.MusicX.Core.VKontakte.Users.Info.CurrentUser();

                NameUser = $"{user.FirstName} {user.LastName}";
                ImageUser = await ImagesService.AvatarUser(user.PhotoUser);
                Changed("NameUser");
                Changed("ImageUser");
            }catch(Flurl.Http.FlurlHttpException)
            {
                await ContentDialogService.Show(new ErrorConnectContentDialog());
                InternetService.GoToOfflineMode();
            }catch(Exception e)
            {
                await ContentDialogService.Show(new ExceptionDialog("Невозможно получить информацию о вашем аккаунте", "", e));
            }
            
        }


        public string NameUser { get; set; } 

        public string ImageUser { get; set; } 

        public RelayCommand LogOutCommand { get; set; }
    }
}
