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
using Fooxboy.MusicX.AndroidApp.Adapters;
using Fooxboy.MusicX.AndroidApp.Converters;
using Fooxboy.MusicX.AndroidApp.Models;
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
            Activity.FindViewById<TextView>(Resource.Id.titlebar_title).Text = "Исполнитель";
            var task = Task.Run(() =>
            {
                return Core.Api.GetApi().VKontakte.Music.Artists.Get(ArtistID);
                //return Core.VKontakte.Music.Artists.GetById(ArtistID).Result;
            });

            artistName.Text = task.Result.Name;
            if (task.Result.Name == "Fooxboy") artistName.Text = "Fooxboy (сделал Music X)";
            banner.SetImageString(ImagesService.BannerArtist(task.Result), 550, 250);
            /*var albums = new Models.Block()
            {
                Title = "Альбомы",
                Albums = Core.Api.GetApi().VKontakte.Music.Albums.
            };
            albums.Playlists = task.Result.Albums;
            var tracks = new ArtistBlock("Треки");
            tracks.Tracks = task.Result.PopularTracks;
            var list = new List<ArtistBlock>();
            list.Add(tracks);
            list.Add(albums);*/
            blocks.SetAdapter(new ArtistAdapter(task.Result.Blocks.ToBlocksList(), this));
            blocks.Clickable = true;
            blocks.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));
            return view;
        }
    }
}