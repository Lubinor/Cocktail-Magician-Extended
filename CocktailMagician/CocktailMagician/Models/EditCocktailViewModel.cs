using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CocktailMagician.Web.Models
{
    public class EditCocktailViewModel
    {
        public EditCocktailViewModel()
        {

        }
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public ICollection<int> ContainedIngredients { get; set; }
        public ICollection<int> ContainedBars { get; set; }
        public IFormFile File { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageSource { get; set; }
    }
}
