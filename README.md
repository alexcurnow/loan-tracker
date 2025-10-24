# State-Issued Loan Tracking System

A professional Blazor Server application demonstrating Clean Architecture, Domain-Driven Design (DDD), and CQRS patterns for tracking state-issued loans.

## Technology Stack

- **.NET 8** - Latest .NET framework
- **Blazor Server** - Interactive web UI framework
- **MudBlazor** - Material Design component library
- **Fluxor** - Redux-style state management (CQRS pattern)
- **Entity Framework Core** - ORM for data access
- **PostgreSQL** - Production database
- **Docker** - Containerization

## Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
LoanTracker.sln
├── LoanTracker.Domain          # Business entities and rules
├── LoanTracker.Application     # Use cases (Commands/Queries)
├── LoanTracker.Infrastructure  # Data access and external services
└── LoanTracker.Web             # Blazor UI and presentation
```

### Key Features

- **Domain-Driven Design**: Loan aggregate with proper encapsulation
- **CQRS Pattern**: Separate commands (writes) and queries (reads) using Fluxor
- **State Machine**: Workflow validation for loan status transitions
- **Seeded Data**: 6 borrower types + sample loans for demo

### Loan Workflow

1. **Open** → Initial draft state
2. **Awaiting Review** → Submitted for review
3. **Approval Pending** → Under final review
4. **Approved** → Loan approved (terminal state)
5. **Denied** → Loan rejected (terminal state)

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started) (for containerized deployment)
- [PostgreSQL 16](https://www.postgresql.org/download/) (if running without Docker)

### Corporate TLS inspection note
If `dotnet restore` fails with SSL errors, export your org�s root (and intermediate) CA(s) and place them in `.certs/` (hidden folder).  
Then rebuild: `docker compose build --no-cache && docker compose up`.

### Running with Docker (Recommended)

1. **Clone the repository**
   ```bash
   git clone <your-repo-url>
   cd loan-tracker
   ```

2. **Start the application**
   ```bash
   docker compose up
   ```

3. **Access the application**
   - Application: http://localhost:5000
   - PostgreSQL: localhost:5433 (external) / 5432 (internal)

4. **Stop the application**
   ```bash
   docker compose down
   ```

### Running Locally (Development)

1. **Install .NET 8 SDK**
   ```bash
   dotnet --version  # Should show 8.0.x
   ```

2. **Set up PostgreSQL**
   - Install PostgreSQL 16
   - Create database: `loantracker`
   - Update connection string in `appsettings.json`

3. **Run migrations**
   ```bash
   dotnet ef database update --project LoanTracker.Infrastructure --startup-project LoanTracker.Web
   ```

4. **Run the application**
   ```bash
   dotnet run --project LoanTracker.Web
   ```

5. **Access the application**
   - Navigate to: https://localhost:5001 or http://localhost:5000

## Project Structure

### Domain Layer
- **Entities**: `Loan`, `BorrowerType`
- **Enums**: `LoanStatus`
- **Services**: `WorkflowStateMachine` for state transitions
- **Interfaces**: Repository contracts

### Application Layer
- **Commands**: `CreateLoanCommand`, `UpdateLoanCommand`, `TransitionLoanStatusCommand`
- **Queries**: `GetAllLoansQuery`, `GetLoanByIdQuery`, `GetLoansByStatusQuery`
- **DTOs**: `LoanDto` for data transfer
- **Handlers**: Command and query handlers

### Infrastructure Layer
- **DbContext**: Entity Framework Core configuration
- **Repositories**: Data access implementations
- **Migrations**: Database schema versioning

### Web Layer
- **Pages**:
  - Dashboard - Loan statistics and recent applications
  - Loan List - All loans with filtering and sorting
  - Loan Details - View loan and manage workflow transitions
  - Create Loan - Form for new loan applications
- **Store**: Fluxor state management (actions, reducers, effects)
- **Layout**: MudBlazor navigation and theming

## Sample Data

The application automatically seeds the database with sample data on first run:

### Borrower Types
- Municipality
- County
- School District
- University
- Small Business
- State Agency

### Sample Loans
The database includes 20 sample loans demonstrating various states:
- Open loans (draft applications)
- Awaiting Review loans (submitted for review)
- Approval Pending loans (under final review)
- Approved loans (terminal state)
- Denied loans (terminal state)

Loan amounts range from $50,000 to $8,000,000 with various interest rates (3.5% - 6.5%) and terms (5-30 years).

## Key Patterns Demonstrated

### Clean Architecture
- **Domain** doesn't depend on anything
- **Application** depends only on Domain
- **Infrastructure** implements Domain/Application interfaces
- **Web** depends on Application and Infrastructure

### CQRS with Fluxor
- **Actions**: User intents (LoadLoansAction, CreateLoanAction)
- **Reducers**: Pure state transformations
- **Effects**: Side effects (API calls, database access)
- **State**: Single source of truth for UI

### Workflow State Machine
```csharp
// Valid transitions defined in WorkflowStateMachine
Open → AwaitingReview
AwaitingReview → ApprovalPending | Open
ApprovalPending → Approved | Denied | AwaitingReview
Approved → ∅ (terminal)
Denied → ∅ (terminal)
```

## Development

### Building
```bash
dotnet build
```

### Running Tests
```bash
dotnet test
```

### Creating Migrations
```bash
dotnet ef migrations add MigrationName --project LoanTracker.Infrastructure --startup-project LoanTracker.Web
```

## Deployment

### Docker
The application is containerized and ready for deployment:
- Multi-stage Dockerfile for optimized image size
- Docker Compose for local orchestration
- Health checks for PostgreSQL

### Railway/Cloud Platform
1. Set environment variables:
   - `ConnectionStrings__DefaultConnection`: PostgreSQL connection string
   - `ASPNETCORE_ENVIRONMENT`: Production
2. Deploy container or use platform's PostgreSQL addon
3. Migrations run automatically on startup

## Interview Talking Points

- **Clean Architecture**: Demonstrates separation of concerns with clear dependency flow
- **DDD**: Loan as aggregate root with proper encapsulation
- **CQRS**: Fluxor provides Redux-style state management with clear command/query separation
- **State Machine**: Ensures data integrity through validated workflow transitions
- **Modern Stack**: Blazor Server, MudBlazor, EF Core, PostgreSQL
- **Containerization**: Production-ready Docker setup
- **Best Practices**: Repository pattern, dependency injection, async/await

## License

This is a portfolio project for demonstration purposes.

## Author

Built to demonstrate proficiency in enterprise .NET development patterns used at the Tennessee Comptroller's Office and similar government technology roles.
