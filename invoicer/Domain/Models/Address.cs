using Domain.Interfaces;

namespace Domain.Models
{
    public class Address : IModel
    {
        public int Id { get; set; }
		public string Street { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public int ZipCode { get; set; }
		public string Country { get; set; } = string.Empty;
	}
}
