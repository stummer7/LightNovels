using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category: EntityObject
    {
        [Display(Name = "Kategorie")]
        [MinLength(5, ErrorMessage = "Der Kategoriename muss mindestens 5 Zeichen lang sein!")]
        [MaxLength(20, ErrorMessage = "Der Kategoriename darf maximal 20 Zeichen lang sein!")]
        public string Name { get; set; } = string.Empty;

        public IList<Movie> Movies { get; set; } = new List<Movie>();
    }
}
