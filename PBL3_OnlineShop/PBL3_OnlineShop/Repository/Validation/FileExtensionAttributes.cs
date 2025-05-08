using System.ComponentModel.DataAnnotations;

namespace PBL3_OnlineShop.Repository.Validation
{
    public class FileExtensionAttributes : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            if (value is IEnumerable<IFormFile> files)
            {
                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                    {
                        return new ValidationResult($"Invalid file type '{extension}'. Allowed types: {string.Join(", ", allowedExtensions)}");
                    }
                }
            }
            else if (value is IFormFile singleFile)
            {
                var extension = Path.GetExtension(singleFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    return new ValidationResult($"Invalid file type '{extension}'. Allowed types: {string.Join(", ", allowedExtensions)}");
                }
            }

            return ValidationResult.Success!;
        }

    }
}
