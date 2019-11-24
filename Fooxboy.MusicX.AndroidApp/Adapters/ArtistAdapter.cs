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
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.AndroidApp.Services;
using Fooxboy.MusicX.AndroidApp.ViewHolders;
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.AndroidApp.Adapters
{
    class ArtistAdapter : RecyclerView.Adapter
    {

        List<IBlock> Blocks;

        public ArtistAdapter(List<IBlock> blocks)
        {
            Blocks = blocks;
        }

        public override int ItemCount => Blocks.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecommendationsViewHolder v = holder as RecommendationsViewHolder;
            v.Caption.Text = Blocks[position].Title;
            int counter = 0;
            if (this.Blocks[position].Playlists?.Count > 0)
            {
                var plistsInBlock = PlaylistsService.CovertToPlaylistFiles(this.Blocks[position].Playlists.Take(2).ToList());
                var adapter = new PlaylistAdapter(plistsInBlock);
                //adapter.ItemClick += AdapterOnPlaylistClick;
                v.List.SetAdapter(adapter);
                v.List.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Horizontal, false));
                v.List.Clickable = true;
            }
            else
            {

                List<AudioFile> tracksInBlock = new List<AudioFile>();
                var tracks_nonvk = MusicService.ConvertToAudioFile(this.Blocks[position].Tracks);
                foreach (var track in tracks_nonvk)
                {
                    if (counter > 1) break;
                    tracksInBlock.Add(track);
                    counter++;
                }
                var adapter = new TrackAdapter(tracksInBlock, this.Blocks[position].Title);
                //adapter.ItemInBlockClick += AdapterOnItemClick;
                v.List.SetAdapter(adapter);
                v.List.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));
                v.List.Clickable = true;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                Inflate(Resource.Layout.RecommendationLayout, parent, false);
            RecommendationsViewHolder v = new RecommendationsViewHolder(itemView);
            return v;
        }
    }

}