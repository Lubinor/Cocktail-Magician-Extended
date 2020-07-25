using System;
using System.Collections.Generic;

namespace CocktailMagician.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Bar> Bars { get; set; } = new HashSet<Bar>();
        public DateTime? CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
