namespace Domain.Models
{
    public record class BankAccount
    {
		public int Id { get; set; }
		public string AccountNumber { get; set; } = string.Empty;
		public string BankCode { get; set; } = string.Empty;
		public string BankName { get; set; } = string.Empty;
		public string IBAN { get; set; } = string.Empty;
	}
}
