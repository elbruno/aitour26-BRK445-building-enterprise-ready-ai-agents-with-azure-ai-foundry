# Guía de Inicio

Esta guía te ayudará a configurar tu entorno de desarrollo y ejecutar la aplicación localmente.

## Requisitos Previos

### Herramientas Requeridas

| Herramienta | Versión | Propósito |
|------|---------|---------|
| **.NET SDK** | 9.0+ | Desarrollo backend |
| **Docker Desktop** | Última | Tiempo de ejecución de contenedores |
| **.NET Aspire Workload** | Última | Orquestación de servicios |
| **Azure CLI** | Última | Gestión de Azure |
| **Git** | Última | Control de versiones |
| **Visual Studio Code** o **Visual Studio 2022** | Última | IDE |

### Requisitos de Azure

- **Suscripción de Azure** con permisos para crear recursos
- **Acceso a Azure AI Foundry** (inscripción en vista previa)
- **Acceso a Azure OpenAI** (suscripción aprobada)

## Pasos de Instalación

### 1. Instalar .NET 9 SDK

**Windows**:
```powershell
winget install Microsoft.DotNet.SDK.9
```

**macOS**:
```bash
brew install dotnet@9
```

**Linux**:
```bash
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 9.0
```

**Verificar instalación**:
```bash
dotnet --version
# Debería mostrar: 9.0.x
```

### 2. Instalar .NET Aspire Workload

```bash
dotnet workload update
dotnet workload install aspire
```

### 3. Instalar Docker Desktop

Descargar e instalar desde: https://www.docker.com/products/docker-desktop/

**Iniciar Docker Desktop** y asegurarse de que esté ejecutándose.

### 4. Instalar Azure CLI

**Windows**:
```powershell
winget install -e --id Microsoft.AzureCLI
```

**macOS**:
```bash
brew install azure-cli
```

**Linux**:
```bash
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
```

**Iniciar sesión en Azure**:
```bash
az login
az account set --subscription <tu-id-de-suscripción>
```

## Configuración del Repositorio

### 1. Clonar el Repositorio

```bash
git clone https://github.com/elbruno/aitour26-BRK445-building-enterprise-ready-ai-agents-with-azure-ai-foundry.git
cd aitour26-BRK445-building-enterprise-ready-ai-agents-with-azure-ai-foundry
```

### 2. Explorar la Estructura del Repositorio

```
├── src/                    # Código fuente
│   ├── ZavaAppHost/       # Orquestador Aspire
│   ├── Store/             # Aplicación frontend
│   ├── Products/          # Servicio de productos
│   ├── SingleAgentDemo/   # Demos de agente único
│   ├── MultiAgentDemo/    # Demos multi-agente
│   └── [8 microservicios] # Servicios especializados
├── docs/                   # Documentación
├── DOCS/                   # Documentación completa
├── data/                   # Datos de ejemplo
└── README.MD              # Readme principal
```

## Configuración de Recursos de Azure

### Opción 1: Despliegue Automatizado (Recomendado)

El script automatizado crea todos los recursos de Azure requeridos.

**Linux/macOS**:
```bash
./deploy.sh
```

**Windows PowerShell**:
```powershell
.\deploy.ps1
```

El script:
1. ✅ Crea recurso de Azure AI Foundry
2. ✅ Despliega modelo GPT-4o-mini
3. ✅ Despliega modelo text-embedding-ada-002
4. ✅ Crea Application Insights
5. ✅ Configura agentes en AI Foundry
6. ✅ Proporciona cadenas de conexión

**Guardar la salida** - necesitarás las cadenas de conexión para desarrollo local.

### Opción 2: Configuración Manual

Sigue la guía detallada en [session-delivery-resources/docs/02.NeededCloudResources.md](../../session-delivery-resources/docs/02.NeededCloudResources.md)

## Configuración Local

### 1. Configurar User Secrets

Los user secrets mantienen los datos sensibles fuera del control de versiones.

**Navegar a AppHost**:
```bash
cd src/ZavaAppHost
```

**Inicializar secrets**:
```bash
dotnet user-secrets init
```

**Configurar conexión de Azure AI Foundry**:
```bash
dotnet user-secrets set "ConnectionStrings:aifoundry" "<tu-cadena-de-conexión>"
```

**Configurar proyecto de AI Foundry**:
```bash
dotnet user-secrets set "ConnectionStrings:aifoundryproject" "<tu-id-de-proyecto>"
```

**Configurar Application Insights**:
```bash
dotnet user-secrets set "ConnectionStrings:appinsights" "<tu-cadena-de-conexión-appinsights>"
```

**Configurar IDs de Agentes**:
```bash
dotnet user-secrets set "ConnectionStrings:photoanalyzeragentid" "<id-del-agente>"
dotnet user-secrets set "ConnectionStrings:customerinformationagentid" "<id-del-agente>"
dotnet user-secrets set "ConnectionStrings:inventoryagentid" "<id-del-agente>"
dotnet user-secrets set "ConnectionStrings:locationserviceagentid" "<id-del-agente>"
dotnet user-secrets set "ConnectionStrings:navigationagentid" "<id-del-agente>"
dotnet user-secrets set "ConnectionStrings:productmatchmakingagentid" "<id-del-agente>"
dotnet user-secrets set "ConnectionStrings:productsearchagentid" "<id-del-agente>"
dotnet user-secrets set "ConnectionStrings:toolreasoningagentid" "<id-del-agente>"
```

**Verificar secrets**:
```bash
dotnet user-secrets list
```

## Ejecutar la Aplicación

### 1. Abrir en IDE

**Visual Studio Code**:
```bash
code .
```

**Visual Studio 2022**:
```bash
start src/Zava-Aspire.slnx
```

### 2. Iniciar con .NET Aspire

