# Developer Guide

This guide provides detailed information for developers working on this application.

## Development Environment

### Recommended IDE Setup

**Visual Studio 2022**:
- Workloads: .NET desktop development, ASP.NET and web development
- Extensions: ReSharper (optional), GitHub Copilot (optional)

**Visual Studio Code**:
- Extensions:
  - C# Dev Kit
  - Docker
  - Azure Tools
  - REST Client
  - GitLens

### Coding Standards

**C# Style**:
- Follow [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use C# 12 features where appropriate
- Async/await for all I/O operations
- Nullable reference types enabled

**Naming Conventions**:
```csharp
// Classes: PascalCase
public class ProductService { }

// Interfaces: IPascalCase
public interface IProductService { }

// Methods: PascalCase
public async Task<Product> GetProductAsync(string id) { }

// Parameters: camelCase
public void ProcessOrder(string orderId, decimal amount) { }

// Private fields: _camelCase
private readonly ILogger<ProductService> _logger;

// Constants: UPPER_CASE
private const int MAX_RETRY_ATTEMPTS = 3;
```

## Project Structure

### Solution Organization

```
src/
├── ZavaAppHost/              # Aspire orchestrator
├── Store/                    # Frontend Blazor app
├── Products/                 # Products service
├── SingleAgentDemo/          # Single agent demonstrations
├── MultiAgentDemo/           # Multi-agent demonstrations
├── AgentsCatalogService/     # Agent registry service
├── [8 Microservices]/        # Specialized agent services
├── [Entity Libraries]/       # Shared data models
├── [Provider Libraries]/     # AI framework integrations
└── ZavaServiceDefaults/      # Shared service configuration
```

### Service Template Structure

```
ServiceName/
├── Controllers/              # API controllers
│   └── ServiceController.cs
├── Models/                   # DTOs and view models
│   ├── Requests/
│   └── Responses/
├── Services/                 # Business logic
│   └── IServiceInterface.cs
├── Properties/
│   └── launchSettings.json
├── Program.cs               # Service entry point
├── ServiceName.csproj       # Project file
└── appsettings.json         # Configuration
```

## Creating a New Microservice

### 1. Create Project

```bash
cd src
dotnet new webapi -n MyNewService
cd MyNewService
```

### 2. Add References

```bash
dotnet add reference ../ZavaServiceDefaults/ZavaServiceDefaults.csproj
dotnet add reference ../SharedEntities/SharedEntities.csproj
```

### 3. Configure Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add service defaults (telemetry, health checks, etc.)
builder.AddServiceDefaults();

// Add your services
builder.Services.AddScoped<IMyService, MyService>();

// Add controllers
builder.Services.AddControllers();

// Add AI provider (choose one)
// builder.Services.AddSemanticKernelProvider();
// builder.Services.AddAgentFxProvider();

var app = builder.Build();

// Map default endpoints (health, etc.)
app.MapDefaultEndpoints();

// Map controllers
app.MapControllers();

app.Run();
```

### 4. Create Controller

```csharp
using Microsoft.AspNetCore.Mvc;

namespace MyNewService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MyController : ControllerBase
{
    private readonly ILogger<MyController> _logger;
    private readonly IMyService _service;

    public MyController(
        ILogger<MyController> logger,
        IMyService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var result = await _service.ProcessAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing request");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
```

### 5. Register in ZavaAppHost

```csharp
// src/ZavaAppHost/Program.cs
var myNewService = builder.AddProject<Projects.MyNewService>("mynewservice")
    .WithExternalHttpEndpoints();
```

## Working with AI Agents

### Using Semantic Kernel

```csharp
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

public class ProductSearchService
{
    private readonly Kernel _kernel;
    
    public ProductSearchService(Kernel kernel)
    {
        _kernel = kernel;
    }
    
    public async Task<SearchResult> SearchAsync(string query)
    {
        // Create agent client
        var agent = await _kernel.GetAgentAsync("product-search-agent");
        
        // Execute search
        var thread = await agent.CreateThreadAsync();
        var message = await thread.AddMessageAsync(query);
        var run = await thread.RunAsync();
        
        // Get response
        var response = await run.GetResponseAsync();
        
        return ParseResponse(response);
    }
}
```

### Using Agent Framework

```csharp
using Microsoft.Agents.AI;

public class ProductSearchService
{
    private readonly IAgentProvider _agentProvider;
    
    public ProductSearchService(IAgentProvider agentProvider)
    {
        _agentProvider = agentProvider;
    }
    
    public async Task<SearchResult> SearchAsync(string query)
    {
        // Get agent
        var agent = await _agentProvider.GetAgentAsync("product-search-agent");
        
        // Create context
        var context = new AgentContext
        {
            Input = query,
            Parameters = new Dictionary<string, object>
            {
                ["maxResults"] = 10
            }
        };
        
        // Execute
        var result = await agent.ExecuteAsync(context);
        
        return new SearchResult
        {
            Products = result.Products,
            RelevanceScores = result.Scores
        };
    }
}
```

## Database Operations

### Entity Framework Core

```csharp
using Microsoft.EntityFrameworkCore;

public class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.HasIndex(e => e.Category);
        });
    }
}
```

### Repository Pattern

```csharp
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(string id);
    Task<List<Product>> GetAllAsync();
    Task<List<Product>> SearchAsync(string query);
    Task<Product> CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(string id);
}

