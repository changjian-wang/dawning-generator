# =============================================
# Dawning Generator API - Multi-stage Dockerfile
# =============================================

# ===== Build Stage =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["Dawning.Generator.sln", "./"]
COPY ["src/Dawning.Generator.Api.csproj", "src/"]
COPY ["src/Dawning.Generator.Application/Dawning.Generator.Application.csproj", "src/Dawning.Generator.Application/"]
COPY ["src/Dawning.Generator.Domain/Dawning.Generator.Domain.csproj", "src/Dawning.Generator.Domain/"]
COPY ["src/Dawning.Generator.Infrastructure/Dawning.Generator.Infrastructure.csproj", "src/Dawning.Generator.Infrastructure/"]
COPY ["nuget.config", "./"]

# Restore dependencies
RUN dotnet restore "src/Dawning.Generator.Api.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/src"
RUN dotnet build "Dawning.Generator.Api.csproj" -c Release -o /app/build

# ===== Publish Stage =====
FROM build AS publish
RUN dotnet publish "Dawning.Generator.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ===== Runtime Stage =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create non-root user for security
RUN groupadd -r dawning && useradd -r -g dawning dawning

# Copy published application
COPY --from=publish /app/publish .

# Copy templates
COPY --from=build /src/templates ./templates

# Create logs directory
RUN mkdir -p /app/Logs && chown -R dawning:dawning /app

# Switch to non-root user
USER dawning

# Expose ports
EXPOSE 5215

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5215

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:5215/health || exit 1

ENTRYPOINT ["dotnet", "Dawning.Generator.Api.dll"]
