using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class PlaylistViewNavigationData
    {
        public Album Album { get; set; }
        public IContainer Container { get; set; }
    }
}
