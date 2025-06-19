using Microsoft.EntityFrameworkCore;
using ToDoApp.DataContext;

namespace ToDoApp.DAL
{
    public class DbContextProvider : IDbContextFactory<ToDoListContext>
    {
        private readonly IServiceProvider serviceProvider;

        public DbContextProvider (IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Get New (Transient) instance of dbContext for EFCore
        /// </summary>
        /// <returns>Instance of EFCore ToDoListContext</returns>
        /// <exception cref="Exception">Failed to create instance from DI</exception>
        public ToDoListContext CreateDbContext()
        {
            return serviceProvider.GetService<ToDoListContext>()
                ?? throw new Exception("Failed to get DB Context");
        }   
    }
}
