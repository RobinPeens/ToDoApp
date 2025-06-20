namespace ToDoApp.Services
{
    public class DataUpdatedService : IDataUpdatedService
    {
        public event EventHandler? RefreshPage;

        public void OnDataUpdated()
        {
            RefreshPage?.Invoke(this, null);
        }
    }
}
