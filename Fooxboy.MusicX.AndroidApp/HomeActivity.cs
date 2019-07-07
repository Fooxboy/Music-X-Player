using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Fooxboy.MusicX.AndroidApp
{
    [Activity(Label = "Домашний экран")]
    public class HomeActivity:Activity
    {

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);

            SetContentView(Resource.Layout.homeActivity);
        }
    }
}