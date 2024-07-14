using Microsoft.EntityFrameworkCore;

namespace StocksApplication.Models
{
	public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext(DbContextOptions dbContextOptions) 
            : base(dbContextOptions)
        {
            if (!Database.EnsureCreated())
            {
                Console.WriteLine("Problems with database");
            }
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
