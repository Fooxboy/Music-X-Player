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
            /* TODO: Ой бля ну тут полный пиздец лучше с нуля переписать мой тебе совет
            v.ShowMoreButton.Click += (sender, e) =>
            {
                if(Blocks[position].Albums?.Count > 0)
                {
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
            };*/
            //Установка заголовка согласно нужной рекомендации
            v.Caption.Text = this.Blocks[position].Title;
            v.SetItemClickListener(this);
            //вот прям тут да между этими комментами
            int counter = 0;
            if (this.Blocks[position].Playlists?.Count > 0)
            {
                var plistsInBlock = PlaylistsService.CovertToPlaylistFiles(this.Blocks[position].Playlists.Take(2).ToList());
                var adapter = new PlaylistAdapter(plistsInBlock);
                adapter.ItemClick += PlaylistAdapterOnItemClick;
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