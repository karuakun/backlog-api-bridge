using System;
using Newtonsoft.Json;

namespace BacklogApiBridge.Backlog.Models
{
    public class Content
    {
        public int Id { get; set; }
        [JsonProperty("key_id")]
        public int KeyId { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public Comment Comment { get; set; }
        public Change[] Changes { get; set; }
        public Notification Notifications { get; set; }
        public User CreatedUser { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
