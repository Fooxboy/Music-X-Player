using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class SearchViewModel:BaseViewModel
    {

        public bool VisibleLoading { get; set; }
        public bool VisibleContent { get; set; }

        public ObservableCollection<IBlock> Blocks { get; set; }

        private Api _api;
        public SearchViewModel(Api api)
        {
            _api = api;
            Blocks = new ObservableCollection<IBlock>();
            VisibleLoading = true;
            VisibleContent = false;
        }

        public async Task StartLoading(string query)
        {
            var results = await _api.VKontakte.Music.Search.GetResultsAsync(query);

            var emptyBlock = results.Single(b => b.Source == "search_suggestions");

            results.Remove(emptyBlock);

            VisibleContent = true;
            VisibleLoading = false;
            Changed("VisibleContent");
            Changed("VisibleLoading");

            foreach (var result in results)
            {
                Blocks.Add(result);
            }
        }

    }
}
