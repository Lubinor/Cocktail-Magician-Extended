using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;

namespace CocktailMagician.Web.Models
{
    public class EditBarCocktailsViewModel
    {
        public EditBarCocktailsViewModel()
        {

        }
        public IEnumerable<int> SelectedCocktails { get; set; }
        public IEnumerable<SelectListItem> Cocktails { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }

        [DisplayName("City")]
        public string CityName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double AverageRating { get; set; }
    }
}