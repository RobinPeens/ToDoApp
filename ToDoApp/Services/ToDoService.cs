using AutoMapper;
using System.Net;
using ToDoApp.DAL;
using ToDoApp.DataContext;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class ToDoService : IToDoService
    {
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly IToDoRepo toDoRepo;

        public ToDoService(
            ILogger<ToDoService> logger,
            IMapper mapper,
            IToDoRepo toDoRepo)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.toDoRepo = toDoRepo;
        }

        public async Task<ToDoModel> GetToDo(int toDoId)
        {
            try
            {
                var result = await toDoRepo.GetToDo(toDoId);
                return mapper.Map<ToDoModel>(result);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Failed to get ToDo.");
                return null;
            }
        }

        public async Task<List<ToDoModel>> GetFiltered(
            StatusType? status = null,
            bool includeDeleted = false)
        {
            try
            {
                var results = await toDoRepo.GetFiltered(status, includeDeleted);
                return mapper.Map<List<ToDoModel>>(results);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Failed to get list of ToDo's.");
                return null;
            }
        }

        public async Task<ToDoModel> AddNew(string title, string? description, DateTime dueDate)
        {
            try
            {
                var results = await toDoRepo.AddNew(title, description, dueDate);
                return mapper.Map<ToDoModel>(results);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Failed to add new ToDo.");
                return null;
            }
        }

        public async Task<ToDoModel> Completed(int toDoId, string notes)
        {
            try
            {
                var results = await toDoRepo.SetCompleted(toDoId, notes);
                return mapper.Map<ToDoModel>(results);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Failed to complete ToDo.");
                return null;
            }
        }

        public async Task<ToDoModel> SetDelete(int toDoId)
        {
            try
            {
                var results = await toDoRepo.SetDelete(toDoId);
                return mapper.Map<ToDoModel>(results);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Failed to delete ToDo.");
                return null;
            }
        }

        public async Task<ToDoModel> SetCancel(int toDoId)
        {
            try
            {
                var results = await toDoRepo.SetCanceled(toDoId);
                return mapper.Map<ToDoModel>(results);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Failed to delete ToDo.");
                return null;
            }
        }

        public async Task<ToDoModel> SetViewed(int toDoId)
        {
            try
            {
                var results = await toDoRepo.SetViewed(toDoId);
                return mapper.Map<ToDoModel>(results);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Failed to set ToDo to views state.");
                return null;
            }
        }
    }
}
