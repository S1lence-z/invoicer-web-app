using Backend.Models;
using Backend.Services.AddressService.Models;

namespace Backend.Services.AddressService
{
	public interface IAddressService : IService<int, Address> { }
}
