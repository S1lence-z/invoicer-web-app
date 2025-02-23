using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class InvoiceItem
    {
		public int Id { get; set; }
		public int InvoiceId { get; set; }
		public Invoice Invoice { get; set; } = null!;
		public string Unit { get; set; } = string.Empty;
		public decimal Quantity { get; set; }
		public string Description { get; set; } = string.Empty;
		public decimal UnitPrice { get; set; }
	}
}
