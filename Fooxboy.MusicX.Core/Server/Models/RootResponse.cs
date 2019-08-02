using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core.Server.Models
{
    public class RootResponse<T>
    {
        public bool Status { get; set; }
        public T Result { get; set; }
        public Error Error { get; set; }
    }
}
