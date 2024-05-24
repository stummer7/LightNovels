using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Novel : EntityObject
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public IList<Category> Categories {get;set;} = new List<Category>();
        public IList<Chapter> Chapters { get;set;} = new List<Chapter>();
        public int Rank { get; set; }
        public int Views { get; set; }
        public int Bookmarked { get; set; }
        public string ImageURL { get; set; } = string.Empty;
    }


}
