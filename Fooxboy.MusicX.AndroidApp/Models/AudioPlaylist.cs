using System;
using System.Collections.Generic;
using System.Net.Mime;
using Android.App;
using Android.Widget;

namespace Fooxboy.MusicX.AndroidApp.Models
{
    public class AudioPlaylist
    {
        private Track currentItem;
        private int currentIndex = -1;
        private Album originalCurrentPlaylist;
        private Album currentPlaylist;
        private List<Track> currentItems;
        private bool repeat = false;
        private bool repeatTrack = false;
        
        public event Delegates.EventHandler<Track> OnCurrentItemChanged;

        public List<Track> Items
        {
            get => currentItems;
        }

        public Track CurrentItem
        {
            get => currentItem;
        }

        public bool Repeat
        {
            get => repeat;
            set
            {
                if (repeat == value) return;

                repeat = value;
            }
        }

        public bool RepeatTrack
        {
            get => repeatTrack;
            set
            {
                if (repeatTrack == value) return;

                repeatTrack = value;
            }
        }

        public void SetCurrentTrack(Track track)
        {
            currentItem = track;
            currentIndex = currentItems.IndexOf(track);
        }

        public AudioPlaylist(Album playlist, Track currentPlayAudioFile, bool repeat, bool repeatTrack)
        {
            currentItem = currentPlayAudioFile;
            currentPlaylist = playlist;
            originalCurrentPlaylist = playlist;
            currentItems = playlist.Tracks;
            currentIndex = currentItems.IndexOf(currentPlayAudioFile);
            this.repeat = repeat;
            this.repeatTrack = repeatTrack;
        }

        public void Next(bool skip = false)
        {
            try
            {
                if (Items.Count == 0) return;
                var indexNextTrack = currentIndex + 1;
                if (indexNextTrack >= Items.Count)
                {
                    if (repeat) indexNextTrack = 0;
                    else indexNextTrack = skip ? 0 : -1;
                }

                if (skip) MoveTo(indexNextTrack);
                else
                {
                    if (repeatTrack) OnCurrentItemChanged?.Invoke(this, currentItem);
                    else MoveTo(indexNextTrack);
                }
            }
            catch (Exception e)
            {
                Toast.MakeText(Application.Context, $"Прошизошла ошибка: {e.ToString()}", ToastLength.Long).Show();
            }
            
        }

        public void Back()
        {
            var indexNextTrack = currentIndex - 1;
            if (indexNextTrack < 0) indexNextTrack = Items.Count - 1;
            MoveTo(indexNextTrack);
        }

        private void MoveTo(int index)
        {
            if (index == -1) return;
            if (index == currentIndex) return;
            currentIndex = index;
            currentItem = Items[index];
            OnCurrentItemChanged?.Invoke(this, currentItem);
        }

    }

    
}