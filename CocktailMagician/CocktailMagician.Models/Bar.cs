using System;
using System.Collections.Generic;

namespace CocktailMagician.Models
{
    public class Bar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double AverageRating { get; set; }
        public ICollection<BarsCocktails> BarCocktails { get; set; } = new HashSet<BarsCocktails>();
        public ICollection<BarsUsersReviews> BarReviews { get; set; } = new HashSet<BarsUsersReviews>();
        public DateTime? CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageSource { get; set; }
    }
}
