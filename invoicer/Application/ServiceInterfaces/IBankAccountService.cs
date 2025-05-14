using Shared.DTOs;
using Shared.Interfaces;

namespace Application.ServiceInterfaces
{
	public interface IBankAccountService : IService<int, BankAccountDto> { }
}
