using Shared.Interfaces;

namespace Shared.AresApiModels
{
	public class AresApiErrorResponse : IAresApiResponse
	{
		public string Kod { get; set; } = string.Empty;
		public string Popis { get; set; } = string.Empty;
		public string? SubKod { get; set; }
	}
}