public class ProductRepository : IProductRepository
{
    private readonly ProductsDbContext _context;
    
    public ProductRepository(ProductsDbContext context)
    {
        _context = context;
    }
    
    public async Task<Product?> GetByIdAsync(string id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task<List<Product>> SearchAsync(string query)
    {
        return await _context.Products
            .Where(p => EF.Functions.Like(p.Name, $"%{query}%") ||
                       EF.Functions.Like(p.Description, $"%{query}%"))
            .OrderByDescending(p => p.CreatedAt)
            .Take(50)
            .ToListAsync();
    }
}
```

## API Development

### Request Validation

```csharp
using FluentValidation;

public class SearchRequest
{
    public string Query { get; set; }
    public int MaxResults { get; set; } = 10;
    public List<string> Categories { get; set; } = new();
}

public class SearchRequestValidator : AbstractValidator<SearchRequest>
{
    public SearchRequestValidator()
    {
        RuleFor(x => x.Query)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(200);
            
        RuleFor(x => x.MaxResults)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);
    }
}
```

### Response Formatting

```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public ErrorDetails? Error { get; set; }
    public ResponseMetadata Metadata { get; set; }
}

public class ErrorDetails
{
    public string Code { get; set; }
    public string Message { get; set; }
    public List<string> ValidationErrors { get; set; }
}

public class ResponseMetadata
{
    public string CorrelationId { get; set; }
    public DateTime Timestamp { get; set; }
    public TimeSpan Duration { get; set; }
}
```

### Error Handling Middleware

```csharp
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    
    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        
        var errorResponse = new ApiResponse<object>
        {
            Success = false,
            Error = new ErrorDetails
            {
                Code = "INTERNAL_ERROR",
                Message = exception.Message
            },
            Metadata = new ResponseMetadata
            {
                CorrelationId = context.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            }
        };
        
        response.StatusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
        
        await response.WriteAsJsonAsync(errorResponse);
    }
}
```

## Testing

### Unit Tests

```csharp
using Xunit;
using Moq;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly ProductService _service;
    
    public ProductServiceTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _service = new ProductService(_mockRepository.Object);
    }
    
    [Fact]
    public async Task GetProduct_WithValidId_ReturnsProduct()
    {
        // Arrange
        var productId = "1";
        var expectedProduct = new Product { Id = productId, Name = "Test" };
        _mockRepository
            .Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync(expectedProduct);
        
        // Act
        var result = await _service.GetProductAsync(productId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProduct.Id, result.Id);
        Assert.Equal(expectedProduct.Name, result.Name);
    }
    
    [Fact]
    public async Task GetProduct_WithInvalidId_ThrowsException()
    {
        // Arrange
        var productId = "invalid";
        _mockRepository
            .Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync((Product?)null);
        
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetProductAsync(productId));
    }
}
```

### Integration Tests

```csharp
using Microsoft.AspNetCore.Mvc.Testing;

public class ProductsApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    
    public ProductsApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetProducts_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/products");
        
        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json", 
            response.Content.Headers.ContentType?.MediaType);
    }
    
    [Fact]
    public async Task SearchProducts_WithQuery_ReturnsResults()
    {
        // Arrange
        var searchRequest = new { query = "drill", maxResults = 10 };
        
        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/products/search", 
            searchRequest);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var results = await response.Content
            .ReadFromJsonAsync<SearchResponse>();
        Assert.NotNull(results);
        Assert.NotEmpty(results.Products);
    }
}
```

## Logging and Telemetry

### Structured Logging

```csharp
public class ProductService
{
    private readonly ILogger<ProductService> _logger;
    
    public async Task<Product> GetProductAsync(string id)
    {
        using var scope = _logger.BeginScope(
            new Dictionary<string, object>
            {
                ["ProductId"] = id,
                ["Operation"] = "GetProduct"
            });
        
        _logger.LogInformation(
            "Retrieving product {ProductId}", id);
        
        try
        {
            var product = await _repository.GetByIdAsync(id);
            
            if (product == null)
            {
                _logger.LogWarning(
                    "Product {ProductId} not found", id);
                throw new KeyNotFoundException($"Product {id} not found");
            }
            
            _logger.LogInformation(
                "Successfully retrieved product {ProductId}", id);
            
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error retrieving product {ProductId}", id);
            throw;
        }
    }
}
```

### Custom Metrics

```csharp
using System.Diagnostics.Metrics;

public class ProductService
{
    private static readonly Meter _meter = new("ProductService");
    private static readonly Counter<long> _productRetrievals = 
        _meter.CreateCounter<long>("product_retrievals");
    private static readonly Histogram<double> _retrievalDuration = 
        _meter.CreateHistogram<double>("product_retrieval_duration");
    
