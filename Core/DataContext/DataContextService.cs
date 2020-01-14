using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.DataContext
{
    public class DataContextService
    {

        public readonly DbContextOptions<ApplicationContext> Options;

        public DataContextService(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            var options = optionsBuilder
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .UseLazyLoadingProxies()
                .Options;
            Options = options;
        }




    }
}