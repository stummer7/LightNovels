using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Movie : EntityObject
    {
        [Display(Name = "Titel"), Required(ErrorMessage = "Titel muss eingegeben werden!")]
        public string Title { get; set; } = string.Empty;

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }
        [Display(Name = "Kategorie")]
        public int CategoryId { get; set; }

        [Display(Name = "Dauer [min]"), Range(1, 1000, ErrorMessage = "Dauer muss zwischen 1 und 1000 Minuten liegen!")]
        public int Duration { get; set; } //in Minuten

        [Display(Name = "Jahr"), Range(1800, 2100, ErrorMessage = "Das Jahr muss 4 stellig sein!")]
        public int Year { get; set; }
    }
}
