using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Activities;
using Fooxboy.MusicX.AndroidApp.Listeners;
using Fooxboy.MusicX.AndroidApp.Services;

namespace Fooxboy.MusicX.AndroidApp.Resources.fragments
{
    public class MiniPlayerFragment:Fragment, View.IOnClickListener
    {

        PlayerService player;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.player_min, container, false);
            var playbtn = view.FindViewById<Button>(Resource.Id.miniPlayer_Playbtn);
            var nextbtn = view.FindViewById<Button>(Resource.Id.miniPlayer_NextBtn);
            nextbtn.SetOnClickListener(this);
            playbtn.SetOnClickListener(this);
            player = PlayerService.Instanse;

            if(player.MainService.IsPlay) playbtn.SetBackgroundResource(Resource.Drawable.outline_pause_black_24dp);


            MiniPlayerService.SetView(view);
            view.SetOnClickListener(this);
            return view;
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.miniPlayer_Playbtn:
                    if (player.MainService.IsPlay)
                    {
                        var btn = v.FindViewById<Button>(Resource.Id.miniPlayer_Playbtn);
                        btn.SetBackgroundResource(Resource.Drawable.play_ic);
                        player.Pause();
                    }
                    else
                    {
                        var btn = v.FindViewById<Button>(Resource.Id.miniPlayer_Playbtn);
                        btn.SetBackgroundResource(Resource.Drawable.outline_pause_black_24dp);
                        player.Play();
                    }
                break;
                case Resource.Id.miniPlayer_NextBtn:
                    player.MainService.NextTrack();
                break;
                default:
                    //Открытие большого плеера.
                    Intent intent = new Intent(Application.Context, typeof(MainPlayerActivity));
                    intent.SetFlags(ActivityFlags.NewTask);
                    Application.Context.StartActivity(intent);
                    break;
            }
            
        }
    }
}