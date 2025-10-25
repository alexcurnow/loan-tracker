# Development Guide

## Recommended Development Workflow

This project uses the **hybrid approach** - dependencies in Docker, app running locally in your IDE.

### Why This Approach?

- ✅ Full debugging with breakpoints
- ✅ Hot reload for instant feedback
- ✅ Fast iteration (no Docker rebuilds)
- ✅ No local PostgreSQL installation needed
- ✅ Consistent database environment across team

---

## Quick Start (JetBrains Rider)

### 1. Start the Database

```bash
./dev-start.sh
# Or manually:
docker compose -f docker-compose.dev.yml up -d
```

### 2. Open Project in Rider

```bash
rider LoanTracker.sln
```

### 3. Configure Run Configuration (First Time Only)

Rider should auto-detect the `LoanTracker.Web` project. If not:

1. **Run > Edit Configurations**
2. **Add New Configuration > .NET Project**
3. Set:
   - **Name**: `LoanTracker.Web (Development)`
   - **Project**: `LoanTracker.Web`
   - **Environment Variables**: `ASPNETCORE_ENVIRONMENT=Development`
   - **Launch Profile**: Use settings from `Properties/launchSettings.json`

### 4. Run/Debug

- **Debug**: Press `F5` or click the bug icon
- **Run**: Press `Shift+F10` or click the play icon

The app will start at `https://localhost:5001` (check Rider's console for exact URL)

---

## Common Development Tasks

### View Database

**Option 1: Rider Database Tools (Built-in)**

1. **View > Tool Windows > Database**
2. **+ > Data Source > PostgreSQL**
3. Configure:
   - **Host**: localhost
   - **Port**: 5433
   - **Database**: loantracker
   - **User**: postgres
   - **Password**: postgres123
4. Click **Test Connection**, then **OK**

**Option 2: Command Line**

```bash
# Quick query
docker exec loan-tracker-db-1 psql -U postgres -d loantracker -c "SELECT * FROM \"Loans\";"

# Interactive mode
docker exec -it loan-tracker-db-1 psql -U postgres -d loantracker
```

### Run Migrations

```bash
cd LoanTracker.Web
dotnet ef migrations add YourMigrationName --project ../LoanTracker.Infrastructure/LoanTracker.Infrastructure.csproj --context ApplicationDbContext

# Apply migrations (or just run the app - migrations apply automatically)
dotnet ef database update --project ../LoanTracker.Infrastructure/LoanTracker.Infrastructure.csproj
```

### Reset Database

```bash
# Stop and remove database container (deletes data)
docker compose -f docker-compose.dev.yml down -v

# Start fresh
docker compose -f docker-compose.dev.yml up -d
```

### View Logs

In Rider's **Run** or **Debug** window, you'll see all logs in real-time.

For database logs:
```bash
docker compose -f docker-compose.dev.yml logs -f db
```

---

## When to Use Full Docker Stack

For testing production-like environment or debugging Docker-specific issues:

```bash
# Stop dev database
docker compose -f docker-compose.dev.yml down

# Start full stack (app + database)
docker compose up --build

# App runs at: http://localhost:5000
```

**Note**: You won't be able to debug with breakpoints in this mode.

---

## Project Structure

```
loan-tracker/
├── LoanTracker.Domain/          # Domain entities, value objects, interfaces
├── LoanTracker.Application/     # Business logic, CQRS handlers
├── LoanTracker.Infrastructure/  # Data access, repositories, EF migrations
├── LoanTracker.Web/            # Blazor UI, Fluxor state management
├── docker-compose.yml          # Production/full stack
├── docker-compose.dev.yml      # Development (database only)
└── Dockerfile                  # App container image
```

---

## Debugging Tips

### Set Breakpoints

1. Click in the left gutter next to any line number
2. Run in **Debug** mode (F5)
3. When code hits the breakpoint, you can:
   - Inspect variables
   - Step through code (F10, F11)
   - Evaluate expressions
   - Modify values on-the-fly

### Debug Entity Framework Queries

In `appsettings.Development.json`, set:

```json
"Logging": {
  "LogLevel": {
    "Microsoft.EntityFrameworkCore.Database.Command": "Information"
  }
}
```

You'll see all SQL queries in the Rider console.

### Debug Blazor Components

- Set breakpoints in `.razor.cs` files or `@code` blocks
- Use `@Console.WriteLine()` for quick debugging
- Check browser console (F12) for JavaScript errors

---

## Troubleshooting

### "Database connection failed"

```bash
# Check if database is running
docker compose -f docker-compose.dev.yml ps

# Check logs
docker compose -f docker-compose.dev.yml logs db

# Restart database
docker compose -f docker-compose.dev.yml restart db
```

### "Port already in use"

```bash
# Check what's using port 5433
lsof -i :5433

# Or stop the dev database and use a different port in docker-compose.dev.yml
```

### "Cannot write DateTime with Kind=Local"

PostgreSQL requires UTC timestamps. Always use:
- `DateTime.UtcNow` instead of `DateTime.Now`
- `DateTime.UtcNow.Date` instead of `DateTime.Today`
- `DateTime.SpecifyKind(date, DateTimeKind.Utc)` when converting

---

## Team Best Practices

### Before Committing

```bash
# Run build
dotnet build

# Run tests (when you add them)
dotnet test

# Check for EF migrations
git status | grep Migration
```

### Sharing Database Changes

1. Create migration: `dotnet ef migrations add YourMigration`
2. Commit migration files in `LoanTracker.Infrastructure/Migrations/`
3. Team members get changes automatically when they pull and run the app

### Environment Variables

- **Never commit secrets** to `appsettings.json`
- Use `appsettings.Development.json` for local settings (not committed if in .gitignore)
- Use environment variables or User Secrets for sensitive data

---

## IDE Shortcuts (Rider)

- `F5` - Start debugging
- `Shift+F10` - Run without debugging
- `Ctrl+F5` - Stop
- `Ctrl+Shift+F10` - Run current file/test
- `F9` - Toggle breakpoint
- `F10` - Step over
- `F11` - Step into
- `Shift+F11` - Step out
- `Alt+F8` - Evaluate expression

---

## Production Deployment

When ready to deploy:

```bash
# Build and test full stack locally
docker compose build
docker compose up

# Push to production (example)
docker compose -f docker-compose.prod.yml up -d
```

See `docker-compose.yml` for production configuration.
