# Getting Started

This guide will help you set up your development environment and run the application locally.

## Prerequisites

### Required Tools

| Tool | Version | Purpose |
|------|---------|---------|
| **.NET SDK** | 9.0+ | Backend development |
| **Docker Desktop** | Latest | Container runtime |
| **.NET Aspire Workload** | Latest | Service orchestration |
| **Azure CLI** | Latest | Azure management |
| **Git** | Latest | Version control |
| **Visual Studio Code** or **Visual Studio 2022** | Latest | IDE |

### Azure Requirements

- **Azure Subscription** with permissions to create resources
- **Azure AI Foundry access** (preview enrollment)
- **Azure OpenAI access** (approved subscription)

### Optional Tools

- **Azure Developer CLI (azd)** - Simplified deployment
- **Python 3.9+** - For documentation tools (MkDocs)
- **Postman** or **Thunder Client** - API testing

## Installation Steps

### 1. Install .NET 9 SDK

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

**Verify installation**:
```bash
dotnet --version
# Should output: 9.0.x
```

### 2. Install .NET Aspire Workload

```bash
dotnet workload update
dotnet workload install aspire
```

**Verify installation**:
```bash
dotnet workload list
# Should show: aspire
```

### 3. Install Docker Desktop

Download and install from: https://www.docker.com/products/docker-desktop/

**Verify installation**:
```bash
docker --version
docker-compose --version
```

**Start Docker Desktop** and ensure it's running.

### 4. Install Azure CLI

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

**Login to Azure**:
```bash
az login
az account set --subscription <your-subscription-id>
```

### 5. Install Git

Download from: https://git-scm.com/downloads

**Verify installation**:
```bash
git --version
```

### 6. Install IDE

**Visual Studio Code**:
- Download: https://code.visualstudio.com/
- Install C# Dev Kit extension
- Install Docker extension

**Visual Studio 2022**:
- Download: https://visualstudio.microsoft.com/
- Select ".NET desktop development" workload
- Select "ASP.NET and web development" workload

## Repository Setup

### 1. Clone the Repository

```bash
git clone https://github.com/elbruno/aitour26-BRK445-building-enterprise-ready-ai-agents-with-azure-ai-foundry.git
cd aitour26-BRK445-building-enterprise-ready-ai-agents-with-azure-ai-foundry
```

### 2. Explore Repository Structure

```bash
ls -la
```

```
├── src/                    # Source code
│   ├── ZavaAppHost/       # Aspire orchestrator
│   ├── Store/             # Frontend application
│   ├── Products/          # Products service
│   ├── SingleAgentDemo/   # Single agent demos
│   ├── MultiAgentDemo/    # Multi-agent demos
│   └── [8 microservices]  # Specialized services
├── docs/                   # Documentation
├── DOCS/                   # Comprehensive docs
├── data/                   # Sample data
├── infra/                  # Infrastructure as code
├── session-delivery-resources/ # Session materials
├── deploy.sh              # Deployment script (Linux/Mac)
├── deploy.ps1             # Deployment script (Windows)
└── README.MD              # Main readme
```

## Azure Resource Setup

### Option 1: Automated Deployment (Recommended)

The automated script creates all required Azure resources.

**Linux/macOS**:
```bash
./deploy.sh
```

**Windows PowerShell**:
```powershell
.\deploy.ps1
```

The script will:
1. ✅ Create Azure AI Foundry resource
2. ✅ Deploy GPT-4o-mini model
3. ✅ Deploy text-embedding-ada-002 model
4. ✅ Create Application Insights
5. ✅ Set up agents in AI Foundry
6. ✅ Provide connection strings

**Save the output** - you'll need the connection strings for local development.

### Option 2: Manual Setup

Follow the detailed guide in [session-delivery-resources/docs/02.NeededCloudResources.md](../../session-delivery-resources/docs/02.NeededCloudResources.md)

#### Quick Manual Steps:

1. **Create Resource Group**:
```bash
az group create --name rg-aitour-brk445 --location eastus
```

2. **Create AI Foundry**:
- Go to https://ai.azure.com
- Create new project
- Note the project ID and endpoint

