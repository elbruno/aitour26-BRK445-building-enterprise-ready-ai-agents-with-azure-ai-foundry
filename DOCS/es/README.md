# BRK445 - Construcción de Agentes de IA Listos para Empresas con Azure AI Foundry

## Descripción General de la Documentación

Bienvenido a la documentación completa de la sesión BRK445 del AI Tour 2026. Esta documentación proporciona información detallada sobre la arquitectura de la aplicación, componentes, servicios y sus interacciones.

## 📚 Estructura de la Documentación

- **[Descripción General de la Arquitectura](01-architecture-overview.md)** - Arquitectura del sistema de alto nivel y patrones de diseño
- **[Componentes de la Aplicación](02-application-components.md)** - Descripción detallada de todos los componentes de la aplicación
- **[Interacciones de Servicios](03-service-interactions.md)** - Cómo los servicios se comunican entre sí
- **[Referencia de API](04-api-reference.md)** - Documentación completa de la API para todos los servicios
- **[Flujo de Datos](05-data-flow.md)** - Flujo de datos entre componentes y servicios
- **[Arquitectura de Despliegue](06-deployment-architecture.md)** - Despliegue en la nube e infraestructura
- **[Guía de Inicio](07-getting-started.md)** - Guía de inicio rápido para desarrolladores
- **[Guía del Desarrollador](08-developer-guide.md)** - Directrices detalladas de desarrollo

## 🎯 Lo Que Aprenderás

Esta aplicación demuestra:

1. **Arquitectura Multi-Agente** - Patrones de diseño para orquestar múltiples agentes de IA
2. **Integración con Azure AI Foundry** - Conectar servicios de IA en la nube con microservicios locales
3. **Orquestación de Servicios** - Usar .NET Aspire para gestionar servicios distribuidos
4. **Frameworks de Agentes** - Integración con Semantic Kernel y Microsoft Agent Framework
5. **Observabilidad** - Monitoreo y rastreo de interacciones de agentes de IA
6. **Patrones Empresariales** - Mejores prácticas de seguridad, gobernanza y confiabilidad

## 🏗️ Arquitectura de un Vistazo

La aplicación consta de:

- **Frontend**: Aplicación Store basada en Blazor
- **Servicios Backend**: Más de 8 microservicios especializados para diferentes capacidades de agentes
- **Agentes de IA**: Agentes de Azure AI Foundry para varias tareas (inventario, información del cliente, navegación, etc.)
- **Orquestación**: .NET Aspire para gestión de servicios y configuración
- **Almacenamiento de Datos**: SQL Server para datos de productos
- **Monitoreo**: Application Insights para observabilidad

## 🚀 Enlaces Rápidos

- [README Principal del Repositorio](../../README.MD)
- [Recursos de Entrega de Sesión](../../session-delivery-resources/readme.md)
- [Código Fuente](../../src/)

## 📖 Para Nuevos Usuarios

Si eres nuevo en esta aplicación, recomendamos leer la documentación en el siguiente orden:

1. Comienza con [Descripción General de la Arquitectura](01-architecture-overview.md) para entender el panorama general
2. Revisa [Componentes de la Aplicación](02-application-components.md) para conocer cada servicio
3. Estudia [Interacciones de Servicios](03-service-interactions.md) para comprender cómo trabajan juntos los componentes
4. Sigue [Guía de Inicio](07-getting-started.md) para configurar tu entorno de desarrollo
5. Usa [Guía del Desarrollador](08-developer-guide.md) para instrucciones detalladas de desarrollo

## 🌍 Soporte de Idiomas

Esta documentación está disponible en:
- 🇺🇸 **Inglés** - `/DOCS/en/`
- 🇪🇸 **Español** - `/DOCS/es/`

## 📝 Contribución

¿Encontraste un problema con la documentación? Por favor, consulta las directrices de contribución del repositorio principal.

## 📧 Soporte

Para preguntas o soporte, por favor consulta el archivo [SUPPORT.md](../../SUPPORT.md) en el repositorio principal.

---

**Última Actualización**: Octubre 2025  
**Versión**: 1.0  
**Audiencia Objetivo**: Desarrolladores, Arquitectos, Ingenieros de IA
