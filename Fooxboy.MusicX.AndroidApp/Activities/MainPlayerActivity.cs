using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.AndroidApp.Services;
using ImageViews.Rounded;

namespace Fooxboy.MusicX.AndroidApp.Activities
{
    [Activity(Label = "Music X")]
    public class MainPlayerActivity: Activity
    {
        private RoundedImageView cover;
        private TextView position;
        private TextView duration;
        private SeekBar seekBar;
        private TextView title;
        private TextView artist;
        private Button backButton;
        private Button playPauseButton;
        private Button nextButton;
        private PlayerService player;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_mainPlayer);
            cover = FindViewById<RoundedImageView>(Resource.Id.coverTrackMainPlayer);
            position = FindViewById<TextView>(Resource.Id.positionTrackMainPlayer);
            duration = FindViewById<TextView>(Resource.Id.durationTrackMainPlayer);
            seekBar = FindViewById<SeekBar>(Resource.Id.seekbarTrackMainPlayer);
            title = FindViewById<TextView>(Resource.Id.titleTrackMainPlayer);
            artist = FindViewById<TextView>(Resource.Id.artistTrackMainPlayer);
            backButton = FindViewById<Button>(Resource.Id.backButtonTrackMainPlayer);
            playPauseButton = FindViewById<Button>(Resource.Id.playPauseButtonTrackMainPlayer);
            nextButton = FindViewById<Button>(Resource.Id.nextButtonTrackMainPlayer);

            player = PlayerService.Instanse;
            player.MainService.PositionChanged += MainServiceOnPositionChanged;
            player.MainService.CurrentAudioChanged += MainServiceOnCurrentAudioChanged;
            player.MainService.ItemFailed += MainServiceOnItemFailed;
            backButton.Click += BackButtonOnClick;
            playPauseButton.Click += PlayPauseButtonOnClick;
            nextButton.Click += NextButtonOnClick;
            UpdateDataPlayer();
        }


        /// <summary>
        /// Обновление всех полей на лаяуте плеера
        /// </summary>
        private void UpdateDataPlayer()
        {
            cover.SetImageString(player.Cover, 300, 300);
            position.Text = player.MainService.Position.ToString("g");
            duration.Text = player.MainService.Duration.ToString("g");
            seekBar.Progress = Convert.ToInt32(player.MainService.Position.TotalSeconds);
            seekBar.Max = Convert.ToInt32(player.CurrentAudioFile.DurationSeconds);
            title.Text = player.Title;
            artist.Text = player.Artist;
        }
        
        /// <summary>
        /// Возникла ошибка при воспроизведении трека
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MainServiceOnItemFailed(object sender, Exception args)
        {
            //TODO: возникла ошибка при воспроизведении
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Изменение текущего трека
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MainServiceOnCurrentAudioChanged(object sender, AudioFile args)
        {
           UpdateDataPlayer();
        }

        /// <summary>
        /// Обновление позиции 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MainServiceOnPositionChanged(object sender, TimeSpan args)
        {
            position.Text = player.MainService.Position.ToString("g");
            seekBar.Progress = Convert.ToInt32(player.MainService.Position.TotalSeconds);
        }

        private void NextButtonOnClick(object sender, EventArgs e)
        {
            player.MainService.NextTrack();
            //throw new NotImplementedException();
        }

        private void PlayPauseButtonOnClick(object sender, EventArgs e)
        {
            //TODO: сделать именение иконочки
            if (player.MainService.IsPlay) player.Pause();
            else player.Play();
            //throw new NotImplementedException();
        }

        private void BackButtonOnClick(object sender, EventArgs e)
        {
            //TODO: сделать в PlayingService переключение назад.
            //player.MainService.
            //throw new NotImplementedException();
        }
    }
}