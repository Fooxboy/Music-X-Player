using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.Core.Models;
using Fooxboy.MusicX.Reimagined.Converters;

namespace Fooxboy.MusicX.Reimagined.Services
{
    public static class RecommendationsService
    {

        public static List<Block> Get()
        {
            if(!isLoaded())
            {
                var blocks = Core.Api.GetApi().VKontakte.Music.Recommendations.Get().ToBlocksList();
                return blocks;
            }
            else
            {
                return StaticContentService.Recommendations;
            }
        }

        public static void Refresh()
        {
            StaticContentService.Recommendations = Core.Api.GetApi().VKontakte.Music.Recommendations.Get().ToBlocksList();
        }

        public static void Clear()
        {
            StaticContentService.Recommendations.Clear();
        }

        public static bool isLoaded()
        {
            return StaticContentService.Recommendations.Count > 0 ? true : false;
        }


    }
}