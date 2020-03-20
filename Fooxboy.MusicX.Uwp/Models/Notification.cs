using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class Notification
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool HasButtons { get; set; }
        public string ButtonOneText { get; set; }
        public string ButtonTwoText { get; set; }
        public ICommand ButtonOneCommand { get; set; }
        public ICommand ButtonTwoCommand { get; set; }
        public bool IsClickButton { get; set; }
    }
}
