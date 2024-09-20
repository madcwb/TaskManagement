
using System;

namespace TaskManagement.Api.Models
{
    public class TaskHistory
    {
        public int Id { get; set; }
        public string Update { get; set; }
        public DateTime ModificationDate { get; set; }
        public string User { get; set; }
        public int TaskId { get; set; }       
        public TaskModel? Task { get; set; }
    }
}
