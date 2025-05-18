using Application.Interfaces;
using Domain.Models;

namespace Application.RepositoryInterfaces
{
	public interface IEntityRepository : IRepository
	{
		Task<Entity> GetByIdAsync(int id, bool isReadonly);
		Task<Entity> GetWithSoldInvoicesByIdAsync(int id, bool isReadonly);
		Task<IEnumerable<Entity>> GetAllAsync();
		Task<Entity> CreateAsync(Entity entity);
		Entity Update(Entity entity);
		Task<bool> DeleteAsync(int id);
	}
}
