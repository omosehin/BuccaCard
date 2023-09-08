using System.Text.Json;

namespace Buccacard.Infrastructure.Utility
{
    public static class GenericUtility
    {
        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreReadOnlyProperties = false
        };
        public static string ToJson(this object input) => JsonSerializer.Serialize(input, jsonOptions);
        public static T FromJson<T>(this string input) => JsonSerializer.Deserialize<T>(input, jsonOptions);

    }
}
