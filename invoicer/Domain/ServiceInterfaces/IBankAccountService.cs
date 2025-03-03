using Domain.Interfaces;
using Domain.Models;

namespace Domain.ServiceInterfaces
{
	public interface IBankAccountService : IService<int, BankAccount> { }
}
