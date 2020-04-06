using Microsoft.Toolkit.Uwp.Helpers;
using System;
using Windows.Media.Core;
using Windows.Media.Playback;
using DynamicData.Binding;
using VkNet.Model.Attachments;

namespace Fooxboy.MusicX.Uwp.Services
{
	public class PlayerService
	{
		private readonly NotificationService _notificationService;
		private readonly MediaPlayer _mediaPlayer;

		public event EventHandler PlayStateChangedEvent;
		public event EventHandler<TimeSpan> PositionTrackChangedEvent;
		public event EventHandler TrackChangedEvent;

		public PlayerService(NotificationService notificationService)
		{
			_notificationService = notificationService;
			_mediaPlayer = new MediaPlayer();
			Tracks = new ObservableCollectionExtended<Audio>();

			_mediaPlayer.AudioCategory = MediaPlayerAudioCategory.Media;
			_mediaPlayer.PlaybackSession.PlaybackStateChanged += MediaPlayerOnCurrentStateChanged;
			_mediaPlayer.MediaEnded += MediaPlayerOnMediaEnded;
			_mediaPlayer.MediaFailed += MediaPlayerOnMediaFailed;
			_mediaPlayer.PlaybackSession.PositionChanged += PlaybackSessionOnPositionChanged;

			//_mediaPlayer.CommandManager.NextBehavior.EnablingRule = MediaCommandEnablingRule.Always;
			//_mediaPlayer.CommandManager.PreviousBehavior.EnablingRule = MediaCommandEnablingRule.Always;

			//_mediaPlayer.CommandManager.NextReceived += (c, e) => NextTrack();
			//_mediaPlayer.CommandManager.PreviousReceived += (c, e) => PreviousTrack();
			//_mediaPlayer.CommandManager.PlayReceived += (c, e) => Play(CurrentTrack);
			//_mediaPlayer.CommandManager.PauseReceived += (c, e) => Pause();
		}

		public bool IsPlaying => _mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing
		                         || _mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Opening
		                         || _mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Buffering;

		public IObservableCollection<Audio> Tracks { get; }

		public Audio CurrentTrack { get; private set; }

		public TimeSpan Position => _mediaPlayer.PlaybackSession.Position;

		public int RepeatMode { get; set; }

		public bool IsShuffle { get; set; }

		public double Volume
		{
			get => _mediaPlayer.Volume;
			set => _mediaPlayer.Volume = value;
		}

		public void Play(Audio track)
		{
			try
			{
				if (track == null) return;

				if (track.ContentRestricted.HasValue)
				{
					_notificationService.CreateNotification("Аудиозапись недоступна",
						"Она была изъята из публичного доступа");
					return;
				}

				if (track.ToString() != CurrentTrack?.ToString())
				{
					CurrentTrack = track;
					_mediaPlayer.Source = MediaSource.CreateFromUri(track.Url);
					TrackChangedEvent?.Invoke(this, EventArgs.Empty);
				}

				_mediaPlayer.Play();
			}
			catch (Exception e)
			{
				_notificationService.CreateNotification("Произошла ошибка при воспроизведении", e.ToString());
			}
		}

		public void Pause()
		{
			_mediaPlayer.Pause();
		}

		public void NextTrack()
		{
			int index;
			if (!IsShuffle)
			{
				index = Tracks.IndexOf(CurrentTrack);
				if (index == Tracks.Count - 1)
				{
					if (RepeatMode == 2) index = 0;
					else return;
				}
				else
				{
					index++;
				}
			}
			else
			{
				index = new Random().Next(0, Tracks.Count);
			}

			var track = Tracks[index];
			Play(track);
		}

		public void PreviousTrack()
		{
			var index = Tracks.IndexOf(CurrentTrack);
			if (index == 0) index = Tracks.Count - 1;
			else index--;

			var track = Tracks[index];
			Play(track);
		}

		//public void Seek(TimeSpan position)
		//{
		//	try
		//	{
		//		_mediaPlayer.PlaybackSession.Position = position;
		//	}
		//	catch (Exception e)
		//	{
		//		_notificationService.CreateNotification("Произошла ошибка при перемотке", e.ToString());
		//	}
		//}

		private void MediaPlayerOnCurrentStateChanged(MediaPlaybackSession sender, object args)
		{
			DispatcherHelper.ExecuteOnUIThreadAsync(() =>
			{
				PlayStateChangedEvent?.Invoke(this, EventArgs.Empty);
			});
		}

		private void MediaPlayerOnMediaEnded(MediaPlayer sender, object args)
		{
			DispatcherHelper.ExecuteOnUIThreadAsync(() =>
			{
				if (RepeatMode != 1) NextTrack();
				else
				{
					Play(CurrentTrack);
				}
			});
		}

		private void MediaPlayerOnMediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
		{
			_notificationService.CreateNotification("Произошла ошибка при загрузке", $"{args.Error}");

			//Ошибка при загрузке

			//if (args.Error == MediaPlayerError.SourceNotSupported)
			//{
			//    //audio source url may expire
			//    CurrentPlaylist.CurrentItem.Source = null;
			//    TryResolveTrack(CurrentPlaylist.CurrentItem);
			//    return;
			//}
			//else if (args.Error == MediaPlayerError.NetworkError)
			//{
			//    InternetService.GoToOfflineMode();
			//}

			//DispatcherHelper.CheckBeginInvokeOnUI(async () =>
			//{
			//    await ContentDialogService.Show(new ExceptionDialog("Невозможно загрузить аудио файл", $"Невозможно загрузить файл по этой причине: {args.Error.ToString()}", new Exception(args.ErrorMessage)));

			//});
			//Log.Error("Media failed. " + args.Error + " " + args.ErrorMessage);
		}

		private void PlaybackSessionOnPositionChanged(MediaPlaybackSession sender, object args)
		{
			DispatcherHelper.ExecuteOnUIThreadAsync(() =>
			{
				PositionTrackChangedEvent?.Invoke(this, sender.Position);
			});
		}
	}
}