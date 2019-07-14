using System;
using System.Threading.Tasks;
using Android.OS;
using Java.Lang;

namespace Fooxboy.MusicX.AndroidApp.Services
{
    public static class TaskService
    {
        private  static readonly Handler Handler = new Handler(Looper.MainLooper);
        public static void Run(Action a)
        {
            Task.Run(a);
        }

        public static void RunOnUI(Action a)
        {
            Handler.Post(new Runnable((a)));
        }
    }
}