3. **Deploy Models**:
- Deploy gpt-4o-mini model
- Deploy text-embedding-ada-002 model
- Note deployment names

4. **Create Agents**:
Create 8 agents in AI Foundry for:
- PhotoAnalyzer
- CustomerInformation
- Inventory
- LocationService
- Navigation
- ProductMatchmaking
- ProductSearch
- ToolReasoning

5. **Create Application Insights**:
```bash
az monitor app-insights component create \
  --app appinsights-brk445 \
  --location eastus \
  --resource-group rg-aitour-brk445 \
  --application-type web
```

## Local Configuration

### 1. Set User Secrets

User secrets keep sensitive data out of source control.

**Navigate to AppHost**:
```bash
cd src/ZavaAppHost
```

**Initialize secrets**:
```bash
dotnet user-secrets init
```

**Set Azure AI Foundry connection**:
```bash
dotnet user-secrets set "ConnectionStrings:aifoundry" "<your-connection-string>"
```

**Set AI Foundry project**:
```bash
dotnet user-secrets set "ConnectionStrings:aifoundryproject" "<your-project-id>"
```

**Set Application Insights**:
```bash
dotnet user-secrets set "ConnectionStrings:appinsights" "<your-appinsights-connection-string>"
```

**Set Agent IDs**:
```bash
dotnet user-secrets set "ConnectionStrings:photoanalyzeragentid" "<agent-id>"
dotnet user-secrets set "ConnectionStrings:customerinformationagentid" "<agent-id>"
dotnet user-secrets set "ConnectionStrings:inventoryagentid" "<agent-id>"
dotnet user-secrets set "ConnectionStrings:locationserviceagentid" "<agent-id>"
dotnet user-secrets set "ConnectionStrings:navigationagentid" "<agent-id>"
dotnet user-secrets set "ConnectionStrings:productmatchmakingagentid" "<agent-id>"
dotnet user-secrets set "ConnectionStrings:productsearchagentid" "<agent-id>"
dotnet user-secrets set "ConnectionStrings:toolreasoningagentid" "<agent-id>"
```

**Verify secrets**:
```bash
dotnet user-secrets list
```

### 2. Verify Configuration

Check that all required settings are present:

```bash
dotnet user-secrets list | grep -E "(aifoundry|appinsights|agentid)"
```

You should see:
- ✅ ConnectionStrings:aifoundry
- ✅ ConnectionStrings:aifoundryproject
- ✅ ConnectionStrings:appinsights
- ✅ 8 agent ID connection strings

## Running the Application

### 1. Open in IDE

**Visual Studio Code**:
```bash
code .
```

**Visual Studio 2022**:
```bash
start src/Zava-Aspire.slnx
```

### 2. Start with .NET Aspire

**From IDE**:
- Open `src/ZavaAppHost/Program.cs`
- Press F5 or click "Run"

**From Command Line**:
```bash
cd src/ZavaAppHost
dotnet run
```

### 3. Access Aspire Dashboard

The Aspire dashboard opens automatically at: https://localhost:17001

The dashboard shows:
- **Resources**: All running services
- **Console Logs**: Real-time logs from all services
- **Traces**: Distributed tracing
- **Metrics**: Performance metrics
- **Environment Variables**: Configuration

### 4. Access Application

Once all services are healthy (green checkmarks in Aspire dashboard):

**Store Frontend**: https://localhost:7001

The Store app provides:
- Product catalog
- Single agent demo
- Multi-agent demo
- Agent catalog
- Settings (framework selection)

### 5. Verify Services

Check that all services are running:

| Service | URL | Status Check |
|---------|-----|--------------|
| Store | https://localhost:7001 | Open in browser |
| Products | https://localhost:7002/health | Should return "Healthy" |
| SingleAgent | https://localhost:7003/health | Should return "Healthy" |
| MultiAgent | https://localhost:7004/health | Should return "Healthy" |

## Testing the Application

### 1. Test Product Catalog

1. Navigate to https://localhost:7001
2. Browse products
3. Search for "drill"
4. Click on a product for details

