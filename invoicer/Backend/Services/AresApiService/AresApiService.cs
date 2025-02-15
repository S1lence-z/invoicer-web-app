using Backend.Services.AresApiService.Models;
using Refit;

namespace Backend.Services.AresApiService
{
	public class AresApiService
	{
		private readonly IAresApi _aresApi;
		private const string baseUrl = "https://ares.gov.cz/ekonomicke-subjekty-v-be/rest";

		public AresApiService()
		{
			_aresApi = RestService.For<IAresApi>(baseUrl);
		}

		public Task<SubjectInformation> GetEntityInformationByIcoAsync(string ico) => _aresApi.GetEntityInformationByIcoAsync(ico);
	}
}
