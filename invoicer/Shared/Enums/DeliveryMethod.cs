using System.Text.Json.Serialization;

namespace Shared.Enums
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum DeliveryMethod
	{
		PersonalPickUp,
		Courier
	}
}
