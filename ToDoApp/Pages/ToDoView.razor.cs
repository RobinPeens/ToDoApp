using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ToDoApp.Components;
using ToDoApp.DataContext;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Pages
{
    public partial class ToDoView
    {
        public List<ToDoModel>? toDos { get; private set; }
        public StatusType? FilterStatus { get; private set; } = null;
        public bool IncludeDeleted { get; private set; } = false;
        public bool ShowViewToDo { get; private set; } = false;
        public bool ShowAddToDo { get; private set; } = false;
        public int ViewToDoId { get; private set; } = 0;

        [Inject]
        protected IToDoService toDoService { get; set; } = null!;

        [Inject]
        private IJSRuntime jsRuntime { get; set; } = null!;

        [Inject]
        private IDataUpdatedService dataUpdatedService { get; set; } = null!;

        protected override void OnInitialized()
        {
            dataUpdatedService.RefreshPage += OnServiceEvent;
        }

        public void Dispose()
        {
            dataUpdatedService.RefreshPage -= OnServiceEvent;
        }

        protected override async Task OnInitializedAsync()
        {
            await Refresh();
        }

        public string GetRowClass(ToDoModel toDo)
        {
            return toDo.Status switch
            {
                StatusType.Deleted => "todo_deleted",
                StatusType.Canceled => "todo_canceled",
                StatusType.Overdue => "todo_overdue",
                StatusType.Completed => "todo_complete",
                _ => "todo",
            };
        }

        public async Task CancelToDo(int toDoId)
        {

            bool confirmed = await jsRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to cancel ToDo:"); // Prompt
            if (confirmed)
            {
                var item = await toDoService.SetCancel(toDoId);

                if (item == null)
                {
                    await jsRuntime.InvokeVoidAsync("alert", "Failed to cancel ToDo.");
                    return;
                }

                await jsRuntime.InvokeVoidAsync("alert", "ToDo Canceled!");
                await Refresh();
            }
        }

        public async Task DeleteToDo(int toDoId)
        {
            bool confirmed = await jsRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete ToDo:"); // Prompt
            if (confirmed)
            {
                var item = await toDoService.SetDelete(toDoId);

                if (item == null)
                {
                    await jsRuntime.InvokeVoidAsync("alert", "Failed to delete ToDo.");
                    return;
                }

                await jsRuntime.InvokeVoidAsync("alert", "ToDo Deleted!");
                await Refresh();
            }
        }

        public void ViewToDo(int toDoId)
        {
            ViewToDoId = toDoId;
            ShowViewToDo = true;
            ShowAddToDo = false;
        }

        public void AddToDo()
        {
            ShowViewToDo = false;
            ShowAddToDo = true;
        }

        public async Task Refresh()
        {
            ShowAddToDo = false;
            ShowViewToDo = false;
            toDos = await toDoService.GetFiltered(FilterStatus, IncludeDeleted);
        }

        private void OnServiceEvent(object? sender, EventArgs e)
        {
            InvokeAsync(async () =>
            {
                await Refresh();
                StateHasChanged();
            });
        }
    }
}
