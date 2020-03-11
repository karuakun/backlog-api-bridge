using Newtonsoft.Json;

namespace BacklogApiBridge.Backlog.Models
{
    public class Change
    {
        public string Field { get; set; }
        [JsonProperty("new_value")]
        public string NewValue { get; set; }
        [JsonProperty("old_value")]
        public string OldValue { get; set; }
        public string Type { get; set; }
    }
}