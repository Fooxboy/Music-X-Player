using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Activities;
using Fooxboy.MusicX.AndroidApp.Services;

namespace Fooxboy.MusicX.AndroidApp.Resources.fragments
{
    public class MiniPlayerFragment:Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.player_min, container, false);
            MiniPlayerService.SetView(view);

            var linearLayout = view.FindViewById<LinearLayout>(Resource.Id.miniplayerLayout);
            linearLayout.Click += (sender, args) =>
            {
                Intent intent = new Intent(Application.Context, typeof(MainPlayerActivity));
                intent.SetFlags(ActivityFlags.NewTask);
                StartActivity(intent);
            };
            return view;
        }
    }
}