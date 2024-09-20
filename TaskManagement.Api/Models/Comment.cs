
using System;

namespace TaskManagement.Api.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CommentDate { get; set; }
        public int TaskId { get; set; }
        public TaskModel Task { get; set; }
    }
}
