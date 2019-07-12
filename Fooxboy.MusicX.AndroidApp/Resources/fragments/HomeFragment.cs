using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Adapters;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.AndroidApp.Services;
using Java.Lang;

namespace Fooxboy.MusicX.AndroidApp.Resources.fragments
{
    public class HomeFragment : Fragment, AbsListView.IOnScrollListener
    {

        private int preLast;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }

        TrackAdapter adapter = null;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.homeActivity, container, false);

            var progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBarLoagingTracks);

            progressBar.Visibility = ViewStates.Visible;

            List<AudioFile> tracks = new List<AudioFile>();
            adapter = new TrackAdapter(tracks);

            var tracksView = view.FindViewById<RecyclerView>(Resource.Id.TracksView);
            Handler handler = new Handler(Looper.MainLooper);

            tracksView.SetAdapter(adapter);
            tracksView.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));

            var task = Task.Run(() =>
            {
                tracks = MusicService.GetMusicLibrary(20, 0);
                var i = 1 + 1;
            });


            task.ContinueWith((t) =>
            {
                while (tracks.Count == 0)
                {
                    System.Threading.Thread.Sleep(500);
                }

                handler.Post(new Runnable(() =>
                {
                    adapter.AddItems(tracks);
                    adapter.NotifyDataSetChanged();
                    progressBar.Visibility = ViewStates.Invisible;
                }));
                
            });

            var a = task.ConfigureAwait(false);

            /* плейлисты ебац */

            var playlistlistview = view.FindViewById<RecyclerView>(Resource.Id.playlists);
            var plist = new List<Models.PlaylistFile>();
            for (int i = 0; i < 15; i++)
            {
                plist.Add(new Models.PlaylistFile()
                {
                    Name = $"Плейлист {i}",
                });
            }
            playlistlistview.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Horizontal, false));
            playlistlistview.SetAdapter(new PlaylistAdapter(plist));

            return view;

        }

        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            try
            {
                //Toast.MakeText(Application.Context, "Оно скролитЬся", ToastLength.Long).Show();
                if (view.LastVisiblePosition == view.Adapter.Count - 1 && view.GetChildAt(view.ChildCount - 1).Bottom <= view.Height)
                {
                    Toast.MakeText(Application.Context, "Мы на дне", ToastLength.Long).Show();
                }
            }
            catch
            {

            }
            

        }

        public void OnScrollStateChanged(AbsListView view, [GeneratedEnum] ScrollState scrollState)
        {
            //throw new NotImplementedException();
        }
    }
}