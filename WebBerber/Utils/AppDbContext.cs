using Microsoft.EntityFrameworkCore;


namespace WebBerber.Utils
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
    }
}
