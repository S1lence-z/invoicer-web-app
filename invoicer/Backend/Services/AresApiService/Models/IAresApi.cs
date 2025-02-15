using Refit;

namespace Backend.Services.AresApiService.Models
{
	public interface IAresApi
	{
		[Get("/ekonomicke-subjekty/{ico}")]
		Task<SubjectInformation> GetEntityInformationByIcoAsync(string ico);
	}
}
