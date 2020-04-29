using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.Core.Models;
using Fooxboy.MusicX.Reimagined.Adapters;
using Fooxboy.MusicX.Reimagined.Models;
using Fooxboy.MusicX.Reimagined.Services;

namespace Fooxboy.MusicX.Reimagined.Fragments
{
    public class MainFragment : Fragment
    {

        bool isLoading = true;
        HomeAdapter Adapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.frag_unified_blocks, container, false);

            var list_blocks = view.FindViewById<RecyclerView>(Resource.Id.unifrag_blocks);
            var loading = view.FindViewById<ProgressBar>(Resource.Id.unifrag_loading);

            List<MainBlock> Blocks = new List<MainBlock>();
            Blocks.Add(new MainBlock()
            {
                Title = "Music X Beta",
                Description = "Вы используете beta-версию Music X! Это значит, что приложение может быть нестабильным, а также не иметь части функциональности. Обновления Вы можете найти в нашем Телеграм-канале https://t.me/MusicXPlayer"
            });
            Blocks.Add(new MainBlock()
            {
                Blocks = RecommendationsService.Get(),
                Title = "Ваши Рекомендации"
            }); ;
            //Blocks.AddRange(RecommendationsService.Get());
            Adapter = new HomeAdapter(Blocks);
            list_blocks.SetAdapter(Adapter);
            list_blocks.SetLayoutManager(new LinearLayoutManager(Android.App.Application.Context, LinearLayoutManager.Vertical, false));
            return view;
        }
    }
}