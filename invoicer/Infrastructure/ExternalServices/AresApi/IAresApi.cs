using Refit;

namespace Infrastructure.ExternalServices.AresApi
{
	public interface IAresApi
	{
		[Get("/ekonomicke-subjekty/{ico}")]
		Task<HttpResponseMessage> GetEntityInformationByIcoAsync(string ico);
	}
}
