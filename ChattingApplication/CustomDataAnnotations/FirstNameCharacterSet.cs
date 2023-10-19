using ChattingApplication.Dtos;
using System.ComponentModel.DataAnnotations;

namespace ChattingApplication.CustomDataAnnotations
{
    public class FirstNameCharacterSet : ValidationAttribute
    {
        string firstNameCharacterSet = "abcdefghijklmnopqrstuvwxyzABCEDFGHIJKLMNOPQRSTUVWXYZ0123456789_'-";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var registerDto = ((RegisterDto)validationContext.ObjectInstance);
            var firstName = registerDto.FirstName;

            if (firstName.Length < 6)
                return new ValidationResult("Firstname must be at least 6 characters long");

            foreach (var character in firstName)
            {
                if (!firstNameCharacterSet.Contains(character))
                    return new ValidationResult("Allowed characters: [a...z][A...Z][0...9] and these symbols: _'- ");
            }

            return ValidationResult.Success;
        }
    }
}
