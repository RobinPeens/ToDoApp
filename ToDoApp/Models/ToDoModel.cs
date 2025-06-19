
namespace ToDoApp.Models
{
    public class ToDoModel
    {
        public int ToDoId { get; set; }

        public StatusType Status { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string? Notes { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}