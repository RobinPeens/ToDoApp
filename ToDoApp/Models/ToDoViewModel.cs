
namespace ToDoApp.Models
{
    public class ToDoViewModel
    {
        public StatusType Status { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime DueDate { get; set; } = DateTime.Now;
    }
}