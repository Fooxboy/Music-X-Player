using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.Core;
using Java.Lang;

namespace Fooxboy.MusicX.Reimagined.Services
{
    public static class AuthService
    {

        private static Context _context;
        private static bool _isds = false;


        public static bool IsLoggedIn()
        {
            var prefs = Application.Context.GetSharedPreferences("MusicXReimagined", FileCreationMode.Private);
            return System.String.IsNullOrEmpty(prefs.GetString("VKToken", null)) ? false : true;
        }

        public static void ShowLoginDialog(Context c)
        {
            _context = c;
            Dialog loginDialog = new Dialog(c);
            loginDialog.Window.SetBackgroundDrawable(c.GetDrawable(Resource.Drawable.dialog));
            loginDialog.SetCanceledOnTouchOutside(false);
            loginDialog.SetCancelable(false);
            loginDialog.SetContentView(Resource.Layout.dialog_auth);
            var loginBtn = loginDialog.FindViewById<Button>(Resource.Id.loginButton);
            var emailField = loginDialog.FindViewById<EditText>(Resource.Id.loginEmailField);
            var passwordField = loginDialog.FindViewById<EditText>(Resource.Id.loginPasswordField);
            loginBtn.Click += (sender, e) =>
            {
                if (emailField.Text == "" || passwordField.Text == "")
                {
                    Toast.MakeText(Application.Context, "Введите email/телефон и(или) пароль", ToastLength.Long).Show();
                }
                else
                {
                    loginDialog.Dismiss();
                    string token = null;
                    Dialog progressDialog = new Dialog(c);
                    progressDialog.SetCanceledOnTouchOutside(false);
                    progressDialog.SetCancelable(false);
                    progressDialog.SetContentView(Resource.Layout.dialog_progress);
                    progressDialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
                    progressDialog.Show();
                    try
                    {
                        Task.Factory.StartNew(async () =>
                        {
                            StaticContentService.Token = await Api.GetApi().VKontakte.Auth.UserAsync(emailField.Text, passwordField.Text, OnTwoFactor, null);
                        });
                    }
                    catch (VkNet.Exception.UserAuthorizationFailException)
                    {
                        Toast.MakeText(Application.Context, "Ошибка: не удалось авторизоваться", ToastLength.Long).Show();
                    }
                    catch (VkNet.Exception.VkAuthorizationException)
                    {
                        Toast.MakeText(Application.Context, "Ошибка: исключение авторизации ВКонтакте", ToastLength.Long).Show();
                    }
                    catch (VkNet.Exception.VkApiAuthorizationException)
                    {
                        Toast.MakeText(Application.Context, "Ошибка: исключение авторизации VkApi", ToastLength.Long).Show();
                    }
                    catch (VkNet.Exception.UserDeletedOrBannedException)
                    {
                        Toast.MakeText(Application.Context, "Ошибка: пользователь забанен или удален", ToastLength.Long).Show();
                    }
                    catch (Flurl.Http.FlurlHttpException)
                    {
                        Toast.MakeText(Application.Context, "Ошибка: нет доступа к сети", ToastLength.Long).Show();
                    }
                    catch (System.Exception ex)
                    {
                        Toast.MakeText(Application.Context, "Ошибка: неизвестная ошибка авторизации: " + ex, ToastLength.Long).Show();
                    }
                    while(StaticContentService.Token == null)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                    progressDialog.Dismiss();
                    loginDialog.Dismiss();
                    var prefs = Application.Context.GetSharedPreferences("MusicXReimagined", FileCreationMode.Private);
                    var editor = prefs.Edit();
                    editor.PutString("VKToken", token);
                    editor.Commit();

                }
            };
            loginDialog.Show();
        }

        public static string OnTwoFactor()
        {
            var t = Task.Run(ShowTwoFactorDialog);
            while (!_isds)
            {

            }
            while (StaticContentService.TwoFactorAuthCode == null)
            {
                System.Threading.Thread.Sleep(300);
            }
            string code = StaticContentService.TwoFactorAuthCode;
            StaticContentService.TwoFactorAuthCode = null;
            return code;
        }

        public async static void ShowTwoFactorDialog()
        {
            Dialog twoFactorDialog = new Dialog(Application.Context);
            twoFactorDialog.Window.SetBackgroundDrawable(_context.GetDrawable(Resource.Drawable.dialog));
            twoFactorDialog.SetCanceledOnTouchOutside(false);
            twoFactorDialog.SetCancelable(false);
            twoFactorDialog.SetContentView(Resource.Layout.dialog_twofactor);
            var tfCodeField = twoFactorDialog.FindViewById<EditText>(Resource.Id.tfCodeField);
            var tfButton = twoFactorDialog.FindViewById<Button>(Resource.Id.tfButton);
            tfButton.Click += (sender, e) =>
            {
                if (tfCodeField.Text == "")
                {
                    Toast.MakeText(Application.Context, "Слыш ты че код введи", ToastLength.Long).Show();
                }
                else
                {
                    StaticContentService.TwoFactorAuthCode = tfCodeField.Text;
                }
            };
            twoFactorDialog.Show();
            _isds = true;
        }

        public static string GetToken()
        {
            return Application.Context.GetSharedPreferences("MusicX", FileCreationMode.Private).GetString("VKToken", null);
        }
    }
}