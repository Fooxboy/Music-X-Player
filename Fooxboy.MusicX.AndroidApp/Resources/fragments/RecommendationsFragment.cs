using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Adapters;
using Fooxboy.MusicX.AndroidApp.Converters;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.VKontakte.Music;
using Java.Lang;

namespace Fooxboy.MusicX.AndroidApp.Resources.fragments
{
    public class RecommendationsFragment : Fragment
    {

        RecommendationAdapter adapter = null;
        bool hasLoading = true;

        List<Block> blocks = new List<Block>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        private void AdapterOnItemClick(object sender, IBlock item)
        {
            Toast.MakeText(Application.Context, $"{item.Title}", ToastLength.Long).Show();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var view = inflater.Inflate(Resource.Layout.activity_recommendations, container, false);
            var list = view.FindViewById<RecyclerView>(Resource.Id.list_recommendations);
            var progress = view.FindViewById<ProgressBar>(Resource.Id.progressBar_recommendations);
            adapter = new RecommendationAdapter(new List<Block>(), this);
            adapter.ItemClick += AdapterOnItemClick;
            list.Clickable = true;
            //progress.Visibility = ViewStates.Invisible;
            Handler handler = new Handler(Looper.MainLooper);
            var scroll_listener = new Listeners.OnScrollToBottomListener(() =>
            {
                if (!hasLoading) return;
                
                if(blocks?.Count > 0)
                {

                    Task.Run(() =>
                    {
                        System.Threading.Thread.Sleep(300);
                        

                            var buffer = new List<Block>();
                            buffer.Add(blocks.First());
                            adapter.AddBlocks(buffer);
                        handler.Post(new Runnable(() =>
                        {
                            adapter.NotifyItemRangeChanged(adapter.ItemCount - 1, 1);
                        }));
                            
                    }).ContinueWith((t) =>
                    {
                        blocks.Remove(blocks.First());
                        progress.Visibility = ViewStates.Invisible;
                    });
                   
                }
                else
                {
                    hasLoading = false;
                    return;
                }
            });

            List<Block> recs = new List<Block>();
            Task.Run(() =>
            {
                recs = Core.Api.GetApi().VKontakte.Music.Recommendations.Get().ToBlocksList();
                //recs = Recommendations.().Blocks;
            }).ContinueWith((t) =>
            {
                blocks.AddRange(recs);
                scroll_listener.InvokeCallback();
            });

            list.AddOnScrollListener(scroll_listener);
            list.SetAdapter(adapter);
            list.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));
            return view;
           
        }


    }
}