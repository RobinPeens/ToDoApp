using Microsoft.AspNetCore.Components;
using ToDoApp.Data;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Pages
{
    public partial class FetchData
    {
        private List<ToDoModel>? toDos;

        [Inject]
        protected IToDoService toDoService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            toDos = await toDoService.GetFiltered();
        }
    }
}
