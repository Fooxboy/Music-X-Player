using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class PlayerViewModel : BaseViewModel
    {
        //Синглтон 
        private static PlayerViewModel instanse;
        public static PlayerViewModel Instanse
        {
            get
            {
                if (instanse != null) return instanse;
                instanse = new PlayerViewModel();
                return instanse;
            }
        }

        /// <summary>
        /// Приватный конструкор
        /// </summary>
        private PlayerViewModel()
        {
            
        }

        //Поле в котором хранится имя исполнителя, так и все другие поля оформляются.
        private string artist;
        private string trackname;
        private string cover;
        public string Artist
        {
            get => artist;
            set
            {
                if (artist == value) return;
                artist = value;
                Changed("Artist");
            }
        }

        public string TrackName
        {
            get => trackname;
            set
            {
                if (trackname == value) return;
                trackname = value;
                Changed("TrackName");
            }
        }

        public string Cover
        {
            get => cover;
            set
            {
                if (cover == value) return;
                cover = value;
                Changed("Cover");
            }
        }

    }
}
