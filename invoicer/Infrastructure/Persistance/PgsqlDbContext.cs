using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistance
{
	public class PgsqlDbContext(DbContextOptions<PgsqlDbContext> options)
		: ApplicationDbContext(options)
	{
	}

	public class PgsqlDbContextFactory : IDesignTimeDbContextFactory<PgsqlDbContext>
	{
		public PgsqlDbContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<PgsqlDbContext>();
			optionsBuilder.UseNpgsql("Host=localhost;Database=invoicer;Username=invoicer;Password=changeme");
			return new PgsqlDbContext(optionsBuilder.Options);
		}
	}
}
