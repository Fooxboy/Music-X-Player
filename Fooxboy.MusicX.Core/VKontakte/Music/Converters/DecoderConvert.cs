using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fooxboy.MusicX.Core.VKontakte.Music.Converters
{
    public static class DecoderConvert
    {
        public static Uri DecodeAudioUrl(this Uri audioUrl)
        {
            try
            {
                var segments = audioUrl.Segments.ToList();

                segments.RemoveAt((segments.Count - 1) / 2);
                segments.RemoveAt(segments.Count - 1);

                segments[segments.Count - 1] = segments[segments.Count - 1].Replace("/", ".mp3");

                return new Uri($"{audioUrl.Scheme}://{audioUrl.Host}{string.Join("", segments)}{audioUrl.Query}");
            }
            catch
            {
                return null;
            }
           
        }
    }
}
