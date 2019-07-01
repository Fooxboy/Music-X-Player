using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
{
    public class SearchViewModel:BaseViewModel
    {

        private static SearchViewModel instanse;

        public static SearchViewModel Instanse
        {
            get
            {
                if (instanse == null) instanse = new SearchViewModel();

                return instanse;
            }
        }

        public static void Reset()
        {
            instanse = null;
        }

        public LoadingCollection<AudioFile> Music { get; set; }

        public AudioFile SelectTrack { get; set; }

        private string request;
        public string Request
        {
            get => request;
            set
            {
                if (request == value) return;
                request = value;
                HeaderText = $"Результаты поиска для {request}";
                Changed("HeaderText");
            }
        }

        public Visibility IsLoading { get; set; } 

        public string HeaderText { get; set; }

        public void MusicListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //TODO: проигрование трека ебаный врот
        }

        public void MusicListView_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
