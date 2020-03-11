namespace BacklogApiBridge.Backlog.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string ProjectKey { get; set; }
        public string Name { get; set; }
        public bool ChartEnabled { get; set; }
        public bool SubtaskingEnabled { get; set; }
        public bool ProjectLeaderCanEditProjectLeader { get; set; }
        public string TextFormattingRule { get; set; }
        public bool Archived { get; set; }
    }
}
