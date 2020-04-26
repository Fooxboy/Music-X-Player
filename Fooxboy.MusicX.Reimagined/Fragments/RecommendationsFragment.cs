using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Fooxboy.MusicX.Reimagined.Fragments
{
    public class RecommendationsFragment : Fragment
    {

        bool isLoading = true;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.frag_recommendations, container, false);

            var list_blocks = view.FindViewById<RecyclerView>(Resource.Id.recommendations_blocks);
            var loading = view.FindViewById<ProgressBar>(Resource.Id.recommendations_loading);

            return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}