**Desde IDE**:
- Abrir `src/ZavaAppHost/Program.cs`
- Presionar F5 o hacer clic en "Run"

**Desde Línea de Comandos**:
```bash
cd src/ZavaAppHost
dotnet run
```

### 3. Acceder al Dashboard de Aspire

El dashboard de Aspire se abre automáticamente en: https://localhost:17001

El dashboard muestra:
- **Resources**: Todos los servicios en ejecución
- **Console Logs**: Logs en tiempo real de todos los servicios
- **Traces**: Rastreo distribuido
- **Metrics**: Métricas de rendimiento
- **Environment Variables**: Configuración

### 4. Acceder a la Aplicación

Una vez que todos los servicios estén saludables (marcas verdes en el dashboard de Aspire):

**Frontend de Store**: https://localhost:7001

La aplicación Store proporciona:
- Catálogo de productos
- Demo de agente único
- Demo multi-agente
- Catálogo de agentes
- Configuración (selección de framework)

### 5. Verificar Servicios

Verificar que todos los servicios estén ejecutándose:

| Servicio | URL | Verificación de Estado |
|---------|-----|--------------|
| Store | https://localhost:7001 | Abrir en navegador |
| Products | https://localhost:7002/health | Debería devolver "Healthy" |
| SingleAgent | https://localhost:7003/health | Debería devolver "Healthy" |
| MultiAgent | https://localhost:7004/health | Debería devolver "Healthy" |

## Probar la Aplicación

### 1. Probar Catálogo de Productos

1. Navegar a https://localhost:7001
2. Explorar productos
3. Buscar "drill"
4. Hacer clic en un producto para ver detalles

### 2. Probar Demo de Agente Único

1. Hacer clic en "Single Agent Demo" en navegación
2. Ingresar consulta: "Find me a red power drill under $200"
3. Hacer clic en "Execute"
4. Ver resultados con información de inventario

### 3. Probar Demo Multi-Agente

1. Hacer clic en "Multi-Agent Demo" en navegación
2. Subir una imagen de producto o ingresar descripción
3. Agregar ubicación (opcional)
4. Hacer clic en "Orchestrate"
5. Ver ejecución del flujo de trabajo y resultados

### 4. Cambiar Frameworks de Agentes

1. Hacer clic en "Settings" en navegación
2. Alternar entre Semantic Kernel y Agent Framework
3. Volver a las demos y verificar que ambos frameworks funcionen

## Problemas Comunes y Soluciones

### Problema: Docker no está ejecutándose

**Error**: "Cannot connect to Docker daemon"

**Solución**:
1. Iniciar Docker Desktop
2. Esperar a que Docker se inicie completamente
3. Reintentar ejecutar la aplicación

### Problema: Puerto ya en uso

**Error**: "Address already in use"

**Solución**:
```bash
# Encontrar proceso usando puerto 7001
lsof -i :7001  # macOS/Linux
netstat -ano | findstr :7001  # Windows

# Matar el proceso
kill <PID>  # macOS/Linux
taskkill /PID <PID> /F  # Windows
```

### Problema: User secrets no encontrados

**Error**: "ConnectionString 'aifoundry' not found"

**Solución**:
1. Verificar que estás en el directorio ZavaAppHost
2. Ejecutar `dotnet user-secrets list`
3. Re-agregar secrets faltantes
4. Reiniciar aplicación

### Problema: Errores de compilación

**Error**: "Project file not found" o "SDK not found"

**Solución**:
```bash
# Limpiar solución
dotnet clean

# Restaurar paquetes
dotnet restore

# Recompilar
dotnet build
```

## Flujo de Trabajo de Desarrollo

### 1. Hacer Cambios en el Código

Editar archivos en tu IDE. Aspire detectará los cambios automáticamente.

### 2. Hot Reload

Para cambios menores:
- Los cambios se recargan automáticamente
- No se necesita reinicio
- Verificar mensajes de recarga en consola

Para cambios mayores:
- Detener la aplicación (Ctrl+C)
- Reiniciar con `dotnet run`

### 3. Ver Logs

**En Dashboard de Aspire**:
- Hacer clic en un servicio
- Ver salida de consola en tiempo real
- Ver logs estructurados

### 4. Depurar

**Visual Studio Code**:
1. Establecer puntos de interrupción en el código
2. Presionar F5
3. Ejecutar ruta de código
4. Depurar en panel Variables

**Visual Studio 2022**:
1. Establecer puntos de interrupción en el código
2. Iniciar depuración (F5)
3. Paso a paso por el código (F10/F11)

## Próximos Pasos

Ahora que tienes la aplicación ejecutándose:

1. **Explorar el Código**: Leer [Guía del Desarrollador](08-developer-guide.md)
2. **Entender la Arquitectura**: Revisar [Descripción General de la Arquitectura](01-architecture-overview.md)
3. **Aprender las APIs**: Verificar [Referencia de API](04-api-reference.md)
4. **Desplegar en Azure**: Seguir [Arquitectura de Despliegue](06-deployment-architecture.md)

## Recursos Adicionales

- [Recursos de Entrega de Sesión](../../session-delivery-resources/readme.md)
- [Guía de Requisitos Previos](../../session-delivery-resources/docs/Prerequisites.md)
- [Cómo Ejecutar Demo Localmente](../../session-delivery-resources/docs/HowToRunDemoLocally.md)

## Obtener Ayuda

- **Problemas**: Abrir un issue en el repositorio de GitHub
- **Preguntas**: Verificar los [recursos de entrega de sesión](../../session-delivery-resources/)
- **Soporte**: Ver [SUPPORT.md](../../SUPPORT.md)

---

**¡Felicitaciones!** 🎉 Ahora tienes la aplicación ejecutándose localmente. ¡Feliz codificación!
