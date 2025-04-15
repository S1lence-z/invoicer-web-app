using System.Text.Json.Serialization;

namespace Domain.Enums
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum InvoiceNumberYearFormat
	{
		FourDigit,
		TwoDigit,
		None
	}
}
