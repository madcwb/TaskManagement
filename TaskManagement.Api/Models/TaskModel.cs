
using System;
using System.Collections.Generic;

namespace TaskManagement.Api.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public int ProjectId { get; set; }

        public ICollection<TaskHistory>? History { get; set; }

        public void Update(TaskModel updatedTask)
        {
            Status = updatedTask.Status;
            Description = updatedTask.Description;
            DueDate = updatedTask.DueDate;
        }
    }
}
