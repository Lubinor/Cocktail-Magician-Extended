using CocktailMagician.Web.Utilities;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CocktailMagician.Web.Models
{
    public class CityViewModel
    {
        public CityViewModel()
        {

        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string OldName { get; set; }
        public ICollection<BarViewModel> Bars { get; set; } = new List<BarViewModel>();
        //[DisplayName("Bars")]
        //public string BarNames { get; set; }
    }
}
