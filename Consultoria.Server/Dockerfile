# ── Estágio 1: Build ───────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Python é necessário pelo Emscripten/wasm-tools para compilar assets WASM nativos
RUN apt-get update && \
    apt-get install -y --no-install-recommends python3 && \
    ln -sf /usr/bin/python3 /usr/bin/python && \
    rm -rf /var/lib/apt/lists/*

# Copiar arquivos de projeto para restauração de dependências
COPY ["Consultoria.Server/Consultoria.Server.csproj",         "Consultoria.Server/"]
COPY ["Consultoria.Client/Consultoria.Client.csproj",         "Consultoria.Client/"]
COPY ["Consultoria.Shared/Consultoria.Shared.csproj",         "Consultoria.Shared/"]
COPY ["Consultoria.Core/Consultoria.Core.csproj",             "Consultoria.Core/"]
COPY ["Consultoria.Infrastructure/Consultoria.Infrastructure.csproj", "Consultoria.Infrastructure/"]

# Instalar workload wasm-tools para Blazor WASM
RUN dotnet workload install wasm-tools

RUN dotnet restore "Consultoria.Server/Consultoria.Server.csproj"

# Copiar código fonte
COPY . .

# Build
WORKDIR "/src/Consultoria.Server"
RUN dotnet build "Consultoria.Server.csproj" -c Release -o /app/build

# ── Estágio 2: Publish ─────────────────────────────────────────────────────
FROM build AS publish
RUN dotnet publish "Consultoria.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ── Estágio 3: Runtime ─────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Consultoria.Server.dll"]
