using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.Core.Models;
using Fooxboy.MusicX.Reimagined.Services;
using ImageViews.Rounded;

namespace Fooxboy.MusicX.Reimagined.Adapters
{
    class SmallBlockAdapter : RecyclerView.Adapter
    {

        List<Block> Blocks = new List<Block>();
        public override int ItemCount => Blocks.Count;

        public SmallBlockAdapter(List<Block> blocks)
        {
            Blocks = blocks;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            SmallBlockAdapterViewHolder vh = holder as SmallBlockAdapterViewHolder;
            vh.Title.Text = Blocks[position].Title;
            if(Blocks[position].Albums != null)
            {
                var imageString = ImagesService.CoverPlaylist(Blocks[position].Albums[new Random().Next(0, Blocks[position].Albums.Count - 1)]);
                if (imageString != "placeholder") vh.Background.SetImageString(imageString, 150, 150);
            }
            else
            {
                var imageString = ImagesService.CoverTrack(Blocks[position].Tracks[new Random().Next(0, Blocks[position].Tracks.Count - 1)]);
                if(imageString != "placeholder") vh.Background.SetImageString(imageString, 150, 150);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
            Inflate(Resource.Layout.model_block_small, parent, false);
            SmallBlockAdapterViewHolder v = new SmallBlockAdapterViewHolder(itemView);
            return v;
        }
    }

    public class SmallBlockAdapterViewHolder : RecyclerView.ViewHolder
    {

        public TextView Title { get; set; }
        public RoundedImageView Background { get; set; }

        public SmallBlockAdapterViewHolder(View itemView) : base (itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.smallBlock_title);
            Background = itemView.FindViewById<RoundedImageView>(Resource.Id.smallBlock_Background);
        }
    }
}