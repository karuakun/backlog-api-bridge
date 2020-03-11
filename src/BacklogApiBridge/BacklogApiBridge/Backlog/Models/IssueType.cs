namespace BacklogApiBridge.Backlog.Models
{
    public class IssueType
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int DisplayOrder { get; set; }
    }
}