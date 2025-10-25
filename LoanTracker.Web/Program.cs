using Fluxor;
using LoanTracker.Application.Commands;
using LoanTracker.Application.Interfaces;
using LoanTracker.Application.Queries;
using LoanTracker.Domain.Entities;
using LoanTracker.Domain.Interfaces;
using LoanTracker.Domain.Services;
using LoanTracker.Infrastructure.Data;
using LoanTracker.Infrastructure.Repositories;
using LoanTracker.Web.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Marten;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add MudBlazor
builder.Services.AddMudServices();

// Add Fluxor state management
builder.Services.AddFluxor(options =>
{
    options.ScanAssemblies(typeof(Program).Assembly);
});

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Database=loantracker;Username=postgres;Password=postgres";

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// Also add DbContext for migration purposes
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// Add Marten for Event Sourcing
builder.Services.AddMarten(connectionString);

// Add repositories
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<IBorrowerTypeRepository, BorrowerTypeRepository>();
builder.Services.AddScoped<IProjectRepository, LoanTracker.Infrastructure.Repositories.ProjectRepository>();
builder.Services.AddScoped<IDisbursementRepository, LoanTracker.Infrastructure.Repositories.DisbursementRepository>();

// Add services
builder.Services.AddScoped<WorkflowStateMachine>();
builder.Services.AddScoped<InterestCalculationService>();
builder.Services.AddScoped<IEventStore, LoanTracker.Infrastructure.Services.MartenEventStore>();

// Add command handlers
builder.Services.AddScoped<ICommandHandler<CreateLoanCommand, LoanTracker.Application.Common.Result<LoanTracker.Application.DTOs.LoanDto>>, CreateLoanCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateLoanCommand, LoanTracker.Application.DTOs.LoanDto>, UpdateLoanCommandHandler>();
builder.Services.AddScoped<ICommandHandler<TransitionLoanStatusCommand, LoanTracker.Application.Common.Result<LoanTracker.Application.DTOs.LoanDto>>, TransitionLoanStatusCommandHandler>();
builder.Services.AddScoped<ICommandHandler<IssueDisbursementCommand, LoanTracker.Application.Common.Result<LoanTracker.Application.Commands.DisbursementDto>>, IssueDisbursementCommandHandler>();

// Add query handlers
builder.Services.AddScoped<IQueryHandler<GetAllLoansQuery, IEnumerable<LoanTracker.Application.DTOs.LoanDto>>, GetAllLoansQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetLoanByIdQuery, LoanTracker.Application.DTOs.LoanDto?>, GetLoanByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetLoansByStatusQuery, IEnumerable<LoanTracker.Application.DTOs.LoanDto>>, GetLoansByStatusQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllBorrowerTypesQuery, IEnumerable<BorrowerType>>, GetAllBorrowerTypesQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetLoanInterestAccrualsQuery, IEnumerable<LoanTracker.Application.Queries.InterestAccrualDto>>, GetLoanInterestAccrualsQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetProjectsByLoanIdQuery, IEnumerable<LoanTracker.Application.Queries.ProjectDto>>, GetProjectsByLoanIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetDisbursementsByLoanIdQuery, IEnumerable<LoanTracker.Application.Queries.DisbursementDto>>, GetDisbursementsByLoanIdQueryHandler>();

var app = builder.Build();

// Apply migrations and seed database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
