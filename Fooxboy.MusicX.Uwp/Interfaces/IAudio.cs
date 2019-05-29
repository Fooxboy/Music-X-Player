using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.Interfaces
{
    public interface IAudio
    {

        /// <summary>
        /// Обложка
        /// </summary>
        string Cover { get; set; }

        /// <summary>
        /// id
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Владелец id (для локальных файлов - 0)
        /// </summary>
        string OwnerId { get; set; }

        /// <summary>
        /// Internal id 
        /// </summary>
        string InternalId { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        string Artist { get; set; }

        /// <summary>
        /// Playlist id
        /// </summary>
        long PlaylistId { get; set; }

        /// <summary>
        /// Продолжительность
        /// </summary>
        TimeSpan Duration { get; set; }

        /// <summary>
        /// Source
        /// </summary>
        Uri Source { get; set; }
    }
}
