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
using Fooxboy.MusicX.AndroidApp.Interfaces;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.AndroidApp.Resources.fragments;
using Fooxboy.MusicX.AndroidApp.Services;
using Fooxboy.MusicX.AndroidApp.ViewHolders;
using Fooxboy.MusicX.Core.Interfaces;
using static Android.Views.View;

namespace Fooxboy.MusicX.AndroidApp.Adapters
{
    public class RecommendationAdapter : RecyclerView.Adapter, IItemClickListener
    {

        public event Delegates.EventHandler<Block> ItemClick;

        public override int ItemCount
        {
            get
            {
                return Blocks.Count;
            }
        }

        List<Block> Blocks;
        Fragment Parent;
        List<RecommendationsViewHolder> ViewHolds = new List<RecommendationsViewHolder>();

        public RecommendationAdapter(List<Block> blocks, Fragment parent)
        {
            this.Blocks = blocks;
            this.Parent = parent;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecommendationsViewHolder v = holder as RecommendationsViewHolder;

            v.ShowMoreButton.Click += (sender, e) =>
            {
                if(Blocks[position].Albums?.Count > 0)
                {
                    var Fragment = new RecommendationPlaylistsFragment();
                    Fragment.playlists = Blocks[position].Albums.ToAlbumsList();
                    Parent.Activity.FindViewById<TextView>(Resource.Id.titlebar_title).Text = Blocks[position].Title;
                    Parent.FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, Fragment).Commit();
                }
                else
                {
                    var Fragment = new RecommendationTracksFragment();
                    Fragment.tracks = Blocks[position].Tracks.ToTracksList();
                    Parent.Activity.FindViewById<TextView>(Resource.Id.titlebar_title).Text = Blocks[position].Title;
                    Parent.FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, Fragment).Commit();
                }
            };
            //Установка заголовка согласно нужной рекомендации
            v.Caption.Text = this.Blocks[position].Title;
            v.SetItemClickListener(this);
            //вот прям тут да между этими комментами
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

                List<Track> BlockTracks = this.Blocks[position].Tracks.Take(3).ToList().ToTracksList();
                //var tracks_nonvk = this.Blocks[position].Tracks;
                //Не знаю зачем я это сделал, но на всякий случай просто оставлю комментом
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
            this.ViewHolds.Add(v);
        }

        private void TrackAdapterOnItemClick(object sender, Track args, Block block)
        {
            Toast.MakeText(Application.Context, $"{args.Title} в блоке {block.Title}", ToastLength.Long).Show();
            var tracks = this.Blocks.First(b => b.Title == block.Title).Tracks;
            var tracksfiles = Converters.TracksConverter.ToTracksList(tracks);
            var playlist = new Album();
            playlist.Artists[0] = new Artist()
            {
                Name = "Music X"
            };
            playlist.Cover = "playlist_placeholder";
            playlist.Genres = null;
            playlist.Id = 1000;
            playlist.AccessKey = null;
            playlist.Tracks = tracksfiles;
            var player = PlayerService.Instanse;
            player.Play(playlist, playlist.Tracks.First(t => t.Url == args.Url));
        }

        public void AddBlocks(List<Block> t)
        {
            this.Blocks.AddRange(t);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                Inflate(Resource.Layout.RecommendationLayout, parent, false);
            RecommendationsViewHolder v = new RecommendationsViewHolder(itemView);
            return v;
        }

        private void PlaylistAdapterOnItemClick(object sender, Album args, Block block)
        {
            var fragment = new PlaylistFragment();
            fragment.playlist = args;
            Parent.FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, fragment).Commit();
        }

        public void OnClick(View itemView, int position, bool isLongClick)
        {
            ItemClick?.Invoke(itemView, Blocks[position]);
        }
    }
}