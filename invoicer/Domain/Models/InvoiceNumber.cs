using Domain.Interfaces;
using Domain.Utils;

namespace Domain.Models
{
	public record class InvoiceNumber
	{
		public string Value { get; init; }
		public string Prefix { get; private init; }
		public string SequenceNumber { get; private init; }
		public string? Year { get; private init; }
		public string? Month { get; private init; }
		public string? Seperator { get; private init; }
		protected NumberingScheme UsedNuberingScheme { get; init; }

		private InvoiceNumber(string value, NumberingScheme scheme, ParsedInvoiceNumberComponents components)
		{
			Value = value;
			Prefix = components.Prefix;
			SequenceNumber = components.SequenceNumber;
			Year = components.Year;
			Month = components.Month;
			Seperator = components.Seperator;
			UsedNuberingScheme = scheme;
		}

		public int GetSequenceNumberAsInt()
		{
			if (string.IsNullOrEmpty(SequenceNumber))
				throw new InvalidOperationException("The sequence number is not available.");
			if (!int.TryParse(SequenceNumber, out int sequenceNumber))
				throw new InvalidOperationException($"The sequence number '{SequenceNumber}' is not a valid integer.");
			return sequenceNumber;
		}

		public override string ToString() => Value;
		public static implicit operator string(InvoiceNumber invoiceNumber) => invoiceNumber.Value;

		public static InvoiceNumber FromString(string potentialInvoiceNumber, NumberingScheme scheme, IInvoiceNumberParser parser)
		{
			if (!parser.TryParse(potentialInvoiceNumber, scheme, out ParsedInvoiceNumberComponents components, out string? errorMessage))
				throw new ArgumentException($"Invalid invoice number string '{potentialInvoiceNumber}' for scheme with id '{scheme.Id}': {errorMessage}");
			return new InvoiceNumber(potentialInvoiceNumber, scheme, components);
		}
	}
}
