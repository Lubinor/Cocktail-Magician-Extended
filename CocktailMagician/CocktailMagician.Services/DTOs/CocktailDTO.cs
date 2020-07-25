using System.Collections.Generic;

namespace CocktailMagician.Services.DTOs
{
    public class CocktailDTO
    {
        public CocktailDTO()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public double AverageRating { get; set; }
        public ICollection<IngredientDTO> Ingredients { get; set; } = new List<IngredientDTO>();
        public ICollection<BarDTO> Bars { get; set; } = new List<BarDTO>();
        //public DateTime? CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageSource { get; set; }
        public int CreatorId { get; set; }
        public string CreatorName { get; set; }

        //public override bool Equals(object obj)
        //{
        //    var other = (CityDTO)obj;
        //    return this.Name == other.Name; // more props with &&
        //}

    }
}
