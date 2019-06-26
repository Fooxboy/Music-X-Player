using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core.Interfaces
{
    public interface IUserInfo
    {
        long Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string PhotoUser { get; set; }
    }
}
