using CocktailMagician.Services.Providers.Contracts;
using System;

namespace CocktailMagician.Services.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetDateTime() => DateTime.UtcNow;
    }
}
