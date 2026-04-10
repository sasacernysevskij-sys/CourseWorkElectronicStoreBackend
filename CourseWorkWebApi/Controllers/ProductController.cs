using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [HttpGet("all")]
    public IActionResult GetAllProduct()
    {
        var products = _db.Products
            .Where(p => p.Status == "Active")
            .ToList();

        return Ok(products);
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
    public IActionResult GetProductsBySlice(int page = 1, int pageSize = 8)
    {
        var query = _db.Products
            .Where(p => p.Status == "Active");

        var totalCount = query.Count();

        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.PictureProduct
            })
            .ToList();

        return Ok(new
        {
            items,
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        });
    }
}