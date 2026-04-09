using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicStore.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;

    public OrdersController(AppDbContext db)
    {
        _db = db;
    }
    [Authorize(Roles = "User,Admin")]
    [HttpPost("buyProduct")]
    public IActionResult Buy(OrderRequest request)
    {
        var product = _db.Products
            .FirstOrDefault(p => p.Id == request.ProductId && p.Status == "Active");
        if (product == null)
        {
            return BadRequest("Product not found or inactive");
        }
        if (product.Quantity <= 0)
        {
            return BadRequest("Product out of stock");
        }
        int buyQuantity = request.Quantity;
        if (request.Quantity > product.Quantity)
        {
            buyQuantity = product.Quantity;
        }
        product.Quantity -= buyQuantity;
        var order = new Order
        {
            BuyerName = User.Identity!.Name!,
            ProductName = product.Name,
            Quantity = buyQuantity,
            BuyerId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value),
            TotalPrice = product.Price * buyQuantity,
            OrderDate = DateTime.UtcNow,
            Status = "Active"
        };
        _db.Orders.Add(order);
        _db.SaveChanges();
        return Ok(new
        {
            order.Id,
            order.BuyerName,
            order.ProductName,
            order.Quantity,
            order.TotalPrice,
            order.OrderDate
        });
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("GetAllProduct")]
    public IActionResult GetAll()
    {
        var orders = _db.Orders
            .Where(o => o.Status == "Active")
            .ToList();

        return Ok(orders);
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("deleteOrder")]
    public IActionResult DeleteOrder(int id)
    {
        var order = _db.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null) return NotFound();

        order.Status = "Inactive";
        _db.SaveChanges();

        return Ok($"Order {id} removed");
    }
}