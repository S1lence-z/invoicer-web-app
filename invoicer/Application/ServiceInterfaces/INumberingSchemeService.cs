using Shared.DTOs;
using Shared.Interfaces;

namespace Application.ServiceInterfaces
{
	public interface INumberingSchemeService : IService<int, NumberingSchemeDto>
	{
		Task<NumberingSchemeDto> GetDefaultNumberingSchemeAsync();
	}
}
