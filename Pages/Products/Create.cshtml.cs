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
	public class CreateModel : PageModel
	{
		private readonly SupermarketContext _context;

		public CreateModel(SupermarketContext context)
		{
			_context = context;
		}

		public List<SelectListItem> Categories { get; set; }

		public void OnGet()
		{
			Categories = _context.Categories
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList();
		}

		[BindProperty]
		public Product Product { get; set; }

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				// Si hay errores de validación, establece la lista desplegable de categorías y vuelve a la página
				ViewData["Categories"] = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
				return Page();
			}

			// Comprueba que se han rellenado todos los campos necesarios
			if (string.IsNullOrEmpty(Product.Name) || Product.Price <= 0 || Product.Stock < 0 || Product.CategoryId <= 0)
			{
				ModelState.AddModelError("", "Please fill in all required fields.");
				ViewData["Categories"] = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
				return Page();
			}

			// Recupera la categoría seleccionada
			var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == Product.CategoryId);

			if (category == null)
			{
				// Si la categoría no existe, establece la lista desplegable de categorías y muestra un mensaje de error
				ModelState.AddModelError("", "Invalid category selected.");
				ViewData["Categories"] = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
				return Page();
			}

			// Añade el nuevo producto a la base de datos y guarda los cambios
			_context.Products.Add(Product);
			await _context.SaveChangesAsync();

			// Establece la lista desplegable de categorías y vuelve a la página Index
			ViewData["Categories"] = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
			return RedirectToPage("./Index");
		}

	}
}

