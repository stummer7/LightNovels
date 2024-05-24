using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category: EntityObject
    {
        [Display(Name = "Kategorie")]
        public string Name { get; set; } = string.Empty;

        public IList<Novel> Novels { get; set; } = new List<Novel>();
    }
}
