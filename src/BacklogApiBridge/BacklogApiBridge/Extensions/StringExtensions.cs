using Newtonsoft.Json;

namespace BacklogApiBridge.Extensions
{
    public static class StringExtensions
    {
        public static T FromJson<T>(this string source, JsonSerializerSettings settings = null)
        {
            if (string.IsNullOrEmpty(source))
                return default;
            var localSettings = settings ?? new JsonSerializerSettings();
            return JsonConvert.DeserializeObject<T>(source, localSettings);
        }

        public static int[] FromJsonToIntArray(this string source, JsonSerializerSettings settings = null)
        {
            return string.IsNullOrEmpty(source) 
                ? new int[] {} 
                : source.FromJson<int[]>(settings);
        }
    }
}