using System;
using System.Reactive.Disposables;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Fooxboy.MusicX.Uwp.ViewModels;
using Microsoft.Toolkit.Uwp.Helpers;
using ReactiveUI;

namespace Fooxboy.MusicX.Uwp.Views
{
    public class LoginViewBase : ReactiveUserControl<LoginViewModel>
    {
    }

    public sealed partial class LoginView
    {
        public LoginView()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                ViewModel.TwoFactorInteraction.RegisterHandler(async interaction =>
                {
                    var code = await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                    {
                        var twoFactorAuthContentDialog = new TwoFactorAuthContentDialog();

                        await twoFactorAuthContentDialog.ShowAsync();

                        return twoFactorAuthContentDialog.Result;
                    });

                    interaction.SetOutput(code);
                }).DisposeWith(disposable);
            });
        }
    }
}