using System.Text;
using System.Text.RegularExpressions;
using NewsPortal.Application.Interfaces;

namespace NewsPortal.Infrastructure.Services;

public sealed class SlugService : ISlugService
{
    public string Generate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        value = value.Trim().ToLowerInvariant();

        value = ReplacePersianCharacters(value);

        value = Regex.Replace(
            value,
            @"[^\p{L}\p{Nd}\s-]",
            string.Empty);

        value = Regex.Replace(
            value,
            @"[\s_-]+",
            "-");

        value = value.Trim('-');

        return value;
    }

    private static string ReplacePersianCharacters(string value)
    {
        return value
            .Replace('ي', 'ی')
            .Replace('ى', 'ی')
            .Replace('ك', 'ک')
            .Replace('ۀ', 'ه')
            .Replace('ة', 'ه');
    }
    public Task<string> GenerateUniqueAsync(string text)
    {
        var slug = text
            .Trim()
            .ToLowerInvariant()
            .Replace(" ", "_");
        return Task.FromResult(slug);

    }
}
