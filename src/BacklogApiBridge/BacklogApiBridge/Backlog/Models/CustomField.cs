namespace BacklogApiBridge.Backlog.Models
{
    public class CustomField
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public IssueType[] ApplicableIssueTypes { get; set; }
        public bool AllowAddItem { get; set; }
        public CustomFieldItem[] Items { get; set; }

    }
    public class CustomFieldItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}
