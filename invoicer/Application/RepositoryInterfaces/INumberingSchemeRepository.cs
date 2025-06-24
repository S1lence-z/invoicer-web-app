using Application.Interfaces;
using Domain.Models;

namespace Application.RepositoryInterfaces
{
	public interface INumberingSchemeRepository : IRepository
	{
		Task<NumberingScheme> GetByIdAsync(int id, bool isReadonly);
		Task<IEnumerable<NumberingScheme>> GetAllAsync();
		Task<NumberingScheme> CreateAsync(NumberingScheme numberingScheme);
		NumberingScheme Update(NumberingScheme numberingScheme);
		Task<bool> DeleteAsync(int id);
		Task<NumberingScheme> GetDefaultSchemeAsync(bool isReadonly);
		Task SetDefaultSchemeAsync(NumberingScheme numberingScheme);
		Task<bool> IsInUseByEntity(NumberingScheme numberingScheme);
	}
}
