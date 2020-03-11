using System;

namespace BacklogApiBridge.Backlog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public ChangeLog[] ChangeLog { get; set; }
        public Notification[] Notifications { get; set; }
        public Star[] Stars { get; set; }
        public DateTimeOffset Created { get; set; }
        public User CreatedUser { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}