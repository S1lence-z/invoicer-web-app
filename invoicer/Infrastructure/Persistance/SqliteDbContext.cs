using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistance
{
	public class SqliteDbContext(DbContextOptions<SqliteDbContext> options)
		: ApplicationDbContext(options)
	{
	}

	public class SqliteDbContextFactory : IDesignTimeDbContextFactory<SqliteDbContext>
	{
		public SqliteDbContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<SqliteDbContext>();
			optionsBuilder.UseSqlite("Data Source=../Infrastructure/Persistance/Invoicer.db");
			return new SqliteDbContext(optionsBuilder.Options);
		}
	}
}
