using Application.AresApi;
using Application.Interfaces;

namespace Application.ExternalServiceInterfaces
{
	public interface IAresApiService
	{
		public Task<IApiResult<IAresApiResponse>> GetEntityInformationByIcoAsync(string ico);
	}
}
