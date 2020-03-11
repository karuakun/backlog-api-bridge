using System;

namespace BacklogApiBridge.Backlog.Models
{
    public class Space
    {
        public string SpaceKey { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public string Lang { get; set; }
        public string TimeZone { get; set; }
        public string ReportSendTime { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}
