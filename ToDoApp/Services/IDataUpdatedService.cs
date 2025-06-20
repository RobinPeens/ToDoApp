
namespace ToDoApp.Services
{
    public interface IDataUpdatedService
    {
        event EventHandler? RefreshPage;

        void OnDataUpdated();
    }
}