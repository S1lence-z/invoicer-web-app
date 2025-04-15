using Application.DTOs;
using Domain.Interfaces;

namespace Application.ServiceInterfaces
{
	public interface IBankAccountService : IService<int, BankAccountDto> { }
}
