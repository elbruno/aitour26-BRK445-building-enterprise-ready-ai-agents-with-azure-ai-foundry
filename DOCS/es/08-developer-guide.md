# Guía del Desarrollador

Esta guía proporciona información detallada para desarrolladores que trabajan en esta aplicación.

## Entorno de Desarrollo

### Configuración de IDE Recomendada

**Visual Studio 2022**:
- Cargas de trabajo: Desarrollo de escritorio .NET, Desarrollo web y ASP.NET
- Extensiones: ReSharper (opcional), GitHub Copilot (opcional)

**Visual Studio Code**:
- Extensiones:
  - C# Dev Kit
  - Docker
  - Azure Tools
  - REST Client

### Estándares de Codificación

**Estilo de C#**:
- Seguir [Convenciones de Codificación de C# de Microsoft](https://docs.microsoft.com/es-es/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Usar características de C# 12 cuando sea apropiado
- Async/await para todas las operaciones de I/O
- Tipos de referencia anulables habilitados

**Convenciones de Nomenclatura**:
```csharp
// Clases: PascalCase
public class ProductService { }

// Interfaces: IPascalCase
public interface IProductService { }

// Métodos: PascalCase
public async Task<Product> GetProductAsync(string id) { }

// Parámetros: camelCase
public void ProcessOrder(string orderId, decimal amount) { }

// Campos privados: _camelCase
private readonly ILogger<ProductService> _logger;

// Constantes: UPPER_CASE
private const int MAX_RETRY_ATTEMPTS = 3;
```

## Estructura del Proyecto

### Organización de la Solución

```
src/
├── ZavaAppHost/              # Orquestador Aspire
├── Store/                    # App Blazor frontend
├── Products/                 # Servicio de productos
├── SingleAgentDemo/          # Demostraciones de agente único
├── MultiAgentDemo/           # Demostraciones multi-agente
├── AgentsCatalogService/     # Servicio de registro de agentes
├── [8 Microservicios]/       # Servicios de agentes especializados
├── [Bibliotecas de Entidades]/ # Modelos de datos compartidos
└── ZavaServiceDefaults/      # Configuración de servicio compartida
```

## Trabajar con Agentes de IA

### Usar Semantic Kernel

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
        // Crear cliente de agente
        var agent = await _kernel.GetAgentAsync("product-search-agent");
        
        // Ejecutar búsqueda
        var thread = await agent.CreateThreadAsync();
        var message = await thread.AddMessageAsync(query);
        var run = await thread.RunAsync();
        
        // Obtener respuesta
        var response = await run.GetResponseAsync();
        
        return ParseResponse(response);
    }
}
```

### Usar Agent Framework

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
        // Obtener agente
        var agent = await _agentProvider.GetAgentAsync("product-search-agent");
        
        // Crear contexto
        var context = new AgentContext
        {
            Input = query,
            Parameters = new Dictionary<string, object>
            {
                ["maxResults"] = 10
            }
        };
        
        // Ejecutar
        var result = await agent.ExecuteAsync(context);
        
        return new SearchResult
        {
            Products = result.Products,
            RelevanceScores = result.Scores
        };
    }
}
```

## Operaciones de Base de Datos

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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.HasIndex(e => e.Category);
        });
    }
}
```

## Desarrollo de API

### Validación de Solicitud

```csharp
using FluentValidation;

public class SearchRequest
{
    public string Query { get; set; }
    public int MaxResults { get; set; } = 10;
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

## Pruebas

### Pruebas Unitarias

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
        var expectedProduct = new Product { Id = productId };
        _mockRepository
            .Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync(expectedProduct);
        
        // Act
        var result = await _service.GetProductAsync(productId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProduct.Id, result.Id);
    }
}
```

## Logging y Telemetría

### Logging Estructurado

```csharp
public class ProductService
{
    private readonly ILogger<ProductService> _logger;
    
    public async Task<Product> GetProductAsync(string id)
    {
        _logger.LogInformation(
            "Recuperando producto {ProductId}", id);
        
        try
        {
            var product = await _repository.GetByIdAsync(id);
            
            if (product == null)
            {
                _logger.LogWarning(
                    "Producto {ProductId} no encontrado", id);
            }
            
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error recuperando producto {ProductId}", id);
            throw;
        }
    }
}
```

## Optimización de Rendimiento

### Almacenamiento en Caché

```csharp
using Microsoft.Extensions.Caching.Distributed;

public class CachedProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IDistributedCache _cache;
    
    public async Task<Product> GetProductAsync(string id)
    {
        var cacheKey = $"product:{id}";
        
        // Intentar caché primero
        var cachedValue = await _cache.GetStringAsync(cacheKey);
        if (cachedValue != null)
        {
            return JsonSerializer.Deserialize<Product>(cachedValue)!;
        }
        
        // Fallo de caché - obtener del repositorio
        var product = await _repository.GetByIdAsync(id);
        
        if (product != null)
        {
            // Almacenar en caché
            await _cache.SetStringAsync(cacheKey,
                JsonSerializer.Serialize(product));
        }
        
        return product;
    }
}
```

## Mejores Prácticas

✅ **HACER**:
- Usar async/await para todas las operaciones de I/O
- Implementar manejo de errores apropiado
- Agregar logging con datos estructurados
- Escribir pruebas unitarias y de integración
- Usar inyección de dependencias
- Seguir principios SOLID

❌ **NO HACER**:
- Bloquear código async con .Result o .Wait()
- Capturar excepciones sin manejarlas
- Codificar valores de configuración
- Ignorar advertencias del compilador
- Omitir manejo de errores

## Próximos Pasos

- [Descripción General de la Arquitectura](01-architecture-overview.md) - Entender el diseño del sistema
- [Interacciones de Servicios](03-service-interactions.md) - Aprender cómo se comunican los servicios
- [Referencia de API](04-api-reference.md) - Documentación completa de API

## Recursos Adicionales

- [Documentación de .NET](https://docs.microsoft.com/es-es/dotnet/)
- [Documentación de Aspire](https://learn.microsoft.com/es-es/dotnet/aspire/)
- [Documentación de Azure AI Foundry](https://learn.microsoft.com/es-es/azure/ai-studio/)
