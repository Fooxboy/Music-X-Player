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
using Fooxboy.MusicX.AndroidApp.Converters;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.AndroidApp.Resources.fragments;
using Fooxboy.MusicX.AndroidApp.Services;
using Fooxboy.MusicX.AndroidApp.ViewHolders;
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.AndroidApp.Adapters
{
    class ArtistAdapter : RecyclerView.Adapter, Interfaces.IItemClickListener
    {
        public event Delegates.EventHandler<Artist, Block> ItemClick;
        List<Block> Blocks;
        Fragment Parent;

        public ArtistAdapter(List<Block> blocks, Fragment p)
        {
            Blocks = blocks;
            Parent = p;
        }

        public override int ItemCount => Blocks.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecommendationsViewHolder v = holder as RecommendationsViewHolder;
            v.Caption.Text = Blocks[position].Title;
            v.SetItemClickListener(this);
            v.ShowMoreButton.Click += (sender, e) =>
            {
                if (Blocks[position].Albums?.Count > 0)
                {
                    
                    var frag = new RecommendationPlaylistsFragment();
                    frag.playlists = this.Blocks[position].Albums.ToAlbumsList();
                    Parent.Activity.FindViewById<TextView>(Resource.Id.titlebar_title).Text = Blocks[position].Title;
                    Parent.FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, frag).Commit();

                }
                else
                {
                    var frag = new RecommendationTracksFragment();
                    frag.tracks = this.Blocks[position].Tracks.ToTracksList();
                    Parent.Activity.FindViewById<TextView>(Resource.Id.titlebar_title).Text = Blocks[position].Title;
                    Parent.FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, frag).Commit();
                }
                Toast.MakeText(Application.Context, $"Произошел кликинг по {this.Blocks[position].Title}", ToastLength.Long).Show();
            };
            int counter = 0;
            if (this.Blocks[position].Albums?.Count > 0)
            {
                var BlockAlbums = this.Blocks[position].Albums.Take(2).ToList().ToAlbumsList();
                var adapter = new PlaylistAdapter(BlockAlbums, Blocks[position]);
                adapter.ItemClick += PlaylistAdapterOnItemClick;
                v.List.SetAdapter(adapter);
                v.List.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Horizontal, false));
                v.List.Clickable = true;
            }
            else
            {

                //List<Track> BlockTracks = new List<Track>();
                var BlockTracks = this.Blocks[position].Tracks.ToTracksList().Take(2).ToList();
                /*foreach (var track in tracks_nonvk)
                {
                    if (counter > 1) break;
                    tracksInBlock.Add(track);
                    counter++;
                }*/
                var adapter = new TrackAdapter(BlockTracks, this.Blocks[position]);
                adapter.ItemClick += TrackAdapterOnItemClick;
                v.List.SetAdapter(adapter);
                v.List.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));
                v.List.Clickable = true;
            }
        }

        public void OnClick(View itemView, int position, bool isLongClick)
        {
            //ItemClick?.Invoke(itemView, Blocks[position]); вот честно хз че это
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                Inflate(Resource.Layout.RecommendationLayout, parent, false);
            RecommendationsViewHolder v = new RecommendationsViewHolder(itemView);
            return v;
        }

        private void TrackAdapterOnItemClick(object sender, Track args, Block block)
        {
            Toast.MakeText(Application.Context, $"{args.Title} в блоке {block.Title}", ToastLength.Long).Show();
            //var tracks = this.Blocks.First(b => b.Title == args.blockID).Tracks;
            var tracks = block.Tracks;
            var tracksfiles = tracks.ToTracksList();
            var playlist = new Album();
            playlist.Artists.Add(new Artist()
            {
                Name = "Music X"
            });
            playlist.Cover = "playlist_placeholder";
            playlist.Genres = null;
            playlist.Id = 1000;
            playlist.Tracks = tracksfiles;
            var player = PlayerService.Instanse;
            player.Play(playlist, playlist.Tracks.First(t => t.Url == args.Url));
        }

        private void PlaylistAdapterOnItemClick(object sender, Album args, Block block)
        {
            var fragment = new PlaylistFragment();
            fragment.playlist = args;
            Parent.FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, fragment).Commit();
        }

    }

}