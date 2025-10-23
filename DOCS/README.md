# Comprehensive Documentation - BRK445

**Building Enterprise-Ready AI Agents with Azure AI Foundry**

Welcome to the comprehensive documentation for the BRK445 session at Microsoft AI Tour 2026. This documentation provides detailed explanations of the application architecture, components, services, and their interactions.

## 📖 Documentation Available in Multiple Languages

Choose your preferred language:

### 🇺🇸 English Documentation
**[English Documentation →](en/README.md)**

Complete documentation in English including:
- Architecture overview
- Component descriptions
- Service interactions
- API reference
- Data flow patterns
- Deployment guide
- Getting started guide
- Developer guide

### 🇪🇸 Documentación en Español
**[Documentación en Español →](es/README.md)**

Documentación completa en español incluyendo:
- Descripción general de la arquitectura
- Descripciones de componentes
- Interacciones de servicios
- Referencia de API
- Patrones de flujo de datos
- Guía de despliegue
- Guía de inicio
- Guía del desarrollador

## 📚 What's Included

This documentation covers:

### 🏗️ Architecture
- **System Architecture**: High-level overview of the entire application
- **Component Architecture**: Detailed breakdown of each service and component
- **Deployment Architecture**: Production infrastructure and Azure resources
- **Data Architecture**: Data models, flow, and storage

### 🔧 Development
- **Getting Started**: Step-by-step setup instructions
- **Developer Guide**: Best practices and coding standards
- **API Reference**: Complete API documentation for all services
- **Service Interactions**: How components communicate

### 🚀 Deployment
- **Azure Resources**: Required cloud resources
- **Configuration**: Environment variables and secrets
- **CI/CD**: Automated deployment pipelines
- **Monitoring**: Observability and diagnostics

### 🤖 AI Agents
- **Agent Frameworks**: Semantic Kernel and Microsoft Agent Framework
- **Agent Catalog**: Available agents and their capabilities
- **Orchestration Patterns**: Single and multi-agent workflows
- **Integration**: Azure AI Foundry integration

## 🎯 Quick Start

### For New Users
1. Read the [Architecture Overview](en/01-architecture-overview.md)
2. Follow the [Getting Started Guide](en/07-getting-started.md)
3. Explore [Application Components](en/02-application-components.md)

### Para Nuevos Usuarios
1. Lee la [Descripción General de la Arquitectura](es/01-architecture-overview.md)
2. Sigue la [Guía de Inicio](es/07-getting-started.md)
3. Explora los [Componentes de la Aplicación](es/02-application-components.md)

## 📊 Visual Documentation

The documentation includes:
- ✅ **Mermaid Diagrams**: Interactive architecture and sequence diagrams
- ✅ **Flow Charts**: Data flow and process visualizations
- ✅ **Component Diagrams**: Service relationships and dependencies
- ✅ **Sequence Diagrams**: Request/response flows

## 🔗 Related Resources

### Repository Resources
- [Main README](../README.MD) - Repository overview
- [Source Code](../src/) - Application source code
- [Session Delivery Resources](../session-delivery-resources/) - Presentation materials

### External Resources
- [Azure AI Foundry Documentation](https://learn.microsoft.com/azure/ai-studio/)
- [.NET Aspire Documentation](https://learn.microsoft.com/dotnet/aspire/)
- [Semantic Kernel Documentation](https://learn.microsoft.com/semantic-kernel/)
- [Azure OpenAI Documentation](https://learn.microsoft.com/azure/ai-services/openai/)

## 📋 Documentation Structure

```
DOCS/
├── README.md (this file)
│
├── en/ (English Documentation)
│   ├── README.md
│   ├── 01-architecture-overview.md
│   ├── 02-application-components.md
│   ├── 03-service-interactions.md
│   ├── 04-api-reference.md
│   ├── 05-data-flow.md
│   ├── 06-deployment-architecture.md
│   ├── 07-getting-started.md
│   ├── 08-developer-guide.md
│   ├── diagrams/
│   └── images/
│
└── es/ (Spanish Documentation)
    ├── README.md
    ├── 01-architecture-overview.md
    ├── 02-application-components.md
    ├── 03-service-interactions.md
    ├── 04-api-reference.md
    ├── 05-data-flow.md
    ├── 06-deployment-architecture.md
    ├── 07-getting-started.md
    ├── 08-developer-guide.md
    ├── diagrams/
    └── images/
```

## 🎓 Learning Path

### Beginner Path
1. **Understand the Architecture**
   - [Architecture Overview](en/01-architecture-overview.md)
   - [Application Components](en/02-application-components.md)

2. **Set Up Your Environment**
   - [Getting Started](en/07-getting-started.md)

3. **Run the Application**
   - Follow the setup instructions
   - Test the demos

### Advanced Path
1. **Deep Dive into Services**
   - [Service Interactions](en/03-service-interactions.md)
   - [Data Flow](en/05-data-flow.md)

2. **Development**
   - [Developer Guide](en/08-developer-guide.md)
   - [API Reference](en/04-api-reference.md)

3. **Deployment**
   - [Deployment Architecture](en/06-deployment-architecture.md)

## 🛠️ Technology Stack

The application uses:
- **.NET 9** - Backend framework
- **Blazor Server** - Frontend UI
- **.NET Aspire** - Service orchestration
- **Azure AI Foundry** - AI agent platform
- **Azure OpenAI** - LLM models
- **SQL Server** - Data storage
- **Docker** - Containerization
- **Application Insights** - Monitoring

## 💡 Key Concepts

### Multi-Agent Architecture
The application demonstrates how to:
- Design agent architectures separating reasoning, tools, and orchestration
- Implement sequential, concurrent, and handoff orchestration patterns
- Integrate Azure AI Foundry with local microservices

### Enterprise Patterns
Learn about:
- Service discovery and configuration
- Distributed tracing and observability
- Error handling and retry policies
- Security and authentication
- Scalability and performance

### Agent Frameworks
The application supports:
- **Semantic Kernel** - Microsoft's mature agent framework
- **Microsoft Agent Framework** - New generation framework

Both frameworks can be switched via the UI without code changes.

## 📝 Contributing to Documentation

Found an issue or want to improve the documentation?

1. Documentation is written in Markdown
2. Diagrams use Mermaid syntax
3. Follow the existing structure and style
4. Provide translations for both English and Spanish

## 📧 Support

For questions or support:
- Check the documentation in your preferred language
- Review the [Session Delivery Resources](../session-delivery-resources/)
- See [SUPPORT.md](../SUPPORT.md) in the main repository

## 📅 Version Information

- **Documentation Version**: 1.0
- **Last Updated**: October 2025
- **Target Audience**: Developers, Architects, AI Engineers
- **Prerequisites**: .NET 9, Docker, Azure Subscription

## 🌟 What Makes This Documentation Special

✅ **Bilingual**: Available in English and Spanish  
✅ **Comprehensive**: Covers architecture, development, and deployment  
✅ **Visual**: Rich diagrams and charts  
✅ **Practical**: Step-by-step guides and examples  
✅ **Up-to-date**: Reflects latest patterns and technologies  
✅ **Accessible**: Clear structure and navigation  

---

**Ready to get started?**

- 🇺🇸 **English**: [Start with the English documentation →](en/README.md)
- 🇪🇸 **Español**: [Comienza con la documentación en español →](es/README.md)

**Happy Learning! / ¡Feliz Aprendizaje!** 🎉
