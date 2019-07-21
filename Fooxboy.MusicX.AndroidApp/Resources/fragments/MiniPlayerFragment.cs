using Android.App;
using Android.OS;
using Android.Views;
using Fooxboy.MusicX.AndroidApp.Services;

namespace Fooxboy.MusicX.AndroidApp.Resources.fragments
{
    public class MiniPlayerFragment:Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.player_min, container, false);
            MiniPlayerService.SetView(view);
            return view;
        }
    }
}