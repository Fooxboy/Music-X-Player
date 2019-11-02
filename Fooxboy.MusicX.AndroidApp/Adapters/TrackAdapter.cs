using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Interfaces;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.AndroidApp.ViewHolders;
using Java.Interop;
using Java.IO;
using Java.Net;
using Object = Java.Lang.Object;

namespace Fooxboy.MusicX.AndroidApp.Adapters
{
    
  
    public class TrackAdapter : RecyclerView.Adapter, IItemClickListener
    {

        public event Delegates.EventHandler<AudioFile> ItemClick; 
        private List<AudioFile> tracks;

        public TrackAdapter(List<AudioFile> t)
        {
            this.tracks = t;
        }


        public override void OnBindViewHolder(RecyclerView.ViewHolder hold, int position)
        {
            var holder = hold as TracksViewHolder;
            holder.Artist.Text = tracks[position].Artist;
            holder.Title.Text = tracks[position].Title;
            holder.SetItemClickListener(this);
            holder.Duration.Text = tracks[position].DurationMinutes;

            if (tracks[position].Cover == "placeholder")
            {
                holder.Cover.SetImageResource(Resource.Drawable.placeholder);
            }else
            {
                var file = new File(tracks[position].Cover);
                var opt = new BitmapFactory.Options();
                opt.InJustDecodeBounds = true;
                //BitmapFactory.DecodeFile(file.AbsolutePath, opt);
                opt.InSampleSize = CalculateInSampleSize(opt, 50, 50);
                opt.InJustDecodeBounds = false;
                Bitmap myBitmap = BitmapFactory.DecodeFile(file.Path, opt);
                holder.Cover.SetImageBitmap(myBitmap);
            }
            //holder.Cover.SetImageResource(Resource.Drawable.placeholder);
        }

        public void AddItems(List<AudioFile> t)
        {
            this.tracks.AddRange(t);
        }

        public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Начальная высота и ширина изображения
            int height = options.OutHeight;
            int width = options.OutWidth;
            int inSampleSize = 1;

            if (height > reqHeight || width > reqWidth)
            {

                int halfHeight = height / 2;
                int halfWidth = width / 2;

                // Рассчитываем наибольшее значение inSampleSize, которое равно степени двойки
                // и сохраняем высоту и ширину, когда они больше необходимых
                while ((halfHeight / inSampleSize) > reqHeight
                        && (halfWidth / inSampleSize) > reqWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return inSampleSize;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                Inflate(Resource.Layout.TrackLayout, parent, false);
            var v = new TracksViewHolder(itemView);
            return v;
        }

        public override int ItemCount
        {
            get
            {
                return tracks.Count;

            }
        }

        public void OnClick(View itemView, int position, bool isLongClick)
        {
            ItemClick?.Invoke(itemView, tracks[position]);
        }
    }
}