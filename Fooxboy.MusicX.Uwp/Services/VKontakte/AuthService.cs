using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Fooxboy.MusicX.Uwp.ViewModels;
using Fooxboy.MusicX.Uwp.ViewModels.VKontakte;
using Fooxboy.MusicX.Uwp.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Fooxboy.MusicX.Uwp.Services.VKontakte
{
    public static class AuthService
    {

        public static bool TwoFactorAuthIsOpen = false;
        public static TwoFactorAuthContentDialog dialog = null;
        

        public async static Task SetNameAccount()
        {
            var user = await Fooxboy.MusicX.Core.VKontakte.Users.Info.CurrentUser();
            var name = $"{user.FirstName} {user.LastName}";
            var viewmodel = PlayerMenuViewModel.Instanse;
            viewmodel.NameAccount = name;
        }

        public async static Task SetPhotoAccount()
        {
            var user = await Fooxboy.MusicX.Core.VKontakte.Users.Info.CurrentUser();
            var photo = await ImagesService.AvatarUser(user.PhotoUser);
            var viewmodel = PlayerMenuViewModel.Instanse;
            viewmodel.PhotoAccount = photo;
        }

        public async static Task<string> FirstAuth(string login, string password)
        {
            var token = await Fooxboy.MusicX.Core.VKontakte.Auth.User(login, password, AuthService.TwoFactorAuth, new CaptchaSolver());
            await Fooxboy.MusicX.Core.VKontakte.Auth.Auto(token, new CaptchaSolver());
            await SetNameAccount();
            await SetPhotoAccount();
            return token;
        }

        public async static Task AutoAuth()
        {
            try
            {
                var tokenObject = await TokenService.Load();
                await Fooxboy.MusicX.Core.VKontakte.Auth.Auto(tokenObject.Token, new CaptchaSolver());
                await SetNameAccount();
                await SetPhotoAccount();
            }catch (VkNet.Exception.UserAuthorizationFailException)
            {
                await TokenService.Delete();
                //await ContentDialogService.Show(new IncorrectLoginOrPasswordContentDialog());
            }
            catch (VkNet.Exception.VkAuthorizationException)
            {
                await TokenService.Delete();

                //await ContentDialogService.Show(new IncorrectLoginOrPasswordContentDialog());
            }
            catch (VkNet.Exception.VkApiAuthorizationException)
            {
                await TokenService.Delete();

                //await ContentDialogService.Show(new IncorrectLoginOrPasswordContentDialog());
            }
            catch (VkNet.Exception.UserDeletedOrBannedException)
            {
                await TokenService.Delete();

                //await ContentDialogService.Show(new IncorrectLoginOrPasswordContentDialog());
            }
        }

        public async static Task LogOut()
        {
            StaticContent.IsAuth = false;
            PlayerMenuViewModel.Instanse.VkontaktePages = Visibility.Collapsed;
            PlayerMenuViewModel.Instanse.NameAccount = "Войти в аккаунт";
            PlayerMenuViewModel.Instanse.PhotoAccount = "ms-appx:///Assets/Images/logo.png";
            StaticContent.MusicVKontakte.Clear();
            StaticContent.PlaylistsVKontakte.Clear();
            HomeViewModel.Instanse.ClearReady();
            StaticContent.NavigationContentService.Go(typeof(HomeLocalView));
            await TokenService.Delete();
        }

        public async static Task<bool> IsAuth()
        {
            try
            {
                var file = await StaticContent.LocalFolder.GetFileAsync("token.json");
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}
