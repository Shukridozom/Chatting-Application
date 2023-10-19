using ChattingApplication.Dtos;
using System.ComponentModel.DataAnnotations;

namespace ChattingApplication.CustomDataAnnotations
{
    public class LastNameCharacterSet : ValidationAttribute
    {
        string lastNameCharacterSet = "abcdefghijklmnopqrstuvwxyzABCEDFGHIJKLMNOPQRSTUVWXYZ0123456789_'-";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var registerDto = ((RegisterDto)validationContext.ObjectInstance);
            var lastName = registerDto.LastName;

            if (lastName.Length < 6)
                return new ValidationResult("Lastname must be at least 6 characters long");

            foreach (var character in lastName)
            {
                if (!lastNameCharacterSet.Contains(character))
                    return new ValidationResult("Allowed characters: [a...z][A...Z][0...9] and these symbols: _'- ");
            }

            return ValidationResult.Success;
        }
    }
}
