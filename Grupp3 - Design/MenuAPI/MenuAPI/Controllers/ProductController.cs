using MenuAPI.Data;
using MenuAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MenuAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly MenuDbContext _context;

        public ProductController(MenuDbContext context)
        {
            _context = context;
        }

        // ✅ Hämta alla produkter
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // ✅ Hämta en specifik produkt
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }

        // ✅ Sök efter produkter baserat på kategori och/eller produktnamn
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

        // ✅ Lägg till en ny produkt
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody] Product model)
        {
            if (model == null)
                return BadRequest("Invalid product data");

            _context.Products.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { productId = model.ProductID }, model);
        }

        // ✅ Uppdatera en produkt
        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] Product model)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return NotFound("Product not found");

            product.ProductName = model.ProductName;
            product.Category = model.Category;
            product.Price = model.Price;
            product.IsVegetarian = model.IsVegetarian;
            product.ImageURL = model.ImageURL;
            product.Description = model.Description;

            await _context.SaveChangesAsync();
            return Ok(product);
        }

        // ✅ Radera en produkt
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return NotFound("Product not found");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ Hämta produkter utifrån kategori
        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetProductsByCategory(string category)
        {
            var products = await _context.Products
                .Where(p => (p.Category ?? "").ToLower() == category.ToLower())
                .ToListAsync();

            return Ok(products);
        }

        // ✅ Hämta alla vegetariska produkter
        [HttpGet("vegetarian")]
        public async Task<IActionResult> GetVegetarianProducts()
        {
            var products = await _context.Products.Where(p => p.IsVegetarian).ToListAsync();
            return Ok(products);
        }
    }
}
