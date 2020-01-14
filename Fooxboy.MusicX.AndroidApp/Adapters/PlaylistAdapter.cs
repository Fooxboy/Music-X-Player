﻿using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Interfaces;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.AndroidApp.ViewHolders;
using Fooxboy.MusicX.Core.VKontakte.Music;
using Java.IO;

namespace Fooxboy.MusicX.AndroidApp.Adapters
{
    public class PlaylistAdapter:RecyclerView.Adapter, IItemClickListener, View.IOnCreateContextMenuListener
    {
        private List<Album> albums; // plists;
        public event Delegates.EventHandler<Block> ItemClick;
        private string BlockID = "";

        public PlaylistAdapter(List<Album> albums, string block = null)
        {
            this.albums = albums;
            if (!String.IsNullOrEmpty(block)) BlockID = block;
        }


        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            PlaylistViewHolder v = holder as PlaylistViewHolder;
            v.Title.Text = this.plists[position].Name;
            v.SetItemClickListener(this);
            if (this.plists[position].Cover == "playlist_placeholder")
            {
                v.Cover.SetImageResource(Resource.Drawable.playlist_placeholder);
            }else
            {
                var file = new File(this.plists[position].Cover);
                var opt = new BitmapFactory.Options();
                opt.InJustDecodeBounds = true;
                BitmapFactory.DecodeFile(file.AbsolutePath, opt);
                opt.InSampleSize = CalculateInSampleSize(opt, 100, 100);
                opt.InJustDecodeBounds = false;
                Bitmap myBitmap = BitmapFactory.DecodeFile(file.AbsolutePath, opt);
                v.Cover.SetImageBitmap(myBitmap);
            }
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

        public void AddItems(List<PlaylistFile> p)
        {
            this.plists.AddRange(p);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                Inflate(Resource.Layout.PlaylistLayout, parent, false);
            itemView.SetOnCreateContextMenuListener(this);
            PlaylistViewHolder v = new PlaylistViewHolder(itemView);
            return v;
        }

        public void OnClick(View itemView, int position, bool isLongClick)
        {
            ItemClick?.Invoke(itemView, new PlaylistInBlock(this.plists[position], this.BlockID));
        }

        public void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            //TODO контекстное меню если очень надо
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