using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation
{
    public static class CommonValidationRules
    {
        private static readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private static readonly int _maxFileSizeInMb = 5;
        private static readonly int _maxFileSizeInBytes = _maxFileSizeInMb * 1024 * 1024; // 5 Mb

        public static IRuleBuilderOptions<T, IFormFile?> ImageFileRules<T>(
            this IRuleBuilder<T, IFormFile?> rule)
        {
            return rule
                .Must(f => f is null || f!.Length <= _maxFileSizeInBytes)
                .WithMessage($"The max file size is {_maxFileSizeInMb} Mb")

                .Must(f => f is null || _allowedExtensions.Contains(
                    Path.GetExtension(f!.FileName).ToLowerInvariant()))
                .WithMessage($"Allowed image types are: {string.Join(", ", _allowedExtensions)}.");
        }
    }
}
