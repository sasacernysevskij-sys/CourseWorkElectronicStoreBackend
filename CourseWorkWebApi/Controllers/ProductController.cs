using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ElectronicStore.Controllers;
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProductsController(AppDbContext db)
    {
        _db = db;
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public IActionResult Create(Product request)
    {
        var product = new Product
        {
            TradeMark = request.TradeMark,
            Name = request.Name,
            Price = request.Price,
            Quantity = request.Quantity,
            Type = request.Type,
            AddToStoreDate = DateTime.UtcNow,
            Status = "Active",
            PictureProduct = request.PictureProduct
        };
        _db.Products.Add(product);
        _db.SaveChanges();
        return Ok(product);
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("deleteProduct")]
    public IActionResult DeleteProduct(int id)
    {
        var product = _db.Products.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();
        product.Status = "Inactive";
        _db.SaveChanges();
        return Ok("Product removed");
    }
    [Authorize(Roles = "Admin")]
    [HttpPatch("updateProduct")]
    public IActionResult UpdateProduct(int id, Product request)
    {
        var product = _db.Products.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();
        product.Name = request.Name;
        product.Price = request.Price;
        product.Quantity = request.Quantity;
        product.Type = request.Type;
        product.PictureProduct = request.PictureProduct;
        product.AddToStoreDate = DateTime.UtcNow;
        product.TradeMark = request.TradeMark;
        _db.SaveChanges();
        return Ok(product);
    }
    [HttpGet]
    public IActionResult GetProducts(
    int page = 1,
    int pageSize = 8,
    string? type = null,
    string? brand = null
)
    {
        var query = _db.Products.AsQueryable();

        // FILTER TYPE
        if (!string.IsNullOrWhiteSpace(type))
        {
            query = query.Where(p => p.Type.ToLower() == type.ToLower());
        }

        // FILTER BRAND
        if (!string.IsNullOrWhiteSpace(brand))
        {
            query = query.Where(p => p.TradeMark.ToLower() == brand.ToLower());
        }

        // TOTAL COUNT (before pagination)
        var totalItems = query.Count();

        // PAGINATION (SLICE)
        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Ok(new
        {
            data = items,
            totalItems = totalItems,
            totalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
            currentPage = page
        });
    }

}