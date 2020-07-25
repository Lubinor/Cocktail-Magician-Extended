using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CocktailMagician.Models
{
    public class User : IdentityUser<int>
    {
        public ICollection<CocktailsUsersReviews> CocktailReviews { get; set; } = new HashSet<CocktailsUsersReviews>();
        public ICollection<BarsUsersReviews> BarReviews { get; set; } = new HashSet<BarsUsersReviews>();
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
    }
}
