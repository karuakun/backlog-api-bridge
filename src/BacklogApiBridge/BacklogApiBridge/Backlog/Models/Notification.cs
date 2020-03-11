namespace BacklogApiBridge.Backlog.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public bool AlreadyRead { get; set; }
        public int Reason { get; set; }
        public User User { get; set; }
        public bool ResourceAlreadyRead { get; set; }
    }
}