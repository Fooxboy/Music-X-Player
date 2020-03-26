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
using Fooxboy.MusicX.AndroidApp.Models;
using Java.Interop;

namespace Fooxboy.MusicX.AndroidApp.Services
{
    public class NavigationService
    {
        public NavigationModel CurrentFragment { get; set; }
        public Stack<NavigationModel> PreviousFragments = new Stack<NavigationModel>();
        public ImageView BackButton { get; set; }



        public NavigationService(ImageView btn)
        {
            BackButton = btn;
        }

        public void SetCurrent(Fragment f, string t)
        {
            if (!(CurrentFragment is null))
            {
                PreviousFragments.Push(CurrentFragment);
                BackButton.Visibility = ViewStates.Visible;
            }
            CurrentFragment = new NavigationModel()
            {
                Title = t,
                Fragment = f
            };
        }

        public NavigationModel GoBack()
        {
            CurrentFragment = PreviousFragments.Pop();
            if (PreviousFragments.Count == 0) BackButton.Visibility = ViewStates.Gone;
            return CurrentFragment;
        }
    }
}