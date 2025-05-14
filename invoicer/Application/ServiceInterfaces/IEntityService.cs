using Application.DTOs;
using Application.Interfaces;

namespace Application.ServiceInterfaces
{
	public interface IEntityService : IService<int, EntityDto> { }
}
