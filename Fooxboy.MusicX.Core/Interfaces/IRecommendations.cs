using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core.Interfaces
{
    public interface IRecommendations
    {
        List<IBlock> Blocks { get; set; }
    }
}
