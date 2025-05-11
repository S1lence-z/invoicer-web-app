using System.Text.Json.Serialization;

namespace Shared.Enums
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum InvoiceStatus
	{
		Pending,
		Paid,
		Overdue,
		Cancelled,
		Draft
	}
}
