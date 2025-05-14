using Shared.DTOs;
using Shared.Interfaces;

namespace Application.ServiceInterfaces
{
	public interface IEntityService : IService<int, EntityDto> { }
}
