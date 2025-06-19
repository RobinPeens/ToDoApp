using ToDoApp.DataContext;
using ToDoApp.Models;

namespace ToDoApp.DAL
{
    public interface IToDoRepo
    {
        Task AddNew(string title, string description, DateTime dueDate);
        Task<List<ToDo>> GetFiltered(StatusType? status = null, bool includeDeleted = false);
        Task SetCompleted(int toDoId, string notes);
        Task SetDelete(int toDoId);
        Task SetViewed(int toDoId);
    }
}
