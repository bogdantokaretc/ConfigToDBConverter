using ConfigToDBConverter.Models;
using Microsoft.EntityFrameworkCore;
namespace ConfigToDBConverter.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<ConfigDataModel> ConfigData { get; set; }
    }
}
