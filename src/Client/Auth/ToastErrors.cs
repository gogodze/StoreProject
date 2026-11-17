using System.Text.Json;
using FluentValidation.Results;

namespace Client.Auth;

internal static class ToastErrors
{
    public static string FromValidation(ValidationResult result, string fallback = "Please fill in the required fields.")
    {
        var messages = result.Errors
            .GroupBy(e => e.PropertyName)
            .Select(g => g.First().ErrorMessage)
            .Where(m => !string.IsNullOrWhiteSpace(m))
            .ToArray();

        return messages.Length switch
        {
            0 => fallback,
            1 => messages[0],
            _ => fallback,
        };
    }

    public static string FromPayload(JsonElement? payload, string fallback = "Request failed")
    {
        if (payload is null)
            return fallback;

        if (payload.Value.TryGetProperty("errors", out var errors)
            && errors.ValueKind == JsonValueKind.Array)
        {
            var messages = errors.EnumerateArray()
                .Select(e => e.GetString())
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .Select(m => m!)
                .Distinct()
                .ToArray();

            return messages.Length switch
            {
                0 => fallback,
                1 => messages[0],
                _ => fallback,
            };
        }

        return fallback;
    }
}
