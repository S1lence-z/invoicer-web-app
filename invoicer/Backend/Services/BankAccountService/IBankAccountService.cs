using Backend.Models;
using Backend.Services.BankAccountService.Models;

namespace Backend.Services.BankAccountService
{
	public interface IBankAccountService : IService<int, BankAccount> { }
}
