using ToDoApp.Models;

namespace ToDoApp.Services
{
    public interface IToDoService
    {
        Task<ToDoModel> AddNew(string title, string? description, DateTime dueDate);
        Task<List<ToDoModel>> GetFiltered(StatusType? status = null, bool includeDeleted = false);
        Task<ToDoModel> Completed(int toDoId, string notes);
        Task<ToDoModel> SetDelete(int toDoId);
        Task<ToDoModel> SetViewed(int toDoId);
        Task<ToDoModel> SetCancel(int toDoId);
        Task<ToDoModel> GetToDo(int toDoId);
    }
}
