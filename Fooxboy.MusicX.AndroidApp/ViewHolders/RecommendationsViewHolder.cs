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

namespace Fooxboy.MusicX.AndroidApp.ViewHolders
{
    public class RecommendationsViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
    {
        public TextView Caption { get; private set; }
        public RecyclerView List { get; private set; }
        public Button ShowMoreButton { get; private set; }
        public RecommendationsViewHolder(View itemView) : base(itemView)
        {
            Caption = itemView.FindViewById<TextView>(Resource.Id.CardCaption);
            List = itemView.FindViewById<RecyclerView>(Resource.Id.CardList);
            ShowMoreButton = itemView.FindViewById<Button>(Resource.Id.CardButton);
            ShowMoreButton.SetOnClickListener(this);

        }

        public void OnClick(View v)
        {
            throw new NotImplementedException();
        }
    }
}