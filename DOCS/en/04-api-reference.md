# API Reference

Complete API documentation for all services in the application.

## Base URLs

### Development
```
Store: https://localhost:7001
Products: https://localhost:7002
SingleAgentDemo: https://localhost:7003
MultiAgentDemo: https://localhost:7004
AgentCatalog: https://localhost:7005
AnalyzePhoto: https://localhost:7010
CustomerInfo: https://localhost:7011
Inventory: https://localhost:7012
Location: https://localhost:7013
Matchmaking: https://localhost:7014
Navigation: https://localhost:7015
ProductSearch: https://localhost:7016
ToolReasoning: https://localhost:7017
```

### Production
URLs are provided by Azure deployment and managed through Aspire.

## Common Headers

All API requests should include:

```http
Content-Type: application/json
Accept: application/json
X-Correlation-ID: <uuid>
User-Agent: <client-name>/<version>
```

## Common Response Codes

| Code | Meaning | Description |
|------|---------|-------------|
| 200 | OK | Successful request |
| 201 | Created | Resource created successfully |
| 400 | Bad Request | Invalid input |
| 401 | Unauthorized | Authentication required |
| 403 | Forbidden | Insufficient permissions |
| 404 | Not Found | Resource not found |
| 429 | Too Many Requests | Rate limit exceeded |
| 500 | Internal Server Error | Server error |
| 503 | Service Unavailable | Service temporarily unavailable |

## Products API

### List Products

```http
GET /api/products
```

**Query Parameters:**
- `page` (integer): Page number (default: 1)
- `pageSize` (integer): Items per page (default: 20, max: 100)
- `category` (string): Filter by category

