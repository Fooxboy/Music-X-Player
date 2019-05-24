using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Services;
using Windows.UI.Xaml.Controls;

namespace Fooxboy.MusicX.Uwp
{
    public static class StaticContent
    {
        public static NavigationService NavigationContentService { get; set; }
        public static Frame PlayerMenuFrame { get; set; }
    }
}
