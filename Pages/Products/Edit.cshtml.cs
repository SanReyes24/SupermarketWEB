using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupermarketWEB.Data;
using SupermarketWEB.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupermarketWEB.Pages.Products
{
	public class EditModel : PageModel
	{
		private readonly SupermarketContext _context;

		public EditModel(SupermarketContext context)
		{
			_context = context;
		}

		public List<SelectListItem> Categories { get; set; }

		[BindProperty]
		public Product Product { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Product = await _context.Products.FindAsync(id);

			if (Product == null)
			{
				return NotFound();
			}

			Categories = await _context.Categories.Select(c =>
				new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name,
					Selected = (c.Id == Product.CategoryId)
				}).ToListAsync();

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				Categories = await _context.Categories.Select(c =>
					new SelectListItem
					{
						Value = c.Id.ToString(),
						Text = c.Name,
						Selected = (c.Id == Product.CategoryId)
					}).ToListAsync();
				return Page();
			}

			var category = await _context.Categories.FindAsync(Product.CategoryId);

			if (category == null)
			{
				ModelState.AddModelError("", "Invalid category selected.");
				Categories = await _context.Categories.Select(c =>
					new SelectListItem
					{
						Value = c.Id.ToString(),
						Text = c.Name,
						Selected = (c.Id == Product.CategoryId)
					}).ToListAsync();
				return Page();
			}

			_context.Attach(Product).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProductExists(Product.Id))
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

		private bool ProductExists(int id)
		{
			return _context.Products.Any(p => p.Id == id);
		}
	}
}
