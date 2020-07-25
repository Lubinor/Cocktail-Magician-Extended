using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CocktailMagician.Web.Models
{
    public class IngredientViewModel
    {
        public IngredientViewModel()
        {

        }
        public int Id { get; set; }
        [Required]
        //[MaxLength(30)]
        public string Name { get; set; }

        [DisplayName("Cocktails")]
        public string CocktailNames { get; set; }
        public ICollection<CocktailViewModel> Cocktails { get; set; } = new List<CocktailViewModel>();
        public IFormFile File { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageSource { get; set; }
    }
}
