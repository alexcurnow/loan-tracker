# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Trust corporate CA(s) ¿ optional (safe when none present)
COPY certs/ /usr/local/share/ca-certificates/
RUN apt-get update && apt-get install -y --no-install-recommends ca-certificates \
 && update-ca-certificates || true \
 && rm -rf /var/lib/apt/lists/*

# Copy solution and project files
COPY LoanTracker.sln ./
COPY LoanTracker.Domain/LoanTracker.Domain.csproj ./LoanTracker.Domain/
COPY LoanTracker.Application/LoanTracker.Application.csproj ./LoanTracker.Application/
COPY LoanTracker.Infrastructure/LoanTracker.Infrastructure.csproj ./LoanTracker.Infrastructure/
COPY LoanTracker.Web/LoanTracker.Web.csproj ./LoanTracker.Web/

# Restore dependencies
RUN dotnet restore

# Copy all source files
COPY . .

# Build and publish
WORKDIR /src/LoanTracker.Web
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published app
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080

# Set environment variable for ASP.NET Core
ENV ASPNETCORE_URLS=http://+:8080

# Run the application
ENTRYPOINT ["dotnet", "LoanTracker.Web.dll"]
