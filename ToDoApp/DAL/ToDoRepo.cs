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
        /// Get ToDo
        /// </summary>
        /// <param name="toDoId"></param>
        /// <returns>Gounf ToDo</returns>
        /// <exception cref="Exception">Thrown if not found</exception>
        public async Task<ToDo> GetToDo(int toDoId)
        {
            using var db = contextProvider.CreateDbContext();
            var toDo = await db.ToDos.FindAsync(toDoId);
            return toDo ??
                throw new Exception($"Failed to get ToDo with ToDoId ${toDoId}");
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

            includeDeleted = includeDeleted || status == StatusType.Deleted;

            query = db.ToDos.Where(a =>
                (status == null || a.StatusId == (int)status) &&
                (includeDeleted || a.StatusId != (int)StatusType.Deleted)
            );

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
        /// <returns>Updated ToDo</returns>
        /// <exception cref="Exception">Throws Exception if no database changes occured</exception>
        public async Task<ToDo> AddNew(
            string title,
            string? description,
            DateTime dueDate)
        {
            using var db = contextProvider.CreateDbContext();

            var toDo = new ToDo
            {
                Title = title,
                Description = description,
                DueDate = dueDate,
                StatusId = (int)StatusType.Pending
            };

            await db.ToDos.AddAsync(toDo);
            var result = await db.SaveChangesAsync();

            if (toDo.ToDoId <= 0 || result == 0)
                throw new Exception("Failed to add new ToDo to database.");

            return toDo;
        }

        /// <summary>
        /// Set ToDo To Completed.
        /// </summary>
        /// <param name="toDoId">ToDo Id</param>
        /// <param name="notes" required>Required Notes field</param>
        /// <returns>Updated ToDo</returns>
        /// <exception cref="Exception">Throws Exception if no database changes occured</exception>
        public async Task<ToDo> SetCompleted(
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

            return toDo;
        }

        /// <summary>
        /// Set ToDo Viewed DateTime, Multi user use case only I guess.
        /// </summary>
        /// <param name="toDoId">ToDo Id</param>
        /// <returns>Updated ToDo</returns>
        /// <exception cref="Exception">Throws Exception if no database changes occured</exception>
        public async Task<ToDo> SetViewed(int toDoId)
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

            return toDo;
        }

        /// <summary>
        /// Delete a ToDo by setting Status to Deleted.
        /// </summary>
        /// <param name="toDoId">ToDo Id</param>
        /// <returns>Updated ToDo</returns>
        /// <exception cref="Exception">Throws Exception if no database changes occured</exception>
        public async Task<ToDo> SetDelete(
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

            return toDo;
        }

        /// <summary>
        /// Cancel a ToDo by setting Status to Canceled.
        /// </summary>
        /// <param name="toDoId">ToDo Id</param>
        /// <returns>Cancel ToDo</returns>
        /// <exception cref="Exception">Throws Exception if no database changes occured</exception>
        public async Task<ToDo> SetCanceled(
            int toDoId)
        {
            using var db = contextProvider.CreateDbContext();

            var toDo = await db.ToDos.FindAsync(toDoId);
            if (toDo == null)
                throw new Exception($"ToDo Not found for ToDoId ${toDoId}");

            toDo.StatusId = (int)StatusType.Canceled;
            toDo.UpdatedDate = DateTime.Now;

            var result = await db.SaveChangesAsync();

            if (result == 0)
                throw new Exception($"Failed to cancel ToDo for ToDoId ${toDoId}");

            return toDo;
        }

        /// <summary>
        /// Process Overdue ToDo's
        /// </summary>
        /// <returns>Update Count</returns>
        public async Task<int> ProcessOverdue()
        {
            using var db = contextProvider.CreateDbContext();

            var toDos = db.ToDos
                .Where(a => a.StatusId == (int)StatusType.Pending && a.DueDate <= DateTime.Now)
                .AsAsyncEnumerable();

            await foreach (var toDo in toDos)
            {
                toDo.StatusId = (int)StatusType.Overdue;
                toDo.UpdatedDate = DateTime.Now;
            }

            var result = await db.SaveChangesAsync();
            return result;
        }
    }
}
