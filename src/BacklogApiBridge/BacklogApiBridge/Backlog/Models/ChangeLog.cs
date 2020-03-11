namespace BacklogApiBridge.Backlog.Models
{
    public class ChangeLog
    {
        public string Field { get; set; }
        public string NewValue { get; set; }
        public string OriginalValue { get; set; }
    }
}