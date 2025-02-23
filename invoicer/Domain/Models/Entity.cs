namespace Domain.Models
{
    public class Entity
    {
		public int Id { get; set; }
		public int Ico { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string PhoneNumber { get; set; } = string.Empty;
		public int BankAccountId { get; set; }
		public BankAccount? BankAccount { get; set; }
		public int AddressId { get; set; }
		public Address? Address { get; set; }
	}
}
