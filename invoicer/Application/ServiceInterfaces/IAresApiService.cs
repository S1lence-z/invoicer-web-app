using Shared.Interfaces;

namespace Application.ServiceInterfaces
{
	public interface IAresApiService
	{
		public Task<IResult<IAresApiResponse>> GetEntityInformationByIcoAsync(string ico);
	}
}
