using LoanTracker.Domain.Entities;
using LoanTracker.Domain.Interfaces;
using LoanTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoanTracker.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public ProjectRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Project?> GetByIdAsync(Guid projectId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Projects
            .Include(p => p.Loan)
            .FirstOrDefaultAsync(p => p.ProjectId == projectId);
    }

    public async Task<IEnumerable<Project>> GetByLoanIdAsync(Guid loanId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Projects
            .Where(p => p.LoanId == loanId)
            .ToListAsync();
    }

    public async Task<Project> AddAsync(Project project)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Projects.Add(project);
        await context.SaveChangesAsync();
        return project;
    }

    public async Task UpdateAsync(Project project)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        project.UpdatedAt = DateTime.UtcNow;
        context.Projects.Update(project);
        await context.SaveChangesAsync();
    }
}
