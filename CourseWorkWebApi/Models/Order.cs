using System.ComponentModel.DataAnnotations;
public class Order
{
    public int Id { get; set; }
    [MaxLength(50)]
    [MinLength(8)]
    public string BuyerName { get; set; }

    public int BuyerId { get; set; }

    [MaxLength(50)]
    public string ProductName { get; set; }

    [Range(0, 10000000)]
    public double TotalPrice { get; set; }

    [Range(1, 100000)]
    public int Quantity { get; set; }

    public DateTime OrderDate { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Active";
}