namespace fastDemo.DTO;

public class ProductRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Category { get; set; }
    public int Stock { get; set; }
}
