using System.ComponentModel.DataAnnotations;
public class Product
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }
    [MaxLength(50)]
    [MinLength(2)]
    public string?  TradeMark{ get; set; }="Without";

    [Range(0, 10000000)]
    public double Price { get; set; }

    [Range(0, 100000)]
    public int Quantity { get; set; }

    [MaxLength(50)]
    public string Type { get; set; }
    public DateTime AddToStoreDate { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Active";

    [MaxLength(255)]
    public string PictureProduct { get; set; } = "Without";
}