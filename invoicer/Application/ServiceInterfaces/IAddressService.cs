using Domain.Interfaces;
using Application.DTOs;

namespace Application.ServiceInterfaces
{
	public interface IAddressService : IService<int, AddressDto> { }
}
