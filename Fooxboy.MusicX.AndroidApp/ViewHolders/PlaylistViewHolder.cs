using System;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Fooxboy.MusicX.AndroidApp.ViewHolders
{
    public class PlaylistViewHolder : RecyclerView.ViewHolder
    {

        public TextView title { get; private set; }

        public PlaylistViewHolder(View itemView) : base(itemView)
        {
            title = itemView.FindViewById<TextView>(Resource.Id.textViewPlaylist);
        }
    }
}