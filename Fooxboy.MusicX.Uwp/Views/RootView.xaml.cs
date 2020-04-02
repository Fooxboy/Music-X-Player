using Fooxboy.MusicX.Uwp.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml.Media.Animation;
using Windows.System;
using ReactiveUI;

namespace Fooxboy.MusicX.Uwp.Views
{
	public class RootViewBase : ReactiveUserControl<RootViewModel>
	{
	}
	
	public partial class RootView
	{
		public RootView()
		{
			//ViewModel.PlayerViewModel.CloseBigPlayer = new Action(CloseBigPlayer);
			InitializeComponent();
        }

		/*protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_container = (IContainer) e.Parameter;

			var navigationService = new NavigationService();
			_container.RegisterInstance<NavigationService>(navigationService);

			PlayerViewModel = new PlayerViewModel(_container);
			NavigationViewModel = new NavigationRootViewModel(_container);
			navigationService.RootFrame = this.Root;
			navigationService.Go(typeof(HomeView), _container);

			UserInfoViewModel = new UserInfoViewModel(_container);
			LoadingViewModel = new LoadingViewModel(_container);
			NotificationViewModel = new NotificationViewModel(_container);

			base.OnNavigatedTo(e);
		}*/

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
            TitleTrack.Text = "Сейчас ничего не воспроизводится";
            //AppWindow appWindow = await AppWindow.TryCreateAsync();
			//Frame appWindowContentFrame = new Frame();
			//appWindowContentFrame.Navigate(typeof(DeveloperView));
			//ElementCompositionPreview.SetAppWindowContent(appWindow, appWindowContentFrame);
			//await appWindow.TryShowAsync();
		}

		private void PersonPicture_Tapped(object sender, TappedRoutedEventArgs e)
		{
			FlyoutProfile.ShowAt((PersonPicture) sender);
		}

		private void TextBlock_PointerEntered(object sender, PointerRoutedEventArgs e)
		{
			if (ArtistText.TextDecorations == Windows.UI.Text.TextDecorations.Underline) return;
			ArtistText.TextDecorations = Windows.UI.Text.TextDecorations.Underline;
		}

		private void ArtistText_PointerExited(object sender, PointerRoutedEventArgs e)
		{
			if (ArtistText.TextDecorations == Windows.UI.Text.TextDecorations.None) return;
			ArtistText.TextDecorations = Windows.UI.Text.TextDecorations.None;
		}

		private void GridButtom_PointerEntered(object sender, PointerRoutedEventArgs e)
		{
			IconBackground.Visibility = Visibility.Visible;
		}

		private void GridButtom_PointerExited(object sender, PointerRoutedEventArgs e)
		{
			IconBackground.Visibility = Visibility.Collapsed;
		}

		private async void Grid_Tapped(object sender, TappedRoutedEventArgs e)
		{
			GridButtom.Height = 660;
			ShadowCover.Visibility = Visibility.Collapsed;
			GridImage.Visibility = Visibility.Collapsed;
			TextGrid.Visibility = Visibility.Collapsed;
			GridButtons.Visibility = Visibility.Collapsed;
			GridTimer.Visibility = Visibility.Collapsed;
			StackButtons.Visibility = Visibility.Collapsed;
			Shadoww.Visibility = Visibility.Collapsed;
			await Animations.BeginAsync();
			//RectangleBackground.Height = +500;
			BigPlayerFrame.Visibility = Visibility.Visible;
			BigPlayerFrame.Navigate(typeof(PlayerView), ViewModel.PlayerViewModel, new DrillInNavigationTransitionInfo());
		}

		private void CloseBigPlayer()
		{
				BigPlayerFrame.Visibility = Visibility.Collapsed;
				GridButtom.Height = 60;
				ShadowCover.Visibility = Visibility.Visible;
				GridImage.Visibility = Visibility.Visible;
				TextGrid.Visibility = Visibility.Visible;
				GridButtons.Visibility = Visibility.Visible;
				GridTimer.Visibility = Visibility.Visible;
				StackButtons.Visibility = Visibility.Visible;
				Shadoww.Visibility = Visibility.Visible;
				AnimationsClose.Begin();
		}

		private void SearchBox_OnKeyUp(object sender, KeyRoutedEventArgs e)
		{
			if (e.OriginalKey == VirtualKey.Enter)
			{
				//Search
			}
		}

		private void SearchBox_OnGotFocus(object sender, RoutedEventArgs e)
		{
			this.SearchBox.PlaceholderText = "Нажмите enter для поиска";
		}

		private void SearchBox_OnLostFocus(object sender, RoutedEventArgs e)
		{
			this.SearchBox.PlaceholderText = "Найдите что нибудь...";
		}
	}
}