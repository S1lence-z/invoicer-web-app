using Shared.DTOs;
using Shared.Interfaces;

namespace Shared.ServiceInterfaces
{
	public interface INumberingSchemeService : IService<int, NumberingSchemeDto>
	{
		Task<NumberingSchemeDto> GetDefaultNumberingSchemeAsync();
	}
}
