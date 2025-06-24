namespace Domain.Utils
{
	public record struct ParsedInvoiceNumberComponents
	{
		public string Prefix { get; set; }
		public string SequenceNumber { get; set; }
		public string Year { get; set; }
		public string Month { get; set; }
		public string Seperator { get; set; }
	}
}
