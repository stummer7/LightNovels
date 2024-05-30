using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User : EntityObject
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string ProfilePicture { get; set; } = string.Empty;
        public DateTime Registered { get; set; }
        public DateTime LastActivity { get; set; }
        public string Description { get; set; } = string.Empty;

        public IList<Novel> Bookmarks { get; set; } = new List<Novel>();
        public IList<Novel> History { get; set; } = new List<Novel>();
    }
}
