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

namespace Fooxboy.MusicX.AndroidApp.DialogFragments
{
    public class IncorrectLoginDialogFragment : DialogFragment
    {

        public static IncorrectLoginDialogFragment NewInstance(Bundle bundle)
        {
            IncorrectLoginDialogFragment fragment = new IncorrectLoginDialogFragment();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.IncorrectLoginDialogLayout, container, false);

            var button = view.FindViewById<Button>(Resource.Id.buttonIncorrectLoginOk);
            button.Click += Button_Click;
            return view;

        }

        public override void OnResume()
        {
            base.OnResume();

            var window = Dialog.Window;
            var requestedWidth = (int)(350 * (this.Resources.DisplayMetrics.Density));
            var requestedHeight = (int)(140 * (this.Resources.DisplayMetrics.Density));

            window.SetLayout(requestedWidth, requestedHeight);
            window.SetGravity(GravityFlags.Center);           
        }
        
        private void Button_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }
    }
}