using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermarketWEB.Models
{
    [Authorize]
    public class Sell
	{
		public int Id { get; set; }
		public string Date { get; set; }

		public string? CustomerId { get; set; } = default!;
		public string? ProductName { get; set; } = default!;
		public int? Quantity { get; set; } = default!;

		//public decimal? ProductPrice { get; set; } = default!;
		[Column(TypeName = "decimal(15,2)")]
		public decimal? ProductPrice { get; set; } = default!;

		[Column(TypeName = "decimal(15,2)")]
		public decimal? TotalSale { get; set; } = default!;
		public string? PayModeName { get; set; } = default!;
		public string? Observation { get; set; } = default!;
	}
}
