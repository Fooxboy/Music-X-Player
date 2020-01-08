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
using Fooxboy.MusicX.AndroidApp.Resources.fragments;
using Fooxboy.MusicX.AndroidApp.Services;
using Fooxboy.MusicX.AndroidApp.ViewHolders;
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.AndroidApp.Adapters
{
    class ArtistAdapter : RecyclerView.Adapter, Interfaces.IItemClickListener
    {
        public event Delegates.EventHandler<Artist, Block> ItemClick;
        List<Artist> Blocks;
        Fragment Parent;

        public ArtistAdapter(List<Artist> blocks, Fragment p)
        {
            Blocks = blocks;
            Parent = p;
        }

        public override int ItemCount => Blocks.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecommendationsViewHolder v = holder as RecommendationsViewHolder;
            v.Caption.Text = Blocks[position].Name;
            v.SetItemClickListener(this);
            v.ShowMoreButton.Click += (sender, e) =>
            {
                if (Blocks[position].Playlists?.Count > 0)
                {
                    //TODO: GOTO PLAYLISTS FRAGMENT (//TODO CREATE PLAYLISTS GRID FRAGMENT)
                    var frag = new RecommendationPlaylistsFragment();
                    frag.playlists = PlaylistsService.CovertToPlaylistFiles(this.Blocks[position].Playlists);
                    Parent.Activity.FindViewById<TextView>(Resource.Id.titlebar_title).Text = Blocks[position].Title;
                    Parent.FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, frag).Commit();

                }
                else
                {
                    var frag = new RecommendationTracksFragment();
                    frag.tracks = MusicService.ConvertToAudioFile(this.Blocks[position].Tracks);
                    Parent.Activity.FindViewById<TextView>(Resource.Id.titlebar_title).Text = Blocks[position].Title;
                    Parent.FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, frag).Commit();
                }
                Toast.MakeText(Application.Context, $"Произошел кликинг по {this.Blocks[position].Title}", ToastLength.Long).Show();
            };
            int counter = 0;
            if (this.Blocks[position].Playlists?.Count > 0)
            {
                var plistsInBlock = PlaylistsService.CovertToPlaylistFiles(this.Blocks[position].Playlists.Take(2).ToList());
                var adapter = new PlaylistAdapter(, "false");
                adapter.ItemClick += AdapterOnPlaylistClick;
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
                adapter.ItemInBlockClick += AdapterOnItemClick;
                v.List.SetAdapter(adapter);
                v.List.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));
                v.List.Clickable = true;
            }
        }

        public void OnClick(View itemView, int position, bool isLongClick)
        {
            ItemClick?.Invoke(itemView, Blocks[position]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                Inflate(Resource.Layout.RecommendationLayout, parent, false);
            RecommendationsViewHolder v = new RecommendationsViewHolder(itemView);
            return v;
        }

        private void AdapterOnItemClick(object sender, AudioInBlock args)
        {
            Toast.MakeText(Application.Context, $"{args.track.Title} в блоке {args.blockID}", ToastLength.Long).Show();
            var tracks = this.Blocks.First(b => b.Title == args.blockID).Tracks;
            var tracksfiles = MusicService.ConvertToAudioFile(tracks);
            var playlist = new PlaylistFile();
            playlist.Artist = "Music X";
            playlist.Cover = "playlist_placeholder";
            playlist.Genre = "";
            playlist.Id = 1000;
            playlist.IsAlbum = false;
            playlist.TracksFiles = tracksfiles;
            var player = PlayerService.Instanse;
            player.Play(playlist, playlist.TracksFiles.First(t => t.SourceString == args.track.SourceString));
        }

        private void AdapterOnPlaylistClick(object sender, Album plist)
        {
            var fragment = new PlaylistFragment();
            fragment.playlist = plist;
            Parent.FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, fragment).Commit();
        }

    }

}