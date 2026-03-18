# Consultoria Dev — Plataforma de Consultoria em Desenvolvimento

Plataforma fullstack construída com **ASP.NET Core 8 Blazor WebAssembly**, SQL Server em container Docker.

## Grupo
|Nome|RM|
|:--:|:--:|
|Diogo Julio|553837|
|Matheus Zottis|94119|
|Jonata Rafael|552939|
|Victor Didoff|552965|
|Vinicius Silva|553240|

## Estrutura do Projeto

```
cp4-project/
├── Consultoria.sln
├── Consultoria.Server/          # ASP.NET Core Web API + host Blazor WASM
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── Client/
│   │   │   ├── ProblemsController.cs    # GET api/client/problems
│   │   │   └── ConsultationController.cs # POST api/client/consultation
│   │   └── Admin/
│   │       ├── AdminConsultationController.cs  # [Authorize(Roles="Admin")]
│   │       └── AdminProblemsController.cs      # [Authorize(Roles="Admin")]
│   ├── Program.cs
│   └── Dockerfile
├── Consultoria.Client/          # Blazor WebAssembly
│   ├── Pages/
│   │   ├── Index.razor          # Home + Anti-Gravity cards
│   │   ├── Login.razor
│   │   ├── Register.razor
│   │   └── Admin/
│   │       ├── Dashboard.razor
│   │       └── ProblemManagement.razor
│   ├── Auth/CustomAuthStateProvider.cs
│   └── wwwroot/
│       ├── js/antigravity.js
│       └── css/app.css
├── Consultoria.Shared/          # DTOs e Enums compartilhados
├── Consultoria.Core/            # Interfaces de serviço
├── Consultoria.Infrastructure/  # EF Core, entidades, serviços
├── docker-compose.yml
├── .env.example
└── .gitignore
```

## Executar com Docker (recomendado)

### Pré-requisitos
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e em execução

### 1. Configurar variáveis de ambiente
```bash
# linux
cp .env.example .env
```
```powershell
# windows
copy .env.example .env
```
> Edite .env com sua senha do SA (deve ser forte: maiúsculas + números + especiais)


### 2. Subir os containers
```bash
docker-compose up --build
```

O `docker-compose` irá:
1. **Construir a imagem** do app Blazor (SDK → publish → runtime)
2. **Subir o SQL Server 2022** em container com volume persistente
3. **Aguardar o DB ficar saudável** antes de iniciar o app (health check)
4. **Aplicar as migrations automaticamente** na primeira execução
5. **Seed dos dados**: roles (Admin/Client), admin user, 6 problemas, 5 developers

### 3. Acessar
| Serviço | URL |
|---------|-----|
| Aplicação | http://localhost:8080 |
| Swagger API | http://localhost:8080/swagger |

### Credenciais do Admin (seed)
| Campo | Valor |
|-------|-------|
| Email | `admin@consultoria.com` |
| Senha | `Admin@123!` |

---

## Desenvolver localmente (sem Docker)

```bash
# 1. Certifique-se de ter um SQL Server em container rodando:
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong!Passw0rd#2026" \
  -p 1433:1433 --name sqlserver-dev -d \
  mcr.microsoft.com/mssql/server:2022-latest

# 2. Configure a connection string via variável de ambiente:
set ConnectionStrings__DefaultConnection=Server=localhost,1433;Database=ConsultoriaDb;User Id=sa;Password=YourStrong!Passw0rd#2026;TrustServerCertificate=True

# 3. Run
cd Consultoria.Server
dotnet run
```

---

## Segurança

- **Autenticação por cookies** via ASP.NET Core Identity
- **Claims-based authorization** — roles `Admin` e `Client`
- **Controladores completamente separados**: `Client/` vs `Admin/`
- **Secrets via variáveis de ambiente** — sem credenciais no código
- **Health check** no SQL Server antes de aceitar tráfego

---

## API Endpoints

### Público
| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/api/auth/login` | Login |
| `POST` | `/api/auth/register` | Registro |
| `POST` | `/api/auth/logout` | Logout |
| `GET`  | `/api/auth/user` | Usuário atual |
| `GET`  | `/api/client/problems` | Listar problemas |
| `POST` | `/api/client/consultation` | Solicitar consultoria |

### Admin `[Authorize(Roles="Admin")]`
| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/admin/consultation` | Listar solicitações |
| `PUT` | `/api/admin/consultation/{id}/status` | Aprovar/Reprovar |
| `POST` | `/api/admin/consultation/{requestId}/dispatch/{developerId}` | Encaminhar via WhatsApp |
| `GET/POST/PUT/DELETE` | `/api/admin/problems` | CRUD de problemas |

Documentação completa em `http://localhost:8080/swagger`.
