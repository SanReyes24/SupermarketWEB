using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SupermarketWEB.Data;
using SupermarketWEB.Models;
using System.Globalization;

namespace SupermarketWEB.Pages.Sales
{
	public class CreateModel : PageModel
	{
		private readonly SupermarketContext _context;

		public CreateModel(SupermarketContext context)
		{
			_context = context;
		}

		public List<SelectListItem> Customer { get; set; }
		public List<SelectListItem> Product2 { get; set; }
		public List<SelectListItem> Product3 { get; set; }
		//public List<Product> Product3 { get; set; }
		public List<SelectListItem> PayMode { get; set; }
		public void OnGet()
		{
			Sell = new Sell();

			Customer = _context.Customers
	.Where(c => !string.IsNullOrEmpty(c.Name))
	.Select(c => new SelectListItem
	{
		Value = c.Name, // Asignar el nombre en lugar del ID
		Text = c.Name
	})
	.ToList();

			Product2 = _context.Products
			.Where(p => !string.IsNullOrEmpty(p.Name))
			.Select(p => new SelectListItem
			{
				Value = p.Name, // Asignar el nombre en lugar del ID
				Text = $"{p.Name} - {p.Price}" // Mostrar el nombre y el precio del producto
			})
			.ToList();


			PayMode = _context.PayModes
				.Where(pm => !string.IsNullOrEmpty(pm.Name))
				.Select(pm => new SelectListItem
				{
					Value = pm.Name, // Asignar el nombre en lugar del ID
					Text = pm.Name
				})
				.ToList();


			Sell.Date = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

		}

		[BindProperty]
		public Sell Sell { get; set; }

		//public Customer Customer2 { get; set; }
		//public Product Product { get; set; }
		//public PayMode PayMode2 { get; set; }
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				// Si hay errores de validación, establece la lista desplegable de categorías y vuelve a la página
				ViewData["Customers"] = new SelectList(await _context.Customers.ToListAsync(), "Name");
				ViewData["Products"] = new SelectList(await _context.Products.ToListAsync(), "Name", "Price");
				ViewData["PayModes"] = new SelectList(await _context.Categories.ToListAsync(), "Name");
				return Page();
			}

			// Recupera la categoría seleccionada
			var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Name == Sell.CustomerId);
			var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == Sell.ProductName);
			var payModeName = await _context.PayModes.FirstOrDefaultAsync(pm => pm.Name == Sell.PayModeName);

			if (customer == null || product == null || payModeName == null)
			{
				// Si algún valor seleccionado no existe, establece las listas desplegables y muestra un mensaje de error
				ModelState.AddModelError("", "Invalid Customer, Product, or PayMode selected.");
				ViewData["Customers"] = new SelectList(await _context.Customers.ToListAsync(), "Id", "Name");
				ViewData["Products"] = new SelectList(await _context.Products.ToListAsync(), "Name", "Price");
				ViewData["PayMode"] = new SelectList(await _context.PayModes.ToListAsync(), "Name");
				return Page();
			}

			// Asigna el precio del producto seleccionado a ProductPrice
			Sell.ProductPrice = product.Price;

			// Calcula el total de la venta
			Sell.TotalSale = Sell.ProductPrice * Sell.Quantity;

			// Añade la nueva venta a la base de datos y guarda los cambios
			_context.Sales.Add(Sell);
			await _context.SaveChangesAsync();

			// Establece las listas desplegables y vuelve a la página Index
			return RedirectToPage("./Index");
		}


	}
}






