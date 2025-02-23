namespace Domain.Models
{
    public class BankAccount
    {
		public int Id { get; set; }
		public int AccountNumber { get; set; }
		public int BankCode { get; set; }
		public string BankName { get; set; } = string.Empty;
		public string IBAN { get; set; } = string.Empty;
	}
}
