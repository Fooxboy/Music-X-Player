using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.Core.Interfaces;
using ImageViews.Rounded;
using Java.IO;
using Java.Net;
using Path = System.IO.Path;

namespace Fooxboy.MusicX.AndroidApp.Services
{
    public static class ImagesService
    {
        static string path = Xamarin.Essentials.FileSystem.AppDataDirectory;
        public static string CoverTrack(ITrack track)
        {
            if(track.Album?.Id != 0)
            {
                return CoverPlaylistById(track.Album.Id, track.Album.Cover);
            }else
            {
                string filename = Path.Combine(path, $"VKTrackId{track.Id}.jpg");

                if (!System.IO.File.Exists(filename))
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(new Uri(track.Album.Cover), filename);
                    }
                }

                return filename;
            }
        }

        public static string PhotoUser(string urlPhoto)
        {
            string filename = Path.Combine(path, $"User{urlPhoto.GetHashCode()}Photo.jpg");

            if (!System.IO.File.Exists(filename))
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(new Uri(urlPhoto), filename);
                }
            }
            return filename;
        }

        public static void SetImageString(this RoundedImageView imageview, string pathImage, int decodePixelWidth, int decodePixelHeight)
        {
            var file = new File(pathImage);
            var opt = new BitmapFactory.Options();
            opt.InJustDecodeBounds = true;
            BitmapFactory.DecodeFile(file.AbsolutePath, opt);
            opt.InSampleSize = CalculateInSampleSize(opt, decodePixelWidth, decodePixelHeight);
            opt.InJustDecodeBounds = false;
            Bitmap myBitmap = BitmapFactory.DecodeFile(file.AbsolutePath, opt);
            imageview.SetImageBitmap(myBitmap);
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

        public static string CoverPlaylist(IAlbum playlist)
        {
            return CoverPlaylistById(playlist.Id, playlist.Cover);
        }

        public static string CoverPlaylistById(long playlistId, string uriImage)
        {
            string filename = System.IO.Path.Combine(path, $"VKPlaylistId{playlistId}.jpg");

            if(!System.IO.File.Exists(filename))
            {
                using(var client = new WebClient())
                {
                    client.DownloadFile(new Uri(uriImage), filename);
                }
            }

            return filename;
        }

        public static string BannerArtist(IArtist artist)
        {
            string filename = Path.Combine(path, $"VKArtistId{artist.Id}.jpg");
            if (!System.IO.File.Exists(filename))
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(new Uri(artist.Banner), filename);
                }
            }

            return filename;
        }
    }
}