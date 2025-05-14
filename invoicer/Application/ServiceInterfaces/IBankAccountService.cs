using Application.DTOs;
using Application.Interfaces;

namespace Application.ServiceInterfaces
{
	public interface IBankAccountService : IService<int, BankAccountDto> { }
}
