using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using ToDoApp.DataContext;
using ToDoApp.Models;

namespace ToDoApp.DAL
{
    public class ToDoRepo : IToDoRepo
    {
        private readonly IDbContextFactory<ToDoListContext> contextProvider;

        public ToDoRepo(IDbContextFactory<ToDoListContext> contextProvider)
        {
            this.contextProvider = contextProvider;
        }

        /// <summary>
        /// Get Filtered List of ToDo's
        /// </summary>
        /// <param name="status">NULL for all</param>
        /// <param name="includeDeleted">Include Deleted if Selecting all Statuses</param>
        /// <returns>List of all ToDo's that match the Status Filter.</returns>
        public async Task<List<ToDo>> GetFiltered(
            StatusType? status = null,
            bool includeDeleted = false)
        {
            using var db = contextProvider.CreateDbContext();
            var query = db.ToDos.AsQueryable();

            if (status != null)
                query = db.ToDos.Where(a => a.StatusId == (int)status);

            if (!includeDeleted)
                query = db.ToDos.Where(a => a.StatusId != (int)StatusType.Deleted);

            query = query
                .OrderBy(a => a.Title)
                .ThenBy(a => a.StatusId);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Add New ToDo
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="description">Description</param>
        /// <param name="dueDate">Due Date for the ToDo Item</param>
        /// <returns>Nothing on success, Exception on fail</returns>
        /// <exception cref="Exception">Throws Exception if no database changes occured</exception>
        public async Task AddNew(
            string title,
            string description,
            DateTime dueDate)
        {
            using var db = contextProvider.CreateDbContext();

            var newTodo = new ToDo
            {
                Title = title,
                Description = description,
                DueDate = dueDate,
                StatusId = (int)StatusType.Pending
            };

            await db.ToDos.AddAsync(newTodo);
            var result = await db.SaveChangesAsync();

            if (newTodo.ToDoId <= 0 || result == 0)
                throw new Exception("Failed to add new ToDo to database.");
        }

        /// <summary>
        /// Set ToDo To Completed.
        /// </summary>
        /// <param name="toDoId">ToDo Id</param>
        /// <param name="notes" required>Required Notes field</param>
        /// <returns>Nothing on success, Exception on fail</returns>
        /// <exception cref="Exception">Throws Exception if no database changes occured</exception>
        public async Task SetCompleted(
            int toDoId,
            string notes)
        {
            using var db = contextProvider.CreateDbContext();

            var toDo = await db.ToDos.FindAsync(toDoId);
            if (toDo == null)
                throw new Exception($"ToDo Not found for ToDoId ${toDoId}");

            toDo.StatusId = (int)StatusType.Completed;
            toDo.Notes = notes;
            toDo.UpdatedDate = DateTime.Now;
            toDo.CompletedDate = DateTime.Now;

            var result = await db.SaveChangesAsync();

            if (result == 0)
                throw new Exception($"Failed to complete ToDo status for ToDoId ${toDoId}");
        }

        /// <summary>
        /// Set ToDo Viewed DateTime, Multi user use case only I guess.
        /// </summary>
        /// <param name="toDoId">ToDo Id</param>
        /// <returns>Nothing on success, Exception on fail</returns>
        /// <exception cref="Exception">Throws Exception if no database changes occured</exception>
        public async Task SetViewed(int toDoId)
        {
            using var db = contextProvider.CreateDbContext();

            var toDo = await db.ToDos.FindAsync(toDoId);
            if (toDo == null)
                throw new Exception($"ToDo Not found for ToDoId ${toDoId}");

            toDo.UpdatedDate = DateTime.Now;
            toDo.ViewedDate = DateTime.Now;

            var result = await db.SaveChangesAsync();

            if (result == 0)
                throw new Exception($"Failed to set viewed ToDo status for ToDoId ${toDoId}");
        }

        /// <summary>
        /// Delete a ToDo by setting Status to Deleted.
        /// </summary>
        /// <param name="toDoId">ToDo Id</param>
        /// <returns>Nothing on success, Exception on fail</returns>
        /// <exception cref="Exception">Throws Exception if no database changes occured</exception>
        public async Task SetDelete(
            int toDoId)
        {
            using var db = contextProvider.CreateDbContext();

            var toDo = await db.ToDos.FindAsync(toDoId);
            if (toDo == null)
                throw new Exception($"ToDo Not found for ToDoId ${toDoId}");

            toDo.StatusId = (int)StatusType.Deleted;
            toDo.UpdatedDate = DateTime.Now;

            var result = await db.SaveChangesAsync();

            if (result == 0)
                throw new Exception($"Failed to delete ToDo status for ToDoId ${toDoId}");
        }
    }
}
