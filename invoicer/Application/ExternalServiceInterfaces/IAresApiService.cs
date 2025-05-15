using Application.AresApi;
using Application.Interfaces;

namespace Application.ExternalServiceInterfaces
{
	public interface IAresApiService
	{
		public Task<IResult<IAresApiResponse>> GetEntityInformationByIcoAsync(string ico);
	}
}
