using Shared.DTOs;
using Shared.Interfaces;

namespace Shared.ServiceInterfaces
{
	public interface IBankAccountService : IService<int, BankAccountDto> { }
}
