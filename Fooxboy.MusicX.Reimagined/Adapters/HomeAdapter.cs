using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.Widget;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.Core.Models;
using Fooxboy.MusicX.Reimagined.Models;

namespace Fooxboy.MusicX.Reimagined.Adapters
{
    public class HomeAdapter : RecyclerView.Adapter
    {
        List<MainBlock> Blocks = new List<MainBlock>();

        public HomeAdapter(List<MainBlock> blocks)
        {
            Blocks = blocks;
        }

        public override int ItemCount => Blocks.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            HomeAdapterViewHolder vh = holder as HomeAdapterViewHolder;
            if(Blocks[position].Blocks == null)
            {
                vh.ItemsList.Visibility = ViewStates.Gone;
                vh.Description.Visibility = ViewStates.Visible;
                vh.ShowMoreButton.Visibility = ViewStates.Gone;
            }
            else
            {
                var Adapter = new SmallBlockAdapter(Blocks[position].Blocks);
                vh.ItemsList.SetAdapter(Adapter);
                vh.ItemsList.SetLayoutManager(new LinearLayoutManager(Android.App.Application.Context, LinearLayoutManager.Horizontal, false));
            }

            vh.Title.Text = Blocks[position].Title;
            vh.Description.Text = Blocks[position].Description;

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
            Inflate(Resource.Layout.model_block, parent, false);
            HomeAdapterViewHolder v = new HomeAdapterViewHolder(itemView);
            return v;
        }
    }

    public class HomeAdapterViewHolder : RecyclerView.ViewHolder
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
        public TextView Title { get; set; }
        public TextView Description { get; set; }
        public Button ShowMoreButton { get; set; }
        public RecyclerView ItemsList { get; set; }

        public HomeAdapterViewHolder(View itemView) : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.block_Title);
            Description = itemView.FindViewById<TextView>(Resource.Id.block_Text);
            ShowMoreButton = itemView.FindViewById<Button>(Resource.Id.block_ShowMoreButton);
            ItemsList = itemView.FindViewById<RecyclerView>(Resource.Id.block_items);
        }

    }
}