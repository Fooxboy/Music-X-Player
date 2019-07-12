using System;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Fooxboy.MusicX.AndroidApp.ViewHolders
{
    public class PlaylistViewHolder : RecyclerView.ViewHolder
    {
        public TextView Title { get; private set; }

        public ImageView Cover { get; private set; }

        public PlaylistViewHolder(View itemView) : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.textViewPlaylist);
            Cover = itemView.FindViewById<ImageView>(Resource.Id.ImagePlaylist);
        }
    }
}