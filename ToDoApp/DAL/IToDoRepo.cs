using ToDoApp.DataContext;
using ToDoApp.Models;

namespace ToDoApp.DAL
{
    public interface IToDoRepo
    {
        Task<ToDo> AddNew(string title, string? description, DateTime dueDate);
        Task<List<ToDo>> GetFiltered(StatusType? status = null, bool includeDeleted = false);
        Task<ToDo> GetToDo(int toDoId);
        Task<int> ProcessOverdue();
        Task<ToDo> SetCanceled(int toDoId);
        Task<ToDo> SetCompleted(int toDoId, string notes);
        Task<ToDo> SetDelete(int toDoId);
        Task<ToDo> SetViewed(int toDoId);
    }
}
