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
    public class RecommendationAdapter : RecyclerView.Adapter
    {
        public override int ItemCount
        {
            get
            {
                return Blocks.Count;
            }
        }

        List<IBlock> Blocks;

        public RecommendationAdapter(List<IBlock> blocks)
        {
            this.Blocks = blocks;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecommendationsViewHolder v = holder as RecommendationsViewHolder;
            //Установка заголовка согласно нужной рекомендации
            v.Caption.Text = this.Blocks[position].Title;
            //вот прям тут да между этими комментами
            int counter = 0;
            if (this.Blocks[position].Playlists?.Count > 0)
            {
                List<PlaylistFile> playlistsInBlock = new List<PlaylistFile>();
                
                foreach(var playlist in this.Blocks[position].Playlists)
                {
                    if (counter > 2) break;
                    playlistsInBlock.Add(Converters.PlaylistConverter.FromCoreToAndroid(playlist));
                    counter++;
                }
                v.List.SetAdapter(new PlaylistAdapter(playlistsInBlock));
                v.List.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Horizontal, false));
            }
            else
            {

                List<AudioFile> tracksInBlock = new List<AudioFile>();
                var tracks_nonvk = MusicService.ConvertToAudioFile(this.Blocks[position].Tracks);
                foreach (var track in tracks_nonvk)
                {
                    if (counter > 1) break;
                    tracksInBlock.Add(Converters.AudioConverter.FromCoreToAndroid(track));
                    counter++;
                }
                
                v.List.SetAdapter(new TrackAdapter(tracksInBlock));
                v.List.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));
            }
            
        }

        public void AddBlocks(List<IBlock> t)
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
    }
}