using FastEndpoints;
using fastdemo.DTO;

public class ProductsDB
{
    // Enkel "databas" i minnet
    public static readonly List<ProductRequest> _products = new(){
        new ProductRequest{ Id = 1, Name = "Cola", Category = "Drinks", Stock = 10 },
        new ProductRequest{ Id = 2, Name = "Candy", Category = "Snacks", Stock = 5 },
        new ProductRequest{ Id = 3, Name = "Chips", Category = "Snacks", Stock = 0 },
        new ProductRequest{ Id = 4, Name = "Water", Category = "Drinks", Stock = 0 }
    };
}

public class GetAllProductsEndpoint : EndpointWithoutRequest<List<ProductResponse>>
{
    public override void Configure()
    {
        Get("/products");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var products = ProductsDB._products.Select(p => new ProductResponse
        {
            Id = p.Id,
            FullName = $"{p.Name} ({p.Category})",
            InStock = p.Stock > 0
        }).ToList();

        await SendAsync(products, cancellation: ct);
    }
}


// Create Product
public class CreateProductEndpoint : Endpoint<ProductRequest, ProductResponse>
{
    public override void Configure()
    {
        Post("/products");
    }

    public override async Task HandleAsync(ProductRequest req, CancellationToken ct)
    {
        var newProduct = new ProductRequest
        {
            Id = ProductsDB._products.Any() ? ProductsDB._products.Max(p => p.Id) + 1 : 1,
            Name = req.Name,
            Category = req.Category,
            Stock = req.Stock
        };

        ProductsDB._products.Add(newProduct);

        var response = new ProductResponse
        {
            Id = newProduct.Id,
            FullName = $"{newProduct.Name} ({newProduct.Category})",
            InStock = newProduct.Stock > 0
        };

        await SendAsync(response, cancellation: ct);
    }
}


// Delete Product
public class DeleteProductEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/products/{id}");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var product = ProductsDB._products.FirstOrDefault(p => p.Id == id);
        if (product is null)
        {
            await SendNotFoundAsync();
            return;
        }

        ProductsDB._products.Remove(product);

        await SendNoContentAsync();
    }
}