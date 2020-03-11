using System;

namespace BacklogApiBridge.Backlog.Models
{
    public class Webhook
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HookUrl { get; set; }
        public bool AllEvent { get; set; }
        public int[] ActiveTypeIds { get; set; }
        public User CreateUser { get; set; }
        public DateTimeOffset Created { get; set; }
        public User Updated { get; set; }
    }
}
