using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Web.Utilities
{
    public static class ToastrConsts
    {
        public const string NullModel = "You are not submitting any data!";
        public const string WrongNameLength = "Name cannot have less than 2 and more than 30 symbols!";
        public const string NameNotValid = "Name cannot contain numbers or special characters!";
        public const string NotUnique = "Item already exists!";
        public const string GenericError = "Something went wrong, please try again!";
        public const string NoPicture = "Please upload a picture!";
        public const string NotFound = "Item not found!";
        public const string Success = "Operation successful!";
        public const string IncorrectInput = "Incorrect input, please try again!";
        public const string IncorrectAddress = "Address must be between 5 and 100 characters!";
        public const string IncorrectPhone = "Phone must contain only numbers and can be between 7 and 20 digits long!";
        public const string CommentTooLong = "Comment cannot exceed 500 characters!";
        public const string IncorrectRating = "Rating must be between 1 and 5!";
        public const string ReviewExists = "You can have only one comment per bar/cocktail!";
    }
}
