using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TagLib.File;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class FileMp3Abstraction : IFileAbstraction
    {
        public string Name { get; set; }

        public Stream ReadStream { get; set; }

        public Stream WriteStream { get; set; }

        public void CloseStream(Stream stream)
        {
            stream.Dispose();
            stream.Close();
        }
    }
}
