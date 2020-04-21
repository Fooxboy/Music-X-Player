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
using Fooxboy.MusicX.Reimagined.Services;

namespace Fooxboy.MusicX.Reimagined.Activities
{
    [Activity(Label = "AuthActivity")]
    public class AuthActivity : Activity
    {

        public override void OnBackPressed()
        {
            return;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_auth);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            var loginBtn = FindViewById<Button>(Resource.Id.loginButton);
            var emailField = FindViewById<EditText>(Resource.Id.loginEmailField);
            var passwordField = FindViewById<EditText>(Resource.Id.loginPasswordField);
            loginBtn.Click += async (sender, e) =>
            {
                if (emailField.Text == "" || passwordField.Text == "")
                {
                    Toast.MakeText(Application.Context, "Введите email/телефон и(или) пароль", ToastLength.Long).Show();
                }
                else
                {
                    string token = null;
                    try
                    {
                            token = await Api.GetApi().VKontakte.Auth.UserAsync(emailField.Text, passwordField.Text, OnTwoFactor, null);
                    }
                    catch (VkNet.Exception.UserAuthorizationFailException)
                    {
                        StaticContentService.ErrorCaption = "Ошибка";
                        StaticContentService.ErrorDescription = "Не удалось авторизоваться";
                        Intent intent = new Intent(Application.Context, typeof(Activities.ErrorActivity));
                        intent.SetFlags(ActivityFlags.NewTask);
                        Application.Context.StartActivity(intent);
                    }
                    catch (VkNet.Exception.VkAuthorizationException)
                    {
                        StaticContentService.ErrorCaption = "Ошибка";
                        StaticContentService.ErrorDescription = "Исключение авторизации ВКонтакте";
                        Intent intent = new Intent(Application.Context, typeof(Activities.ErrorActivity));
                        intent.SetFlags(ActivityFlags.NewTask);
                        Application.Context.StartActivity(intent);
                    }
                    catch (VkNet.Exception.VkApiAuthorizationException)
                    {
                        StaticContentService.ErrorCaption = "Ошибка";
                        StaticContentService.ErrorDescription = "Исключение авторизации VkApi";
                        Intent intent = new Intent(Application.Context, typeof(Activities.ErrorActivity));
                        intent.SetFlags(ActivityFlags.NewTask);
                        Application.Context.StartActivity(intent);
                    }
                    catch (VkNet.Exception.UserDeletedOrBannedException)
                    {
                        StaticContentService.ErrorCaption = "Ошибка";
                        StaticContentService.ErrorDescription = "Пользователь забанен или удален";
                        Intent intent = new Intent(Application.Context, typeof(Activities.ErrorActivity));
                        intent.SetFlags(ActivityFlags.NewTask);
                        Application.Context.StartActivity(intent);
                    }
                    catch (Flurl.Http.FlurlHttpException)
                    {
                        StaticContentService.ErrorCaption = "Ошибка";
                        StaticContentService.ErrorDescription = "Нет доступа к сети";
                        Intent intent = new Intent(Application.Context, typeof(Activities.ErrorActivity));
                        intent.SetFlags(ActivityFlags.NewTask);
                        Application.Context.StartActivity(intent);
                    }
                    catch (System.Exception ex)
                    {
                        StaticContentService.ErrorCaption = "Неизвестная ошибка";
                        StaticContentService.ErrorDescription = ex.Message;
                        Intent intent = new Intent(Application.Context, typeof(Activities.ErrorActivity));
                        intent.SetFlags(ActivityFlags.NewTask);
                        Application.Context.StartActivity(intent);
                    }


                    if(token != null)
                    {
                        var prefs = Application.Context.GetSharedPreferences("MusicXReimagined", FileCreationMode.Private);
                        var editor = prefs.Edit();
                        editor.PutString("VKToken", token);
                        editor.Commit();
                        Intent intent = new Intent(Application.Context, typeof(MainActivity));
                        intent.SetFlags(ActivityFlags.NewTask);
                        Application.Context.StartActivity(intent);
                        this.Finish();
                    }
                }
            };

        }


        public static string OnTwoFactor()
        {
            Intent intent = new Intent(Application.Context, typeof(Activities.TFActivity));
            intent.SetFlags(ActivityFlags.NewTask);
            Application.Context.StartActivity(intent);

            while (StaticContentService.TwoFactorAuthCode == null)
            {
                System.Threading.Thread.Sleep(300);
            }
            string code = StaticContentService.TwoFactorAuthCode;
            StaticContentService.TwoFactorAuthCode = null;
            return code;
        }

    }
}