### 2. Test Single Agent Demo

1. Click "Single Agent Demo" in navigation
2. Enter query: "Find me a red power drill under $200"
3. Click "Execute"
4. View results with inventory information

### 3. Test Multi-Agent Demo

1. Click "Multi-Agent Demo" in navigation
2. Upload a product image or enter description
3. Add location (optional)
4. Click "Orchestrate"
5. View workflow execution and results

### 4. Switch Agent Frameworks

1. Click "Settings" in navigation
2. Toggle between Semantic Kernel and Agent Framework
3. Return to demos and verify both frameworks work

## Common Issues and Solutions

### Issue: Docker not running

**Error**: "Cannot connect to Docker daemon"

**Solution**:
1. Start Docker Desktop
2. Wait for Docker to fully start
3. Retry running the application

### Issue: Port already in use

**Error**: "Address already in use"

**Solution**:
```bash
# Find process using port 7001
lsof -i :7001  # macOS/Linux
netstat -ano | findstr :7001  # Windows

# Kill the process
kill <PID>  # macOS/Linux
taskkill /PID <PID> /F  # Windows
```

### Issue: User secrets not found

**Error**: "ConnectionString 'aifoundry' not found"

**Solution**:
1. Verify you're in the ZavaAppHost directory
2. Run `dotnet user-secrets list`
3. Re-add missing secrets
4. Restart application

### Issue: SQL Server container fails

**Error**: "SQL Server container not starting"

**Solution**:
1. Check Docker has enough resources (4GB+ RAM)
2. Stop other containers
3. Clean up Docker: `docker system prune`
4. Restart application

### Issue: AI Foundry connection fails

**Error**: "Unable to connect to AI Foundry"

**Solution**:
1. Verify connection string is correct
2. Check Azure subscription is active
3. Verify AI Foundry project exists
4. Check network connectivity to Azure
5. Verify API key hasn't expired

### Issue: Build errors

**Error**: "Project file not found" or "SDK not found"

**Solution**:
```bash
# Clean solution
dotnet clean

# Restore packages
dotnet restore

# Rebuild
dotnet build
```

## Development Workflow

### 1. Make Code Changes

Edit files in your IDE. Aspire will automatically detect changes.

### 2. Hot Reload

For minor changes:
- Changes are automatically reloaded
- No restart needed
- Check console for reload messages

For major changes:
- Stop the application (Ctrl+C)
- Restart with `dotnet run`

### 3. View Logs

**In Aspire Dashboard**:
- Click on a service
- View real-time console output
- View structured logs

**In Terminal**:
```bash
dotnet run --project src/ZavaAppHost
# Logs appear in terminal
```

### 4. Debug

**Visual Studio Code**:
1. Set breakpoints in code
2. Press F5
3. Execute code path
4. Debug in Variables panel

**Visual Studio 2022**:
1. Set breakpoints in code
2. Start debugging (F5)
3. Step through code (F10/F11)

### 5. Test Changes

Run automated tests:
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test src/Products.Tests

# Run with coverage
dotnet test /p:CollectCoverage=true
```

## Next Steps

Now that you have the application running:

1. **Explore the Code**: Read [Developer Guide](08-developer-guide.md)
2. **Understand Architecture**: Review [Architecture Overview](01-architecture-overview.md)
3. **Learn the APIs**: Check [API Reference](04-api-reference.md)
4. **Deploy to Azure**: Follow [Deployment Architecture](06-deployment-architecture.md)

## Additional Resources

- [Session Delivery Resources](../../session-delivery-resources/readme.md)
- [Prerequisites Guide](../../session-delivery-resources/docs/Prerequisites.md)
- [How to Run Demo Locally](../../session-delivery-resources/docs/HowToRunDemoLocally.md)

## Getting Help

- **Issues**: Open an issue in the GitHub repository
- **Questions**: Check the [session delivery resources](../../session-delivery-resources/)
- **Support**: See [SUPPORT.md](../../SUPPORT.md)

---

**Congratulations!** 🎉 You now have the application running locally. Happy coding!
