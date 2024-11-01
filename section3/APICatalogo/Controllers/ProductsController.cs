using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _context.Products?.ToList();
            if (products is null)
                return NotFound("Products not found");

            return products;
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public ActionResult<Product> Get(int id)
        {
            var product = _context.Products?.FirstOrDefault(p => p.ProductId == id);
            if (product is null)
                return NotFound("Product not found");

            return product;
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            if (product is null)
                return BadRequest();

            _context.Products?.Add(product);
            _context.SaveChanges();
            return new CreatedAtRouteResult("GetProduct",
                new { id = product.ProductId }, product);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product)
        {
            if (id != product.ProductId)
                return BadRequest();

            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(product); // Or NoContent()
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            //var product = _context.Products?.FirstOrDefault(p => p.ProductId == id);
            var product = _context.Products?.Find(id); // Leverage given that try to find on memory
            if (product is null)
                return NotFound("Product not found");

            _context.Products?.Remove(product);
            _context.SaveChanges();

            return Ok(product);
        }
    }
}