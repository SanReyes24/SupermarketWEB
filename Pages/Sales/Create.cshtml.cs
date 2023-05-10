using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SupermarketWEB.Data;
using SupermarketWEB.Models;

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

			// Comprueba que se han rellenado todos los campos necesarios
			/*if (string.IsNullOrEmpty(Sell.Date) || string.IsNullOrEmpty(Sell.CustomerId) || string.IsNullOrEmpty(Sell.ProductName) || Sell.Quantity <= 0 || string.IsNullOrEmpty(Sell.ProductPrice) ||
				Sell.TotalSale<=0 || string.IsNullOrEmpty(Sell.PayModeName) || string.IsNullOrEmpty(Sell.Observation))
			{
				ModelState.AddModelError("", "Please fill in all required fields.");
				//ViewData["Categories"] = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
				ViewData["Customers"] = new SelectList(await _context.Customers.ToListAsync(), "Id", "Name");
				ViewData["Products"] = new SelectList(await _context.Products.ToListAsync(), "Name", "Price");
				ViewData["PayMode"] = new SelectList(await _context.PayModes.ToListAsync(), "Name");
				return Page();
			}*/

			// Recupera la categoría seleccionada
			var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Name == Sell.CustomerId);
			var productName = await _context.Products.FirstOrDefaultAsync(c => c.Name == Sell.ProductName);
			var productPrice = await _context.Products.FirstOrDefaultAsync(c => c.Price.ToString() == Sell.ProductPrice);
			var payModeName = await _context.PayModes.FirstOrDefaultAsync(c => c.Name == Sell.PayModeName);
			/*
			if (customer == null || productName==null || productPrice.Price <=0 || payModeName==null )
			{
				// Si la categoría no existe, establece la lista desplegable de categorías y muestra un mensaje de error
				ModelState.AddModelError("", "Invalid Customer selected.");
				ViewData["Customers"] = new SelectList(await _context.Customers.ToListAsync(), "Id", "Name");
				ViewData["Products"] = new SelectList(await _context.Products.ToListAsync(), "Name", "Price");
				ViewData["PayMode"] = new SelectList(await _context.PayModes.ToListAsync(), "Name");
				return Page();
			}*/

			// Añade el nuevo producto a la base de datos y guarda los cambios
			_context.Sales.Add(Sell);

			await _context.SaveChangesAsync();

			// Establece la lista desplegable de categorías y vuelve a la página Index
			/*ViewData["Categories"] = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
			*/
			return RedirectToPage("./Index");
		}
	}
}






