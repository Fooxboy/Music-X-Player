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
    public class MainPlayerActivity: Activity, SeekBar.IOnSeekBarChangeListener
    {
        private RoundedImageView cover;
        private TextView position;
        private TextView duration;
        private SeekBar seekBar;
        private TextView title;
        private TextView artist;
        private Button backButton;
        private Button playPauseButton;
        private Button closeButton;
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
            closeButton = FindViewById<Button>(Resource.Id.closeButtonMainPlayer);

            player = PlayerService.Instanse;
            player.MainService.PositionChanged += MainServiceOnPositionChanged;
            player.MainService.CurrentAudioChanged += MainServiceOnCurrentAudioChanged;
            player.MainService.ItemFailed += MainServiceOnItemFailed;
            backButton.Click += BackButtonOnClick;
            closeButton.Click += CloseButtonOnClick;
            playPauseButton.Click += PlayPauseButtonOnClick;
            nextButton.Click += NextButtonOnClick;
            seekBar.SetOnSeekBarChangeListener(this);
            UpdateDataPlayer();
        }

        private void CloseButtonOnClick(object sender, EventArgs e)
        {
            Finish();
        }


        /// <summary>
        /// Обновление всех полей на лаяуте плеера
        /// </summary>
        private void UpdateDataPlayer()
        {
            // cover.SetImageString(player.Cover, 300, 300);
            if (player.MainService.IsPlay) {
                if (player.Cover == "placeholder") cover.SetImageDrawable(GetDrawable(Resource.Drawable.placeholder));
                if (player.Cover != "placeholder") cover.SetImageString(player.Cover, 300, 300);
                position.Text = player.MainService.Position.ToString("m\\:ss");
                duration.Text = player.MainService.Duration.ToString("m\\:ss");
                seekBar.Progress = Convert.ToInt32(player.MainService.Position.TotalSeconds);
                seekBar.Max = Convert.ToInt32(player.CurrentAudioFile.Duration.TotalSeconds);
                title.Text = player.Title;
                artist.Text = player.Artist;

                playPauseButton.Background = GetDrawable(Resource.Drawable.outline_pause_black_24dp);
            }
            
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
        private void MainServiceOnCurrentAudioChanged(object sender, Track args)
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
            position.Text = player.MainService.Position.ToString("m\\:ss");
            seekBar.Progress = Convert.ToInt32(player.MainService.Position.TotalSeconds);
        }

        private void NextButtonOnClick(object sender, EventArgs e)
        {
            player.MainService.NextTrack();
            UpdateDataPlayer();
            //throw new NotImplementedException();
        }

        private void PlayPauseButtonOnClick(object sender, EventArgs e)
        {
            //TODO: сделать именение иконочки
            if (player.MainService.IsPlay)
            {
                playPauseButton.Background = GetDrawable(Resource.Drawable.play_ic);
                player.Pause();
            }
            else
            {
                playPauseButton.Background = GetDrawable(Resource.Drawable.outline_pause_black_24dp);
                player.Play();
            }
            
            //throw new NotImplementedException();
        }

        private void BackButtonOnClick(object sender, EventArgs e)
        {
            player.MainService.BackTrack();
            //player.MainService.
            //throw new NotImplementedException();
        }

        public void OnProgressChanged(SeekBar bar, int progress, bool fromUser)
        {
            if(fromUser) player.MainService.SeekTo(TimeSpan.FromSeconds(progress));

            //throw new NotImplementedException();
        }

        public void OnStartTrackingTouch(SeekBar bar)
        {
            //throw new NotImplementedException();
        }

        public void OnStopTrackingTouch(SeekBar bar)
        {
            //throw new NotImplementedException();
        }
    }
}