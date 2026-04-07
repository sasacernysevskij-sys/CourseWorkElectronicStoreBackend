
public class Order
{
    public int Id { get; set; }
    public string BuyerName { get; set; }
    public int BuyerId { get; set; }
    public string ProductName { get; set; }
    public double TotalPrice { get; set; }
    public int Quantity { get; set; }
    public string OrderDate { get; set; }
    public string Status { get; set; } = "Active";
}