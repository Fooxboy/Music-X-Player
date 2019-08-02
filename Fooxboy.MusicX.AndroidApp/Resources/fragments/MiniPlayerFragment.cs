using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Fooxboy.MusicX.AndroidApp.Activities;
using Fooxboy.MusicX.AndroidApp.Listeners;
using Fooxboy.MusicX.AndroidApp.Services;

namespace Fooxboy.MusicX.AndroidApp.Resources.fragments
{
    public class MiniPlayerFragment:Fragment, View.IOnClickListener
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.player_min, container, false);
            MiniPlayerService.SetView(view);
            view.SetOnClickListener(this);
            return view;
        }

        public void OnClick(View v)
        {
            //Открытие большого плеера.
            Intent intent = new Intent(Application.Context, typeof(MainPlayerActivity));
            intent.SetFlags(ActivityFlags.NewTask);
            Application.Context.StartActivity(intent);
        }
    }
}