using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ShowroomManagement.Data
{
    public class DbInitialize
    {
        public static void ShowroomDBInitialize(IServiceProvider serviceProvider) {
            using (var context = new ShowroomContext(serviceProvider
            .GetRequiredService<DbContextOptions<ShowroomContext>>()))
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
