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
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Host=localhost;Database=loantracker;Username=postgres;Password=postgres"
    )
);

// Add repositories
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<IBorrowerTypeRepository, BorrowerTypeRepository>();

// Add services
builder.Services.AddScoped<WorkflowStateMachine>();

// Add command handlers
builder.Services.AddScoped<ICommandHandler<CreateLoanCommand, LoanTracker.Application.DTOs.LoanDto>, CreateLoanCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateLoanCommand, LoanTracker.Application.DTOs.LoanDto>, UpdateLoanCommandHandler>();
builder.Services.AddScoped<ICommandHandler<TransitionLoanStatusCommand, LoanTracker.Application.DTOs.LoanDto>, TransitionLoanStatusCommandHandler>();

// Add query handlers
builder.Services.AddScoped<IQueryHandler<GetAllLoansQuery, IEnumerable<LoanTracker.Application.DTOs.LoanDto>>, GetAllLoansQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetLoanByIdQuery, LoanTracker.Application.DTOs.LoanDto?>, GetLoanByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetLoansByStatusQuery, IEnumerable<LoanTracker.Application.DTOs.LoanDto>>, GetLoansByStatusQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllBorrowerTypesQuery, IEnumerable<BorrowerType>>, GetAllBorrowerTypesQueryHandler>();

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
