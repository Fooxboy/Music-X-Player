//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Fooxboy.MusicX.Uwp.Models;
//using Fooxboy.MusicX.Uwp.Services.VKontakte;

//namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
//{
//    public class NewRecommendationsViewModel:BaseViewModel
//    {
//        public List<Block> Blocks { get; set; }
//        public bool IsLoading { get; set; }

//        public async Task StartLoading()
//        {
//            IsLoading = true;
//            Changed("IsLoading");

//            var recc = await Fooxboy.MusicX.Core.VKontakte.Music.Recommendations.New();
//            var blocks = recc.Blocks;
//            var listBlocks = new List<Block>();
            
//            foreach (var blockA in blocks)
//            {
//                var block = BlockService.ConvertToBlock(blockA);
//                if (block.Tracks != null)
//                {
//                    var tracks = await MusicService.ConvertToAudioFile(block.Tracks);
//                     block.TrackFiles = tracks;
//                }

//                if (block.Playlists != null)
//                {
//                    var playlists = new List<PlaylistFile>();
//                    foreach (var blockPlaylist in block.Playlists)
//                    {
//                        var plist = await PlaylistsService.ConvertToPlaylistFile(blockPlaylist);
//                        playlists.Add(plist);
//                    }

//                    block.PlaylistsFiles = playlists;
//                }
//                listBlocks.Add(block);
//            }

//            Blocks = listBlocks;
//            Changed("Blocks");
//            IsLoading = false;
//            Changed("IsLoading");
//        }
//    }
//}
