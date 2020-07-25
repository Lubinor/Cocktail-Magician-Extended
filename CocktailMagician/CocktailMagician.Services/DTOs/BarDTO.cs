using System;
using System.Collections.Generic;

namespace CocktailMagician.Services.DTOs
{
    [Serializable]
    public class BarDTO
    {
        public BarDTO()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double AverageRating { get; set; }
        public ICollection<CocktailDTO> Cocktails { get; set; } = new HashSet<CocktailDTO>();
        public byte[] ImageData { get; set; }
        public string ImageSource { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<BarReviewDTO> Reviews { get; set; } = new HashSet<BarReviewDTO>();
    }
}
