using System;

namespace BacklogApiBridge.Backlog.Models
{
    public class BacklogVersion
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? ReleaseDueDate { get; set; }
        public bool Archived { get; set; }
        public int DisplayOrder { get; set; }
    }
}
