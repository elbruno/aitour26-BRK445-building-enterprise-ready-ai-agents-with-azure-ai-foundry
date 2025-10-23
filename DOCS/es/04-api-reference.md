# Referencia de API

Documentación completa de la API para todos los servicios en la aplicación.

## URLs Base

### Desarrollo
```
Store: https://localhost:7001
Products: https://localhost:7002
SingleAgentDemo: https://localhost:7003
MultiAgentDemo: https://localhost:7004
AgentCatalog: https://localhost:7005
```

### Producción
Las URLs son proporcionadas por el despliegue de Azure y gestionadas a través de Aspire.

## Encabezados Comunes

Todas las solicitudes de API deben incluir:

```http
Content-Type: application/json
Accept: application/json
X-Correlation-ID: <uuid>
User-Agent: <nombre-cliente>/<versión>
```

## Códigos de Respuesta Comunes

| Código | Significado | Descripción |
|------|---------|-------------|
| 200 | OK | Solicitud exitosa |
| 201 | Created | Recurso creado exitosamente |
| 400 | Bad Request | Entrada inválida |
| 401 | Unauthorized | Autenticación requerida |
| 404 | Not Found | Recurso no encontrado |
| 429 | Too Many Requests | Límite de tasa excedido |
| 500 | Internal Server Error | Error del servidor |

## API de Productos

### Listar Productos

```http
GET /api/products
```

**Parámetros de Consulta:**
- `page` (entero): Número de página (predeterminado: 1)
- `pageSize` (entero): Elementos por página (predeterminado: 20, máx: 100)
- `category` (cadena): Filtrar por categoría

**Respuesta:**
```json
{
  "items": [
    {
      "id": "1",
      "name": "Taladro Inalámbrico 18V",
      "description": "Taladro profesional inalámbrico",
      "price": 129.99,
      "category": "Herramientas",
      "imageUrl": "/images/drill.png",
      "inStock": true
    }
  ],
  "page": 1,
  "pageSize": 20,
  "totalItems": 150,
  "totalPages": 8
}
```

### Obtener Producto

```http
GET /api/products/{id}
```

**Respuesta:**
```json
{
  "id": "1",
  "name": "Taladro Inalámbrico 18V",
  "description": "Taladro profesional inalámbrico con batería",
  "price": 129.99,
  "category": "Herramientas",
  "specifications": {
    "voltage": "18V",
    "weight": "3.5 lbs"
  },
  "inStock": true,
  "stockQuantity": 45
}
```

### Buscar Productos

```http
GET /api/products/search?q={consulta}
```

**Parámetros:**
- `q` (cadena, requerido): Consulta de búsqueda
- `category` (cadena): Filtrar por categoría
- `minPrice` (decimal): Precio mínimo
- `maxPrice` (decimal): Precio máximo

## API de Demo de Agente Único

### Ejecutar Agente Único (Semantic Kernel)

```http
POST /api/singleagent/sk/execute
```

**Solicitud:**
```json
{
  "query": "Encuéntrame taladros rojos por debajo de $200",
  "customerId": "cust-123",
  "includeInventory": true,
  "maxResults": 10
}
```

**Respuesta:**
```json
{
  "success": true,
  "results": [
    {
      "product": {
        "id": "1",
        "name": "Taladro Inalámbrico 18V",
        "price": 129.99
      },
      "inventory": {
        "available": true,
        "quantity": 45,
        "nearestStore": "Tienda A"
      },
      "relevanceScore": 0.95
    }
  ],
  "reasoning": "Encontré 3 taladros rojos que coinciden con tus criterios",
  "executionTime": "1.2s"
}
```

## API de Demo Multi-Agente

### Orquestar Multi-Agente (Semantic Kernel)

```http
POST /api/multiagent/sk/orchestrate
```

**Solicitud:**
```json
{
  "query": "Encontrar productos similares en stock cerca de mí",
  "image": "imagen-codificada-base64",
  "location": {
    "latitude": 47.6062,
    "longitude": -122.3321
  },
  "customerId": "cust-123"
}
```

**Respuesta:**
```json
{
  "success": true,
  "workflow": {
    "steps": [
      {
        "agent": "PhotoAnalyzer",
        "action": "Imagen analizada",
        "duration": "0.5s"
      },
      {
        "agent": "ProductSearch",
        "action": "Encontrados 5 productos similares",
        "duration": "0.3s"
      },
      {
        "agent": "Inventory",
        "action": "Disponibilidad verificada",
        "duration": "0.2s"
      },
      {
        "agent": "Location",
        "action": "Tiendas más cercanas encontradas",
        "duration": "0.1s"
      }
    ]
  },
  "results": [...],
  "totalExecutionTime": "1.1s"
}
```

## API de Catálogo de Agentes

### Listar Todos los Agentes

```http
GET /api/agentcatalog
```

**Respuesta:**
```json
{
  "agents": [
    {
      "id": "photo-analyzer",
      "name": "Analizador de Fotos",
      "description": "Analiza imágenes de productos",
      "capabilities": ["análisis-imagen", "identificación-producto"],
      "status": "activo",
      "version": "1.0"
    }
  ],
  "total": 8
}
```

## Servicios de Microservicios

### Servicio de Análisis de Fotos

**Analizar Imagen**

```http
POST /api/analyzephoto/analyze
```

**Solicitud:**
```json
{
  "image": "datos-imagen-codificados-base64",
  "includeAttributes": true,
  "findSimilar": true
}
```

### Servicio de Información del Cliente

**Obtener Perfil de Cliente**

```http
GET /api/customerinfo/{customerId}
```

### Servicio de Inventario

**Verificar Inventario de Producto**

```http
GET /api/inventory/{productId}
```

**Respuesta:**
```json
{
  "productId": "1",
  "available": true,
  "totalQuantity": 125,
  "stores": [
    {
      "storeId": "store-1",
      "storeName": "Tienda Centro",
      "quantity": 45,
      "reservable": true
    }
  ],
  "lastUpdated": "2024-10-23T10:30:00Z"
}
```

### Servicio de Ubicación

**Listar Todas las Tiendas**

```http
GET /api/location/stores
```

**Encontrar Tienda Más Cercana**

```http
GET /api/location/nearest?lat={latitud}&lon={longitud}
```

### Servicio de Recomendaciones

**Obtener Recomendaciones**

```http
POST /api/matchmaking/recommend
```

### Servicio de Navegación

**Ubicar Producto**

```http
GET /api/navigation/locate/{productId}?storeId={storeId}
```

### Servicio de Búsqueda de Productos

**Buscar Productos**

```http
POST /api/productsearch/search
```

### Servicio de Razonamiento

**Realizar Razonamiento**

```http
POST /api/toolreasoning/reason
```

## Respuestas de Error

Todas los endpoints pueden devolver respuestas de error en este formato:

```json
{
  "error": {
    "code": "ERROR_CODE",
    "message": "Mensaje de error legible",
    "details": "Detalles adicionales sobre el error",
    "correlationId": "7b3f8a2c-9e1d-4f6b-a5c3-8d9e2f1a3b4c",
    "timestamp": "2024-10-23T10:30:00Z"
  }
}
```

## Límites de Tasa

| Nivel | Solicitudes/Minuto | Ráfaga |
|------|----------------|-------|
| Anónimo | 60 | 10 |
| Autenticado | 600 | 100 |
| Premium | 6000 | 1000 |

## Próximos Pasos

- [Flujo de Datos](05-data-flow.md) - Comprensión de modelos de datos
- [Guía del Desarrollador](08-developer-guide.md) - Detalles de implementación
