# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# --- Corporate CA (kept from your original) ---
COPY .certs/ /usr/local/share/ca-certificates/
RUN apt-get update && apt-get install -y --no-install-recommends ca-certificates \
 && update-ca-certificates || true \
 && rm -rf /var/lib/apt/lists/*

# --- Make vendored NuGet packages available inside the build context ---
# These two lines are the key: commit NuGet.config and nuget/local/* to your repo
COPY NuGet.config ./
COPY nuget/local/ ./nuget/local/

# Copy solution + project files first (best layer caching)
COPY LoanTracker.sln ./
COPY LoanTracker.Domain/LoanTracker.Domain.csproj ./LoanTracker.Domain/
COPY LoanTracker.Application/LoanTracker.Application.csproj ./LoanTracker.Application/
COPY LoanTracker.Infrastructure/LoanTracker.Infrastructure.csproj ./LoanTracker.Infrastructure/
COPY LoanTracker.Web/LoanTracker.Web.csproj ./LoanTracker.Web/

# Restore using the repo-scoped NuGet.config (LocalRepoFeed + nuget.org)
RUN dotnet restore LoanTracker.sln

# Copy the rest of the source
COPY . .

# Build and publish (flip -c Debug if you want debug symbols in-container)
WORKDIR /src/LoanTracker.Web
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "LoanTracker.Web.dll"]
