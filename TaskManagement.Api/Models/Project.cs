
using System.Collections.Generic;

namespace TaskManagement.Api.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<TaskModel>? Tasks { get; set; }
    }
}
