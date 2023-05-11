using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupermarketWEB.Data;
using SupermarketWEB.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SupermarketWEB.Pages.Sales
{
    public class EditModel : PageModel
    {
        private readonly SupermarketContext _context;

        public EditModel(SupermarketContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Sell Sell { get; set; }
        public SelectList Customer { get; set; }
        public SelectList Product2 { get; set; }
        public SelectList PayMode { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sell = await _context.Sales.FirstOrDefaultAsync(m => m.Id == id);

            if (Sell == null)
            {
                return NotFound();
            }

            Customer = new SelectList(_context.Customers
                .Where(c => !string.IsNullOrEmpty(c.Name))
                .Select(c => new SelectListItem
                {
                    Value = c.Name,
                    Text = c.Name
                }), "Value", "Text", Sell.CustomerId);

            Product2 = new SelectList(_context.Products
                .Where(p => !string.IsNullOrEmpty(p.Name))
                .Select(p => new SelectListItem
                {
                    Value = p.Name,
                    Text = $"{p.Name} - {p.Price}"
                }), "Value", "Text", Sell.ProductName);

            PayMode = new SelectList(_context.PayModes
                .Where(pm => !string.IsNullOrEmpty(pm.Name))
                .Select(pm => new SelectListItem
                {
                    Value = pm.Name,
                    Text = pm.Name
                }), "Value", "Text", Sell.PayModeName);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Customer = new SelectList(_context.Customers
                    .Where(c => !string.IsNullOrEmpty(c.Name))
                    .Select(c => new SelectListItem
                    {
                        Value = c.Name,
                        Text = c.Name
                    }), "Value", "Text");

                Product2 = new SelectList(_context.Products
                    .Where(p => !string.IsNullOrEmpty(p.Name))
                    .Select(p => new SelectListItem
                    {
                        Value = p.Name,
                        Text = $"{p.Name} - {p.Price}"
                    }), "Value", "Text");

                PayMode = new SelectList(_context.PayModes
                    .Where(pm => !string.IsNullOrEmpty(pm.Name))
                    .Select(pm => new SelectListItem
                    {
                        Value = pm.Name,
                        Text = pm.Name
                    }), "Value", "Text");

                return Page();
            }

            _context.Attach(Sell).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SellExists(Sell.Id))
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

        private bool SellExists(int id)
        {
            return _context.Sales.Any(e => e.Id == id);
        }
    }
}