    public async Task<Product> GetProductAsync(string id)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            var product = await _repository.GetByIdAsync(id);
            
            _productRetrievals.Add(1, 
                new KeyValuePair<string, object?>("status", "success"));
            
            return product;
        }
        catch (Exception)
        {
            _productRetrievals.Add(1, 
                new KeyValuePair<string, object?>("status", "failure"));
            throw;
        }
        finally
        {
            stopwatch.Stop();
            _retrievalDuration.Record(stopwatch.ElapsedMilliseconds);
        }
    }
}
```

## Performance Optimization

### Caching

```csharp
using Microsoft.Extensions.Caching.Distributed;

public class CachedProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IDistributedCache _cache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(1);
    
    public async Task<Product> GetProductAsync(string id)
    {
        var cacheKey = $"product:{id}";
        
        // Try cache first
        var cachedValue = await _cache.GetStringAsync(cacheKey);
        if (cachedValue != null)
        {
            return JsonSerializer.Deserialize<Product>(cachedValue)!;
        }
        
        // Cache miss - get from repository
        var product = await _repository.GetByIdAsync(id);
        
        if (product != null)
        {
            // Store in cache
            var serialized = JsonSerializer.Serialize(product);
            await _cache.SetStringAsync(
                cacheKey,
                serialized,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheDuration
                });
        }
        
        return product;
    }
}
```

### Async Patterns

```csharp
// Good: Parallel execution
var tasks = new[]
{
    GetProductAsync(id1),
    GetProductAsync(id2),
    GetProductAsync(id3)
};
var products = await Task.WhenAll(tasks);

// Good: Streaming results
public async IAsyncEnumerable<Product> StreamProductsAsync()
{
    await foreach (var product in _repository.GetAllAsync())
    {
        yield return product;
    }
}

// Bad: Blocking async call
var product = GetProductAsync(id).Result; // DON'T DO THIS

// Bad: Async void (except event handlers)
public async void ProcessProduct() // DON'T DO THIS
{
    await Task.Delay(1000);
}
```

## Security Best Practices

### Input Validation

```csharp
public class ProductController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct(
        [FromBody] CreateProductRequest request)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Product name is required");
        }
        
        // Sanitize HTML input
        request.Description = HtmlEncoder.Default.Encode(
            request.Description);
        
        // Validate price range
        if (request.Price < 0 || request.Price > 1000000)
        {
            return BadRequest("Invalid price");
        }
        
        var product = await _service.CreateProductAsync(request);
        return CreatedAtAction(nameof(GetProduct), 
            new { id = product.Id }, product);
    }
}
```

### Secrets Management

```csharp
// Good: Use configuration
var connectionString = configuration.GetConnectionString("ProductsDb");

// Good: Use Key Vault in production
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{keyVaultName}.vault.azure.net/"),
    new DefaultAzureCredential());

// Bad: Hardcoded secrets
var apiKey = "sk-1234567890"; // DON'T DO THIS
```

## CI/CD Integration

### GitHub Actions Workflow

```yaml
name: Build and Test

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  build:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 9.0.x
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Test
      run: dotnet test --no-build --verbosity normal
    
    - name: Publish
      run: dotnet publish -c Release -o ./publish
```

## Debugging Tips

### Remote Debugging in Container

```bash
# Attach to running container
docker exec -it <container-id> bash

# View logs
docker logs <container-id> --follow

# Inspect environment
docker exec <container-id> env
```

### Aspire Dashboard

- View all service logs in one place
- Trace requests across services
- Monitor performance metrics
- Check service health

### Application Insights

```csharp
// Add custom trace
telemetryClient.TrackTrace("Custom message", 
    SeverityLevel.Information,
    new Dictionary<string, string>
    {
        ["CustomProperty"] = "value"
    });

// Track custom event
telemetryClient.TrackEvent("ProductPurchased",
    new Dictionary<string, string>
    {
        ["ProductId"] = productId,
        ["CustomerId"] = customerId
    });
```

## Best Practices Summary

✅ **DO**:
- Use async/await for all I/O operations
- Implement proper error handling
- Add logging with structured data
- Write unit and integration tests
- Use dependency injection
- Follow SOLID principles
- Cache expensive operations
- Validate all inputs
- Use meaningful variable names
- Document public APIs

❌ **DON'T**:
- Block async code with .Result or .Wait()
- Catch exceptions without handling them
- Hardcode configuration values
- Ignore compiler warnings
- Skip error handling
- Use magic numbers
- Leave debugging code in production
- Expose sensitive data in logs
- Trust user input without validation

## Next Steps

- [Architecture Overview](01-architecture-overview.md) - Understand the system design
- [Service Interactions](03-service-interactions.md) - Learn how services communicate
- [API Reference](04-api-reference.md) - Complete API documentation

## Additional Resources

- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [Azure AI Foundry Documentation](https://learn.microsoft.com/en-us/azure/ai-studio/)
- [Semantic Kernel Documentation](https://learn.microsoft.com/en-us/semantic-kernel/)
