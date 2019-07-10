using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.AndroidApp.ViewHolders;
using Fooxboy.MusicX.Core.VKontakte.Music;

namespace Fooxboy.MusicX.AndroidApp.Adapters
{
    public class PlaylistAdapter:RecyclerView.Adapter
    {
        private List<PlaylistFile> plists;

        public PlaylistAdapter(List<PlaylistFile> p)
        {
            this.plists = p;
        }


        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            PlaylistViewHolder v = holder as PlaylistViewHolder;
            v.title.Text = this.plists[position].Name;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                Inflate(Resource.Layout.PlaylistLayout, parent, false);
            PlaylistViewHolder v = new PlaylistViewHolder(itemView);
            return v;
        }

        public override int ItemCount
        {
            get
            {
                return plists.Count;

            }
        }
    }
}