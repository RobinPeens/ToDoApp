using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Components
{
    public partial class ViewToDo
    {

        [SupplyParameterFromForm]
        public ToDoModel Model { get; set; } = null!;

        public bool IsValid { get; set; } = true;

        [Parameter]
        public int ToDoId { get; set; } = 0;

        [Parameter]
        public EventCallback OnSubmit { get; set; }

        [Inject]
        protected IToDoService toDoService { get; set; } = null!;

        [Inject]
        private IJSRuntime jsRuntime { get; set; } = null!;

        protected override void OnInitialized() => Model ??= new();

        protected override async Task OnInitializedAsync()
        {
            if (ToDoId != 0)
                Model = await toDoService.GetToDo(ToDoId);
        }

        public async Task Submit()
        {
            if ((Model?.Notes?.Length ?? 0) == 0)
            {
                IsValid = false;
            }
            else
            {
                var result = await toDoService.Completed(ToDoId, Model!.Notes!);
                Model = new();
                await OnSubmit.InvokeAsync();
            }
        }
        public async Task Cancel()
        {
            bool confirmed = await jsRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to cancel ToDo:"); // Prompt
            if (confirmed)
            {
                var result = await toDoService.SetCancel(ToDoId);
                Model = new();
                await OnSubmit.InvokeAsync();
            }
        }

        public async Task Back()
        {
            Model = new();
            await OnSubmit.InvokeAsync();
        }
    }
}
