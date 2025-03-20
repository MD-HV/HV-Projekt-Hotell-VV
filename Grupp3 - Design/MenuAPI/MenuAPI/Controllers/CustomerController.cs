using MenuAPI.Data;
using MenuAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MenuAPI.Controllers
{
    [Route("api/customer/products")]
    [ApiController]
    public class CustomerProductController : ControllerBase
    {
        private readonly MenuDbContext _context;

        public CustomerProductController(MenuDbContext context)
        {
            _context = context;
        }

        // Hämta alla produkter
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // Hämta en specifik produkt
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return NotFound("Produkten hittades inte");

            return Ok(product);
        }

        // Sök efter produkter baserat på kategori och/eller namn
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string? category, [FromQuery] string? name)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => (p.Category ?? "").ToLower().Contains(category.ToLower()));
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => (p.ProductName ?? "").ToLower().Contains(name.ToLower()));
            }

            var products = await query.ToListAsync();
            return Ok(products);
        }

        // Hämta produkter utifrån kategori
        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetProductsByCategory(string category)
        {
            var products = await _context.Products
                .Where(p => (p.Category ?? "").ToLower() == category.ToLower())
                .ToListAsync();

            return Ok(products);
        }

        // Hämta alla vegetariska produkter
        [HttpGet("vegetarian")]
        public async Task<IActionResult> GetVegetarianProducts()
        {
            var products = await _context.Products.Where(p => p.IsVegetarian).ToListAsync();
            return Ok(products);
        }
    }
}

