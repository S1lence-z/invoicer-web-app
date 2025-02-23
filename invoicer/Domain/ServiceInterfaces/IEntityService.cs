using Domain.Interfaces;
using Domain.Models;

namespace Domain.ServiceInterfaces
{
	public interface IEntityService : IService<int, Entity> { }
}
