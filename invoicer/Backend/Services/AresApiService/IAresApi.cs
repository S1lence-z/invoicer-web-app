using Refit;

namespace Backend.Services.AresApiService
{
	public interface IAresApi
	{
		[Get("/ekonomicke-subjekty/{ico}")]
		Task<HttpResponseMessage> GetEntityInformationByIcoAsync(string ico);
	}
}
