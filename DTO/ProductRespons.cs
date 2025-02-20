namespace fastDemo.DTO;

public class ProductResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool InStock { get; set; }
}
