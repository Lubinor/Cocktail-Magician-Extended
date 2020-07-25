using System;
using System.Collections.Generic;
using System.Text;

namespace CocktailMagician.Services.ValidationModels
{
    public class ValidationModel
    {
        public bool HasValidName { get; set; } = true;
        public bool HasProperNameLength { get; set; } = true;
        public bool HasProperInputData { get; set; } = true;
        public bool HasProperPhone { get; set; } = true;
        public bool HasProperAddress { get; set; } = true;
        public bool HasCorrectRating { get; set; } = true;
        public bool HasCorrectCommentLength { get; set; } = true;
    }
}
