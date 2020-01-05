using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Fooxboy.MusicX.Core;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Resources.fragments;
using Fooxboy.MusicX.AndroidApp.Services;
using Fooxboy.MusicX.Core.VKontakte;
using Java.Lang;

namespace Fooxboy.MusicX.AndroidApp
{
    [Activity(Label = "@string/app_name")]
    public class AuthActivity : Activity
    {
        private EditText loginText;
        private EditText passwordText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_auth);
            loginText = FindViewById<EditText>(Resource.Id.vklogin);
            passwordText = FindViewById<EditText>(Resource.Id.vkpassword);

            Button btn = FindViewById<Button>(Resource.Id.log_in_btn);
            btn.Click += async (sender, e) => 
            {
                if(loginText.Text == "" || passwordText.Text == "")
                {
                    Toast.MakeText(this, "Вы не указали логин и пароль", ToastLength.Long).Show();
                    return;
                }

                var progressBarLoading = FindViewById<ProgressBar>(Resource.Id.progressBarLogin);
                progressBarLoading.Visibility = ViewStates.Visible;
                btn.Visibility = ViewStates.Invisible;
                Toast.MakeText(this, "Пытаюсь связаться с ВКонтакте...", ToastLength.Long).Show();
                string token = null;
                try
                {
                    token = await Api.GetApi().VKontakte.Auth.UserAsync(loginText.Text, passwordText.Text, TwoFactorAuth, null);
                    
                }catch(VkNet.Exception.UserAuthorizationFailException)
                {
                    AuthService.ShowIncorrectLoginDialog(this.FragmentManager);
                }catch(VkNet.Exception.VkAuthorizationException)
                {
                    AuthService.ShowIncorrectLoginDialog(this.FragmentManager);
                }
                catch (VkNet.Exception.VkApiAuthorizationException)
                {
                    AuthService.ShowIncorrectLoginDialog(this.FragmentManager);
                }
                catch (VkNet.Exception.UserDeletedOrBannedException)
                {
                    AuthService.ShowIncorrectLoginDialog(this.FragmentManager);
                }
                catch (Flurl.Http.FlurlHttpException)
                {
                    Toast.MakeText(this, "Нет доступа к интернету", ToastLength.Long).Show();
                }
                catch (System.Exception ex)
                {
                    Toast.MakeText(this, "Неизвестная ошибка авторизации: " + ex, ToastLength.Long).Show();
                }

                if(token != null)
                {
                    //Toast.MakeText(this, "Ваш токен " + token, ToastLength.Long).Show();
                    var prefs = Application.Context.GetSharedPreferences("MusicX", FileCreationMode.Private);
                    var editor = prefs.Edit();
                    editor.PutString("VKToken", token);
                    editor.Commit();

                    Intent intent = new Intent(this.ApplicationContext, typeof(MainActivity));
                    intent.SetFlags(ActivityFlags.NewTask);
                    StartActivity(intent);
                    Finish();
                }else
                {
                    progressBarLoading.Visibility = ViewStates.Invisible;
                    btn.Visibility = ViewStates.Visible;

                }

            };

        }

        private string TwoFactorAuth()
        {
            Intent intent = new Intent(this.ApplicationContext, typeof(TwoFactorDialogActivity));
            intent.SetFlags(ActivityFlags.NewTask);
            StartActivity(intent);

            while (Services.StaticContentService.CodeTwoFactorAuth == null)
            {
                Thread.Sleep(1000);
            }

            return Services.StaticContentService.CodeTwoFactorAuth;
        }
        
        //Prevent Activity from closing by back button/gesture
        public override void OnBackPressed()
        {
            return;
        }
    }
}