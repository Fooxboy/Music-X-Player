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
using Android.Util;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Services;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models.Music.BlockInfo;
using ImageViews.Rounded;

namespace Fooxboy.MusicX.AndroidApp.Resources.fragments
{
    public class ArtistFragment : Fragment
    {

        public long ArtistID { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_artist, container, false);
            var artistName = view.FindViewById<TextView>(Resource.Id.artist_name);
            var blocks = view.FindViewById<RecyclerView>(Resource.Id.artist_blocks);
            var banner = view.FindViewById<RoundedImageView>(Resource.Id.artist_image);

            var task = Task.Run(() =>
            {
                return Core.VKontakte.Music.Artists.GetById(ArtistID).Result;
            });

            artistName.Text = task.Result.Name;
            banner.SetImageString(ImagesService.BannerArtist(task.Result), banner.Width, banner.Height);
            var blockTracks = new Block();
            return view;
        }
    }
}