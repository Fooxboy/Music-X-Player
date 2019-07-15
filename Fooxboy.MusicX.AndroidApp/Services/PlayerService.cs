using Fooxboy.MusicX.AndroidApp.Models;

namespace Fooxboy.MusicX.AndroidApp.Services
{
    public class PlayerService
    {
        public PlayingService MainService;
        private static PlayingService inst;
        
        
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Cover { get; set; }

        
        public static PlayingService Instanse => inst ?? (inst = new PlayingService());


        private PlayerService()
        {
            MainService = new PlayingService();
            MainService.CurrentAudioChanged += MainServiceOnCurrentAudioChanged;
        }

        private void MainServiceOnCurrentAudioChanged(object sender, AudioFile args)
        {
            Title = args.Title;
            Artist = args.Artist;
            Cover = args.Cover;
        }
    }
}