using Application.DTOs;
using Application.Interfaces;

namespace Application.ServiceInterfaces
{
	public interface IAddressService : IService<int, AddressDto> { }
}
