using Microsoft.AspNetCore.Components;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Components
{
    public partial class CreateToDo
    {
        [Inject]
        protected IToDoService toDoService { get; set; } = null!;

        [SupplyParameterFromForm]
        public ToDoViewModel Model { get; set; } = null!;

        [Parameter]
        public EventCallback OnSubmit { get; set; }

        protected override void OnInitialized() => Model ??= new();

        public async Task Submit()
        {
            var result = await toDoService.AddNew(Model.Title, Model.Description, Model.DueDate);
            Model = new();
            await OnSubmit.InvokeAsync();
        }

        public async Task Cancel()
        {
            Model = new();
            await OnSubmit.InvokeAsync();
        }
    }
}
