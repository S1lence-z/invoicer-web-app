using System.Text.Json.Serialization;

namespace Domain.Enums
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum Currency
	{
		CZK,
		EUR,
		USD
	}
}
