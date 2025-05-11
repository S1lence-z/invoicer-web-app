using Shared.Interfaces;

namespace Shared.ServiceInterfaces
{
	public interface IAresApiService
	{
		public Task<IResult<IAresApiResponse>> GetEntityInformationByIcoAsync(string ico);
	}
}
