using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BacklogApiBridge.Backlog.Models
{
    public class PullRequest
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int RepositoryId { get; set; }
        public int Number { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Base { get; set; }
        public string Branch { get; set; }
        public Status Status { get; set; }
        public Assignee Assignee { get; set; }
        public Issue Issue { get; set; }
        public object BaseCommit { get; set; }
        public object BranchCommit { get; set; }
        public DateTimeOffset? CloseAt { get; set; }
        public DateTimeOffset? MergeAt { get; set; }
        public User CreatedUser { get; set; }
        public DateTimeOffset Created { get; set; }
        public User UpdatedUser { get; set; }
        public DateTimeOffset Updated { get; set; }
        public List<Attachment> Attachments { get; set; }
        public List<Star> Stars { get; set; }
    }
}