**Response:**
```json
{
  "items": [
    {
      "id": "1",
      "name": "18V Cordless Drill",
      "description": "Professional cordless drill",
      "price": 129.99,
      "category": "Tools",
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

### Get Product

```http
GET /api/products/{id}
```

**Response:**
```json
{
  "id": "1",
  "name": "18V Cordless Drill",
  "description": "Professional cordless drill with battery",
  "price": 129.99,
  "category": "Tools",
  "imageUrl": "/images/drill.png",
  "specifications": {
    "voltage": "18V",
    "weight": "3.5 lbs",
    "batteryIncluded": true
  },
  "inStock": true,
  "stockQuantity": 45
}
```

### Search Products

```http
GET /api/products/search?q={query}
```

**Query Parameters:**
- `q` (string, required): Search query
- `category` (string): Filter by category
- `minPrice` (decimal): Minimum price
- `maxPrice` (decimal): Maximum price

**Response:**
```json
{
  "results": [
    {
      "id": "1",
      "name": "18V Cordless Drill",
      "price": 129.99,
      "relevance": 0.95
    }
  ],
  "total": 10
}
```

## Single Agent Demo API

### Execute Single Agent (Semantic Kernel)

```http
POST /api/singleagent/sk/execute
```

**Request:**
```json
{
  "query": "Find me red power drills under $200",
  "customerId": "cust-123",
  "includeInventory": true,
  "maxResults": 10
}
```

**Response:**
```json
{
  "success": true,
  "results": [
    {
      "product": {
        "id": "1",
        "name": "18V Cordless Drill",
        "price": 129.99
      },
      "inventory": {
        "available": true,
        "quantity": 45,
        "nearestStore": "Store A"
      },
      "relevanceScore": 0.95
    }
  ],
  "reasoning": "Found 3 red power drills matching your criteria",
  "executionTime": "1.2s"
}
```

### Execute Single Agent (Agent Framework)

```http
POST /api/singleagent/agentfx/execute
```

Same request/response format as SK endpoint.

## Multi-Agent Demo API

### Orchestrate Multi-Agent (Semantic Kernel)

```http
POST /api/multiagent/sk/orchestrate
```

**Request:**
```json
{
  "query": "Find similar products in stock near me",
  "image": "base64-encoded-image",
  "location": {
    "latitude": 47.6062,
    "longitude": -122.3321
  },
  "customerId": "cust-123"
}
```

**Response:**
```json
{
  "success": true,
  "workflow": {
    "steps": [
      {
        "agent": "PhotoAnalyzer",
        "action": "Analyzed image",
        "duration": "0.5s"
      },
      {
        "agent": "ProductSearch",
        "action": "Found 5 similar products",
        "duration": "0.3s"
      },
      {
        "agent": "Inventory",
        "action": "Checked availability",
        "duration": "0.2s"
      },
      {
        "agent": "Location",
        "action": "Found nearest stores",
        "duration": "0.1s"
      }
    ]
  },
  "results": [
    {
      "product": {...},
      "inventory": {...},
      "stores": [...]
    }
  ],
  "totalExecutionTime": "1.1s"
}
```

### Orchestrate Multi-Agent (Agent Framework)

```http
POST /api/multiagent/agentfx/orchestrate
```

Same request/response format as SK endpoint.

## Agent Catalog API

### List All Agents

```http
GET /api/agentcatalog
```

**Response:**
```json
{
  "agents": [
    {
      "id": "photo-analyzer",
      "name": "Photo Analyzer",
      "description": "Analyzes product images",
      "capabilities": ["image-analysis", "product-identification"],
      "status": "active",
      "version": "1.0"
    },
    {
      "id": "product-search",
      "name": "Product Search",
      "description": "Semantic product search",
      "capabilities": ["search", "vector-search", "nlp"],
      "status": "active",
      "version": "1.0"
    }
  ],
  "total": 8
}
```

### Get Agent Details

```http
GET /api/agentcatalog/{agentId}
```

**Response:**
```json
{
  "id": "product-search",
  "name": "Product Search",
  "description": "Semantic product search using AI",
  "capabilities": ["search", "vector-search", "nlp"],
  "tools": [
    {
      "name": "semantic_search",
      "description": "Perform semantic product search"
    },
    {
      "name": "vector_search",
      "description": "Vector similarity search"
    }
  ],
  "status": "active",
  "version": "1.0",
  "endpoint": "/api/productsearch",
  "documentation": "https://..."
}
```

### Get Agent Capabilities

```http
GET /api/agentcatalog/capabilities
```

**Response:**
```json
{
  "capabilities": [
    {
      "name": "image-analysis",
      "agents": ["photo-analyzer"],
      "description": "Analyze images and identify products"
    },
    {
      "name": "search",
      "agents": ["product-search"],
      "description": "Search products using natural language"
    }
  ]
}
```

## Analyze Photo Service API

### Analyze Image

```http
POST /api/analyzephoto/analyze
```

**Request:**
```json
{
  "image": "base64-encoded-image-data",
  "includeAttributes": true,
  "findSimilar": true
}
```

**Response:**
```json
{
  "analysis": {
    "detectedObjects": ["drill", "battery", "case"],
    "colors": ["red", "black"],
    "attributes": {
      "category": "power-tools",
      "type": "drill",
      "brand": "detected-brand"
    }
  },
  "identifiedProduct": {
    "productId": "1",
    "confidence": 0.92
  },
  "similarProducts": ["2", "3", "4"],
  "processingTime": "0.5s"
}
```

### Identify Product

```http
POST /api/analyzephoto/identify
```

**Request:**
```json
{
  "image": "base64-encoded-image-data"
}
```

**Response:**
```json
{
  "productId": "1",
  "productName": "18V Cordless Drill",
  "confidence": 0.95,
  "matchedFeatures": ["color", "shape", "brand-logo"]
}
```

## Customer Information Service API

### Get Customer Profile

```http
GET /api/customerinfo/{customerId}
```

**Response:**
```json
{
  "customerId": "cust-123",
  "name": "John Doe",
  "email": "john.doe@example.com",
  "memberSince": "2023-01-15",
  "tier": "gold",
  "preferences": {
    "categories": ["tools", "hardware"],
    "brands": ["DeWalt", "Milwaukee"],
    "notifications": true
  }
}
```

### Get Customer Preferences

```http
GET /api/customerinfo/{customerId}/preferences
```

**Response:**
```json
{
  "customerId": "cust-123",
  "preferences": {
    "favoriteCategories": ["tools", "hardware"],
    "favoriteBrands": ["DeWalt", "Milwaukee"],
    "priceRange": {
      "min": 0,
      "max": 500
    },
    "notifications": {
      "email": true,
      "sms": false,
      "push": true
    }
  }
}
```

### Get Purchase History

```http
GET /api/customerinfo/{customerId}/history
```

**Query Parameters:**
- `page` (integer): Page number
- `pageSize` (integer): Items per page
- `startDate` (date): Filter from date
- `endDate` (date): Filter to date

**Response:**
```json
{
  "customerId": "cust-123",
  "purchases": [
    {
      "orderId": "order-456",
      "date": "2024-10-15",
      "items": [
        {
          "productId": "1",
          "productName": "18V Cordless Drill",
          "quantity": 1,
          "price": 129.99
        }
      ],
      "total": 129.99
    }
  ],
  "totalOrders": 15,
  "totalSpent": 2456.78
}
```

## Inventory Service API

### Check Product Inventory

```http
GET /api/inventory/{productId}
```

**Response:**
```json
{
  "productId": "1",
  "available": true,
  "totalQuantity": 125,
  "stores": [
    {
      "storeId": "store-1",
      "storeName": "Downtown Store",
      "quantity": 45,
      "reservable": true
    },
    {
      "storeId": "store-2",
      "storeName": "North Store",
      "quantity": 80,
      "reservable": true
    }
  ],
  "lastUpdated": "2024-10-23T10:30:00Z"
}
```

### Get Available Products

```http
GET /api/inventory/available
```

**Query Parameters:**
- `storeId` (string): Filter by store
- `category` (string): Filter by category

**Response:**
```json
{
  "products": [
    {
      "productId": "1",
      "quantity": 45,
      "status": "in-stock"
    }
  ],
  "store": "store-1",
  "total": 150
}
```

### Get Store Inventory

```http
GET /api/inventory/store/{storeId}
```

**Response:**
```json
{
  "storeId": "store-1",
  "storeName": "Downtown Store",
  "inventory": [
    {
      "productId": "1",
      "productName": "18V Cordless Drill",
      "quantity": 45,
      "location": {
        "aisle": "12",
        "section": "B",
        "shelf": "3"
      }
    }
  ],
  "totalProducts": 500,
  "lastUpdated": "2024-10-23T10:30:00Z"
}
```

## Location Service API

### List All Stores

```http
GET /api/location/stores
```

**Response:**
```json
{
  "stores": [
    {
      "storeId": "store-1",
      "name": "Downtown Store",
      "address": "123 Main St, Seattle, WA 98101",
      "coordinates": {
        "latitude": 47.6062,
        "longitude": -122.3321
      },
      "phone": "555-0100",
      "hours": "Mon-Sat 9AM-9PM, Sun 10AM-6PM"
    }
  ],
  "total": 10
}
```

### Find Nearest Store

```http
GET /api/location/nearest?lat={latitude}&lon={longitude}
```

**Query Parameters:**
- `lat` (decimal, required): Latitude
- `lon` (decimal, required): Longitude
- `maxDistance` (decimal): Maximum distance in miles (default: 25)
- `limit` (integer): Max results (default: 5)

**Response:**
```json
{
  "nearestStores": [
    {
      "storeId": "store-1",
      "name": "Downtown Store",
      "distance": 2.5,
      "distanceUnit": "miles",
      "address": "123 Main St, Seattle, WA 98101",
      "estimatedDriveTime": "8 minutes"
    }
  ],
  "searchLocation": {
    "latitude": 47.6062,
    "longitude": -122.3321
  }
}
```

### Get Store Details

```http
GET /api/location/store/{storeId}
```

**Response:**
```json
{
  "storeId": "store-1",
  "name": "Downtown Store",
  "address": {
    "street": "123 Main St",
    "city": "Seattle",
    "state": "WA",
    "zipCode": "98101",
    "country": "USA"
  },
  "coordinates": {
    "latitude": 47.6062,
    "longitude": -122.3321
  },
  "contact": {
    "phone": "555-0100",
    "email": "downtown@store.com"
  },
  "hours": {
    "monday": "9AM-9PM",
    "tuesday": "9AM-9PM",
    "wednesday": "9AM-9PM",
    "thursday": "9AM-9PM",
    "friday": "9AM-9PM",
    "saturday": "9AM-9PM",
    "sunday": "10AM-6PM"
  },
  "services": ["pickup", "delivery", "returns"]
}
```

## Matchmaking Service API

### Get Recommendations

```http
POST /api/matchmaking/recommend
```

**Request:**
```json
{
  "customerId": "cust-123",
  "context": {
    "currentProduct": "1",
    "recentViews": ["2", "3"],
    "recentPurchases": ["4", "5"]
  },
  "limit": 10
}
```

**Response:**
```json
{
  "recommendations": [
    {
      "productId": "6",
      "productName": "Battery Pack 18V",
      "score": 0.92,
      "reason": "Frequently bought together",
      "category": "accessories"
    },
    {
      "productId": "7",
      "productName": "Drill Bit Set",
      "score": 0.88,
      "reason": "Complements your purchase",
      "category": "accessories"
    }
  ],
  "algorithm": "collaborative-filtering",
  "confidence": 0.85
}
```

### Find Similar Products

```http
POST /api/matchmaking/similar/{productId}
```

**Query Parameters:**
- `limit` (integer): Max results (default: 10)

**Response:**
```json
{
  "productId": "1",
  "similarProducts": [
    {
      "productId": "8",
      "productName": "20V Cordless Drill",
      "similarityScore": 0.95,
      "reasons": ["same-category", "similar-price", "same-brand"]
    }
  ]
}
```

### Get Personalized Suggestions

```http
POST /api/matchmaking/personalized/{customerId}
```

**Request:**
```json
{
  "context": "browsing",
  "filters": {
    "categories": ["tools"],
    "priceMax": 200
  },
  "limit": 10
}
```

**Response:**
```json
{
  "customerId": "cust-123",
  "suggestions": [
    {
      "productId": "9",
      "score": 0.90,
      "reason": "Based on your purchase history"
    }
  ]
}
```

## Navigation Service API

### Locate Product

```http
GET /api/navigation/locate/{productId}
```

**Query Parameters:**
- `storeId` (string, required): Store ID

**Response:**
```json
{
  "productId": "1",
  "productName": "18V Cordless Drill",
  "location": {
    "storeId": "store-1",
    "aisle": "12",
    "section": "B",
    "shelf": "3",
    "description": "Aisle 12, Section B, Shelf 3"
  },
  "available": true,
  "quantity": 45
}
```

### Calculate Route

```http
POST /api/navigation/route
```

**Request:**
```json
{
  "storeId": "store-1",
  "startLocation": "entrance",
  "destinations": [
    {"productId": "1"},
    {"productId": "2"},
    {"productId": "3"}
  ],
  "optimize": true
}
```

**Response:**
```json
{
  "route": {
    "totalDistance": 450,
    "estimatedTime": "5 minutes",
    "waypoints": [
      {
        "sequence": 1,
        "location": "entrance",
        "action": "Start here"
      },
      {
        "sequence": 2,
        "location": "aisle-5",
        "productId": "2",
        "action": "Pick up Hammer"
      },
      {
        "sequence": 3,
        "location": "aisle-12",
        "productId": "1",
        "action": "Pick up Cordless Drill"
      },
      {
        "sequence": 4,
        "location": "aisle-15",
        "productId": "3",
        "action": "Pick up Paint"
      }
    ],
    "optimized": true
  }
}
```

### Get Aisle Information

```http
GET /api/navigation/aisle/{aisleId}
```

**Query Parameters:**
- `storeId` (string, required): Store ID

**Response:**
```json
{
  "aisleId": "12",
  "storeId": "store-1",
  "sections": ["A", "B", "C", "D"],
  "categories": ["Power Tools", "Hand Tools"],
  "products": [
    {
      "productId": "1",
      "section": "B",
      "shelf": "3"
    }
  ]
}
```

## Product Search Service API

### Search Products

```http
POST /api/productsearch/search
```

**Request:**
```json
{
  "query": "red power drill with battery under $200",
  "filters": {
    "category": "tools",
    "priceMin": 0,
    "priceMax": 200,
    "inStockOnly": true
  },
  "options": {
    "useSemanticSearch": true,
    "includeRecommendations": true,
    "top": 10
  }
}
```

**Response:**
```json
{
  "query": "red power drill with battery under $200",
  "results": [
    {
      "productId": "1",
      "name": "18V Cordless Drill with Battery",
      "price": 129.99,
      "relevanceScore": 0.95,
      "highlights": {
        "name": "18V <em>Cordless Drill</em> with <em>Battery</em>",
        "description": "<em>Red</em> professional power drill..."
      },
      "inStock": true
    }
  ],
  "total": 5,
  "executionTime": "0.3s",
  "searchType": "semantic"
}
```

### Vector Search

```http
POST /api/productsearch/vector
```

**Request:**
```json
{
  "query": "power drill",
  "vectorOptions": {
    "useEmbeddings": true,
    "threshold": 0.7
  },
  "top": 10
}
```

**Response:**
```json
{
  "results": [
    {
      "productId": "1",
      "similarityScore": 0.92
    }
  ],
  "total": 8,
  "vectorsCompared": 1500
}
```

### Get Search Suggestions

```http
GET /api/productsearch/suggest?q={query}
```

**Query Parameters:**
- `q` (string, required): Partial query
- `limit` (integer): Max suggestions (default: 5)

**Response:**
```json
{
  "query": "power",
  "suggestions": [
    "power drill",
    "power saw",
    "power tools",
    "power sander",
    "power washer"
  ]
}
```

## Tool Reasoning Service API

### Perform Reasoning

```http
POST /api/toolreasoning/reason
```

**Request:**
```json
{
  "query": "I need to build a deck. What tools do I need?",
  "context": {
    "customerId": "cust-123",
    "budget": 500,
    "skillLevel": "intermediate"
  }
}
```

**Response:**
```json
{
  "decision": "recommend-tool-package",
  "reasoning": "Based on deck building requirements, you need measuring, cutting, fastening, and finishing tools",
  "confidence": 0.88,
  "recommendations": [
    {
      "productId": "1",
      "productName": "Cordless Drill",
      "reason": "Essential for fastening deck boards",
      "priority": 1
    },
    {
      "productId": "10",
      "productName": "Circular Saw",
      "reason": "For cutting deck boards to size",
      "priority": 1
    }
  ],
  "steps": [
    {
      "step": 1,
      "action": "Measure and mark",
      "tools": ["tape measure", "pencil"]
    },
    {
      "step": 2,
      "action": "Cut boards",
      "tools": ["circular saw"]
    },
    {
      "step": 3,
      "action": "Fasten boards",
      "tools": ["cordless drill", "screws"]
    }
  ],
  "estimatedCost": 450,
  "toolsUsed": ["product-search", "inventory", "matchmaking"]
}
```

### Create Action Plan

```http
POST /api/toolreasoning/plan
```

**Request:**
```json
{
  "goal": "Find and purchase deck building materials",
  "constraints": {
    "budget": 1000,
    "timeline": "this weekend",
    "location": "Seattle, WA"
  }
}
```

**Response:**
```json
{
  "plan": {
    "goal": "Find and purchase deck building materials",
    "steps": [
      {
        "step": 1,
        "action": "Search for deck materials",
        "tool": "product-search",
        "estimated Time": "5 minutes"
      },
      {
        "step": 2,
        "action": "Check inventory",
        "tool": "inventory",
        "estimatedTime": "2 minutes"
      },
      {
        "step": 3,
        "action": "Find nearest store",
        "tool": "location",
        "estimatedTime": "1 minute"
      },
      {
        "step": 4,
        "action": "Calculate route",
        "tool": "navigation",
        "estimatedTime": "1 minute"
      }
    ],
    "estimatedTotalTime": "9 minutes",
    "feasible": true
  }
}
```

### Make Decision

```http
POST /api/toolreasoning/decide
```

**Request:**
```json
{
  "question": "Should I buy now or wait for a sale?",
  "context": {
    "productId": "1",
    "currentPrice": 129.99,
    "historicalPrices": [139.99, 119.99, 129.99],
    "urgency": "low"
  }
}
```

**Response:**
```json
{
  "decision": "wait",
  "reasoning": "Historical data shows this product goes on sale approximately every 6 weeks. Last sale was 4 weeks ago. Given low urgency, waiting 2-3 weeks may result in 15-20% savings.",
  "confidence": 0.75,
  "alternatives": [
    {
      "action": "buy-now",
      "reason": "Guaranteed availability",
      "score": 0.60
    },
    {
      "action": "set-price-alert",
      "reason": "Get notified when price drops",
      "score": 0.85
    }
  ],
  "recommendedAction": "set-price-alert"
}
```

## Error Responses

All endpoints may return error responses in this format:

```json
{
  "error": {
    "code": "ERROR_CODE",
    "message": "Human-readable error message",
    "details": "Additional details about the error",
    "correlationId": "7b3f8a2c-9e1d-4f6b-a5c3-8d9e2f1a3b4c",
    "timestamp": "2024-10-23T10:30:00Z",
    "path": "/api/products/123"
  }
}
```

### Common Error Codes

| Code | Description |
|------|-------------|
| `INVALID_REQUEST` | Request validation failed |
| `RESOURCE_NOT_FOUND` | Requested resource doesn't exist |
| `SERVICE_UNAVAILABLE` | Service temporarily unavailable |
| `RATE_LIMIT_EXCEEDED` | Too many requests |
| `AI_SERVICE_ERROR` | Azure AI Foundry error |
| `AUTHENTICATION_REQUIRED` | Auth token missing or invalid |
| `INSUFFICIENT_PERMISSIONS` | User lacks required permissions |
| `INTERNAL_ERROR` | Unexpected server error |

## Rate Limits

| Tier | Requests/Minute | Burst |
|------|----------------|-------|
| Anonymous | 60 | 10 |
| Authenticated | 600 | 100 |
| Premium | 6000 | 1000 |

## Next Steps

- [Data Flow](05-data-flow.md) - Understanding data models
- [Developer Guide](08-developer-guide.md) - Implementation details
