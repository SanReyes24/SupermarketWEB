using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupermarketWEB.Data;
using SupermarketWEB.Models;

namespace SupermarketWEB.Pages.Sales
{
	public class IndexModel : PageModel
	{
		private readonly SupermarketContext _context;

		public IndexModel(SupermarketContext context)
		{
			_context = context;
		}

		public IList<Sell?> Sales { get; set; } = default!;

		public async Task OnGetAsync()
		{
			if (_context.Sales != null)
			{
				Sales = await _context.Sales.ToListAsync();
			}

		}
	}
}
