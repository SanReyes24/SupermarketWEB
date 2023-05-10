using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupermarketWEB.Data;
using SupermarketWEB.Models;

namespace SupermarketWEB.Pages.Sales
{
	public class DeleteModel : PageModel
	{
		private readonly SupermarketContext _context;

		public DeleteModel(SupermarketContext context)
		{
			_context = context;
		}

		[BindProperty]

		public Sell Sell { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null || _context.Sales == null)
			{
				return NotFound();
			}

			var sell = await _context.Sales.FirstOrDefaultAsync(m => m.Id == id);

			if (sell == null)
			{
				return NotFound();
			}
			else
			{
				Sell = sell;
			}
			return Page();
		}
		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null || _context.Sales == null)
			{
				return NotFound();
			}
			var sell = await _context.Sales.FindAsync(id);

			if (sell != null)
			{
				Sell = sell;
				_context.Sales.Remove(Sell);
				await _context.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}
