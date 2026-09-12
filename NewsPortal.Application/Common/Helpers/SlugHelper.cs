using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace NewsPortal.Application.Common.Helpers;

public static class SlugHelper
{
    public static string GenerateSlug(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        text = text.Trim().ToLowerInvariant();
        text = RemoveDiacritics(text);
        text = Regex.Replace(text, @"[^a-z0-9\u0600-\u06FF\s-]", "");
        text = Regex.Replace(text, @"[\s_]+", "-");
        text = Regex.Replace(text, @"-{2,}", "-");

        return text.Trim('-');
    }

    private static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
