
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public string Type { get; set; }
    public string SubType { get; set; }
    public string AddToStoreDate { get; set; }
    public string Status { get; set; } = "Active";
    public string PictureProduct { get; set; } = "Without";
}