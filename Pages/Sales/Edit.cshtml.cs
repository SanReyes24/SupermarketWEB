using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupermarketWEB.Data;
using SupermarketWEB.Models;

namespace SupermarketWEB.Pages.Sales
{
	public class EditModel : PageModel
	{
		private readonly SupermarketContext _context;

		public EditModel(SupermarketContext context)
		{
			_context = context;
		}
		public List<SelectListItem> Customer { get; set; }
		public List<SelectListItem> Product2 { get; set; }
		public List<SelectListItem> Product3 { get; set; }
		//public List<Product> Product3 { get; set; }
		public List<SelectListItem> PayMode { get; set; }
		[BindProperty]

		public Sell Sell { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			Customer = _context.Customers
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList();

			Product2 = _context.Products
			.Select(c => new SelectListItem
			{
				Value = c.Id.ToString(),
				Text = c.Name

			}).ToList();

			Product3 = _context.Products
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Price.ToString()

				}).ToList();


			PayMode = _context.PayModes
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList();


			if (id == null || _context.Sales == null)
			{
				return NotFound();
			}

			var sell = await _context.Sales.FirstOrDefaultAsync(m => m.Id == id);

			if (sell == null)
			{
				return NotFound();
			}
			Sell = sell;
			return Page();

			Customer = _context.Customers
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList();

			Product2 = _context.Products
			.Select(c => new SelectListItem
			{
				Value = c.Id.ToString(),
				Text = c.Name

			}).ToList();

			Product3 = _context.Products
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Price.ToString()

				}).ToList();


			PayMode = _context.PayModes
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			_context.Attach(Sell).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CustomerExists(Sell.Id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return RedirectToPage("./Index");
		}

		private bool CustomerExists(int id)
		{
			return (_context.Sales?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
