using Application.DTOs;
using Application.Interfaces;

namespace Application.ServiceInterfaces
{
	public interface INumberingSchemeService : IService<int, NumberingSchemeDto>
	{
		Task<NumberingSchemeDto> GetDefaultNumberingSchemeAsync();
	}
}
