using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CocktailMagician.Web.Models
{
    public class CreateCocktailViewModel
    {
        public CreateCocktailViewModel()
        {

        }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public ICollection<int> ContainedIngredients { get; set; }
        public IFormFile File { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageSource { get; set; }
        public int CreatorId { get; set; }
    }
}
