using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BacklogApiBridge.Backlog.Models;

namespace BacklogApiBridge.CustomModel
{
    public class StatusChange
    {
        public int IssueId { get; set; }
        public string NewValue { get; set; }
        public string OriginalValue { get; set; }
        public string CurrentStatus { get; set; }
        public User UpdateUser { get; set; }
        public DateTimeOffset Updated { get; set; }
        public Comment Comment { get; set; }
    }
}
