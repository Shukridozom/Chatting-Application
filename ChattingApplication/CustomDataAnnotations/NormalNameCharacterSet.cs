using System.ComponentModel.DataAnnotations;

namespace ChattingApplication.CustomDataAnnotations
{
    public class NormalNameCharacterSet : ValidationAttribute
    {
        string nameCharacterSet = "abcdefghijklmnopqrstuvwxyzABCEDFGHIJKLMNOPQRSTUVWXYZ0123456789_'-";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var name = (string)validationContext.ObjectInstance;

            if (name.Length < 6)
                return new ValidationResult("Name must be at least 6 characters long");

            foreach (var character in name)
            {
                if (!nameCharacterSet.Contains(character))
                    return new ValidationResult("Allowed characters: [a...z][A...Z][0...9] and these symbols: _'- ");
            }
            return ValidationResult.Success;
        }
    }
}
