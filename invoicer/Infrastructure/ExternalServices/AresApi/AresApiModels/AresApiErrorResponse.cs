using Application.DTOs.AresApi;

namespace Infrastructure.ExternalServices.AresApi.AresApiModels
{
	public class AresApiErrorResponse : IAresApiResponse
	{
		public string Kod { get; set; } = string.Empty;
		public string Popis { get; set; } = string.Empty;
		public string? SubKod { get; set; }
	}
}
