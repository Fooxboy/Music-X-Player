using Android.Views;
using Android.Widget;

namespace Fooxboy.MusicX.AndroidApp.Services
{
    public static class MiniPlayerService
    {
        public static View MiniPlayer;
        public static FrameLayout Frame;
        public static void SetView(View miniPlayer)
        {
            MiniPlayer = miniPlayer;
        }

        public static void SetFrame(FrameLayout frame)
        {
            Frame = frame;
        }
    }
}