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
using Fooxboy.MusicX.AndroidApp.Converters;
using Fooxboy.MusicX.AndroidApp.Interfaces;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.AndroidApp.ViewHolders;
using Java.Interop;
using Java.IO;
using Java.Net;
using Object = Java.Lang.Object;

namespace Fooxboy.MusicX.AndroidApp.Adapters
{
    
  
    public class TrackAdapter : RecyclerView.Adapter, IItemClickListener, View.IOnLongClickListener, View.IOnCreateContextMenuListener
    {
        List<TracksViewHolder> holders = new List<TracksViewHolder>();
        public event Delegates.EventHandler<Track, Block> ItemClick;
        private List<Track> tracks;
        private Block block;
        private int position;

        public int GetPosition()
        {
            return position;
        }

        public void SetPosition(int pos)
        {
            this.position = pos;
        }

        public TrackAdapter(List<Track> track, Block b = null)
        {
            this.tracks = track;
            if (!(b is null)) this.block = b;
        }


        public override void OnBindViewHolder(RecyclerView.ViewHolder hold, int position)
        {
            var holder = hold as TracksViewHolder;
            holder.Artist.Text = tracks[position].Artist;
            holder.Title.Text = tracks[position].Title;
            holder.SetItemClickListener(this);
            holder.Duration.Text = tracks[position].Duration.ToDuration();
            holder.ItemView.SetOnLongClickListener(this);
            //holder.ItemView.SetOnLongClickListener(this);

            if(tracks[position].Album is null)
            {
                holder.Cover.SetImageResource(Resource.Drawable.placeholder);
            }else
            {
                if (tracks[position].Album?.Cover is null)
                {
                    holder.Cover.SetImageResource(Resource.Drawable.placeholder);
                }
                else
                {
                    var file = new File(tracks[position].Album.Cover);
                    var opt = new BitmapFactory.Options();
                    opt.InJustDecodeBounds = true;
                    opt.InSampleSize = CalculateInSampleSize(opt, 50, 50);
                    opt.InJustDecodeBounds = false;
                    Bitmap myBitmap = BitmapFactory.DecodeFile(file.Path, opt);
                    holder.Cover.SetImageBitmap(myBitmap);
                }
            }
            
            //holder.Cover.SetImageResource(Resource.Drawable.placeholder);
            holders.Add(holder);
        }

        public void AddItems(List<Track> track)
        {
            this.tracks.AddRange(track);
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
            itemView.SetOnCreateContextMenuListener(this);
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

            ItemClick?.Invoke(itemView, tracks[position], block);
            /*if(this.blockID != "" && this.blockID != "false")
            {
                AudioInBlock data = new AudioInBlock(tracks[position], this.blockID);
                ItemInBlockClick?.Invoke(itemView, data);
            }
            else {
                ItemClick?.Invoke(itemView, tracks[position]);
            }*/
            
        }

        public bool OnLongClick(View v)
        {
            SetPosition(holders.First(h => h.ItemView == v).Position);
            return false;
        }

        public void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if(block is null)
            {
                menu.Add(Menu.None, 0, Menu.None, "Перейти к исполнителю");
            }else{
                menu.Add(Menu.None, 0, Menu.None, "Перейти к исполнителю");
                menu.Add(Menu.None, 1, Menu.None, "Удалить");
            }
        }
    }
}