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

        public SmallBlockAdapterViewHolder(View itemView) : base (itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.smallBlock_title);
        }
    }
}