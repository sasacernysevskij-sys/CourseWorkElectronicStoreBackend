using System.ComponentModel.DataAnnotations;
public class OrderRequest
{
    public int ProductId { get; set; }
    [Range(1, 100000)]
    public int Quantity { get; set; }
}