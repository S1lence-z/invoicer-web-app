using Application.DTOs;
using Domain.Interfaces;

namespace Application.ServiceInterfaces
{
	public interface IEntityService : IService<int, EntityDto> { }
}
