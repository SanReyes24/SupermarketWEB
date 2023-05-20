using Microsoft.AspNetCore.Authorization;

namespace SupermarketWEB.Models
{
    [Authorize]
    public class Provider
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string? Description { get; set; }
	}
}
