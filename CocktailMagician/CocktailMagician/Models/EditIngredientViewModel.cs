using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CocktailMagician.Web.Models
{
    public class EditIngredientViewModel
    {
        public EditIngredientViewModel()
        {

        }
        public int Id { get; set; }

        [Required]
        //[MaxLength(30)]
        public string Name { get; set; }
        public IFormFile File { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageSource { get; set; }
        //public ICollection<int> ContainedCocktails { get; set; }
    }
}
