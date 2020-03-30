using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc;

namespace Fooxboy.MusicX.Uwp
{
    public static class Container
    {
        public static IContainer Get { get; private set; }
        public static void SetContainer(IContainer container)
        {
            Get = container;
        }

    }
}
