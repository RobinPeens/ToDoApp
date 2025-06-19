using AutoMapper;
using System.Net;
using ToDoApp.DAL;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public interface IToDoService
    {
        Task<List<ToDoModel>> GetFiltered(StatusType? status = null, bool includeDeleted = false);
    }

    public class ToDoService : IToDoService
    {
        private readonly IMapper mapper;
        private readonly IToDoRepo toDoRepo;

        public ToDoService(
            IMapper mapper,
            IToDoRepo toDoRepo)
        {
            this.mapper = mapper;
            this.toDoRepo = toDoRepo;
        }

        public async Task<List<ToDoModel>> GetFiltered(
            StatusType? status = null,
            bool includeDeleted = false)
        {
            var results = await toDoRepo.GetFiltered(status, includeDeleted);
            return mapper.Map<List<ToDoModel>>(results);
        }
    }
}
