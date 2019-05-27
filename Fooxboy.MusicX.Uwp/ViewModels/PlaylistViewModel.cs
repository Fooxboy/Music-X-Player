using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fooxboy.MusicX.Uwp.Models;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class PlaylistViewModel : BaseViewModel
    {
        public PlaylistViewModel()
        {
        }

        private static PlaylistViewModel instanse;
        public static PlaylistViewModel Instanse
        {
            get
            {
                if (instanse != null) return instanse;
                instanse = new PlaylistViewModel();
                return instanse;
            }
        }

        private ObservableCollection<AudioFile> music;
        public ObservableCollection<AudioFile> Music
        {
            get
            {
                return music;
            }
            set
            {
                if (value != music)
                {
                    music = value;
                    Changed("Music");
                }
            }
        }

        private string playlistname;
        public string PlaylistName
        {
            get
            {
                return playlistname;
            }
            set
            {
                if (value != playlistname)
                {
                    playlistname = value;
                    Changed("PlaylistName");
                }
            }
        }


        private int pltrackcount;
        public int PLTrackCount
        {
            get
            {
                return pltrackcount;
            }
            set
            {
                if (value != pltrackcount)
                {
                    pltrackcount = value;
                    Changed("PLTrackCount");
                }
            }
        }

    }
}
