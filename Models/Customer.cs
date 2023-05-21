using Microsoft.AspNetCore.Authorization;

namespace SupermarketWEB.Models
{
    public class Customer
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Identification { get; set; }
		public string Observation { get; set; }
	}
}