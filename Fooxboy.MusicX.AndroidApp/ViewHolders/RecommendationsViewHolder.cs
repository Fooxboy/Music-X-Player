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
using Fooxboy.MusicX.AndroidApp.Interfaces;

namespace Fooxboy.MusicX.AndroidApp.ViewHolders
{
    public class RecommendationsViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
    {
        private IItemClickListener itemClickListener;

        public TextView Caption { get; private set; }
        public RecyclerView List { get; private set; }
        public Button ShowMoreButton { get; private set; }
        public RecommendationsViewHolder(View itemView) : base(itemView)
        {
            Caption = itemView.FindViewById<TextView>(Resource.Id.CardCaption);
            List = itemView.FindViewById<RecyclerView>(Resource.Id.CardList);
            ShowMoreButton = itemView.FindViewById<Button>(Resource.Id.CardButton);
            ShowMoreButton.SetOnClickListener(this);
            itemView.SetOnClickListener(this);

        }

        public void SetItemClickListener(IItemClickListener listener)
        {
            this.itemClickListener = listener;
        }

        public void OnClick(View v)
        {
            itemClickListener.OnClick(v, AdapterPosition, false);
        }
    }
}