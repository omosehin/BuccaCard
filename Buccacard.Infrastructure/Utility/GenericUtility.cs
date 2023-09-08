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
        public static void Forget(this Task task)
        {
            if (!task.IsCompleted || task.IsFaulted) _ = ForgetAwaited(task);
            async static Task ForgetAwaited(Task task) => await task;
        }
    }
}
