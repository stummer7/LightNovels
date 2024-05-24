using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Chapter: EntityObject
    {
        public string Title { get; set; } = string.Empty;
        public DateTime Released { get; set; }
        public string Text { get; set; } = string.Empty;
        public Novel Novel { get; set; } = new();
        public int NovelId { get; set; }

    }
}
