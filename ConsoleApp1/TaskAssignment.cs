using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class TaskAssignment
    {
        public string AssignmentId { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }

        // Foreign keys
        public string UserId { get; set; } = string.Empty;
        public string TaskId { get; set; } = string.Empty;
        public string StatusId { get; set; } = string.Empty;
    }

}
