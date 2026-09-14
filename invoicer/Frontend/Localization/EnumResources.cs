namespace Frontend.Localization
{
	/// <summary>
	/// Marker type for the shared enum display names in Resources/Localization/EnumResources*.resx.
	/// Keys are "&lt;EnumTypeName&gt;.&lt;Value&gt;", e.g. "PaymentMethod.BankTransfer". Resolve them with
	/// <see cref="Extensions.EnumLocalizerExtensions.Localize{TEnum}"/>.
	/// </summary>
	public sealed class EnumResources
	{
	}
}
