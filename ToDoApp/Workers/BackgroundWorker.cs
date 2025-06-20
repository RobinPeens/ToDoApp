
using System.Threading;
using ToDoApp.DAL;
using ToDoApp.Services;

namespace ToDoApp.Workers
{
    public class BackgroundWorker : BackgroundService, IDisposable
    {
        private readonly ILogger logger;
        private readonly IDataUpdatedService dataUpdatedService;
        private readonly IToDoRepo toDoRepo;

        public BackgroundWorker(
            ILogger<BackgroundWorker> logger,
            IDataUpdatedService dataUpdatedService,
            IToDoRepo toDoRepo)
        {
            this.logger = logger;
            this.dataUpdatedService = dataUpdatedService;
            this.toDoRepo = toDoRepo;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _ = Worker(stoppingToken);
            return Task.CompletedTask;
        }

        private async Task Worker(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(60 * 1000); // Wait 1 min between polls. THIS REALLY SHOULD BE IN APPSETTNGS....

                try
                {
                    var updates = await toDoRepo.ProcessOverdue();

                    if (updates > 0)
                        dataUpdatedService.OnDataUpdated();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to process overdue ToDo's.");
                }
            }
        }
    }
}
