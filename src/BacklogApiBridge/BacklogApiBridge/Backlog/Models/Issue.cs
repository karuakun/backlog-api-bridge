using System;
using System.Collections.Generic;

namespace BacklogApiBridge.Backlog.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string IssueKey { get; set; }
        public int KeyId { get; set; }
        public IssueType IssueType { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
//        public object Resolutions { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public Assignee Assignee { get; set; }
        public List<Category> Categories { get; set; }
        //        public List<object> versions { get; set; }
        public List<Milestone> Milestone { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public double? EstimatedHours { get; set; }
        public double? ActualHours { get; set; }
        public int? ParentIssueId { get; set; }
        public User CreatedUser { get; set; }
        public DateTime Created { get; set; }
        public User UpdatedUser { get; set; }
        public DateTime Updated { get; set; }
//        public List<object> customFields { get; set; }
        public List<Attachment> Attachments { get; set; }
        public List<SharedFile> SharedFiles { get; set; }
        public List<Star> Stars { get; set; }

    }
    public class Priority
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Status
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class Assignee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoleType { get; set; }
        public object Lang { get; set; }
        public string MailAddress { get; set; }
    }

    public class Milestone
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
    public class Attachment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
    }
    public class SharedFile
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Dir { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public User CreatedUser { get; set; }
        public DateTime Created { get; set; }
        public User UpdatedUser { get; set; }
        public DateTime Updated { get; set; }
    }

    public class Star
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public User Presenter { get; set; }
        public DateTime Created { get; set; }
    }
}
