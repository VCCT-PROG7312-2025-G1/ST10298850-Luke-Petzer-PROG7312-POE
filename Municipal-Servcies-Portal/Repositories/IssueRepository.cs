using Microsoft.EntityFrameworkCore;
using Municipal_Servcies_Portal.Data;
using Municipal_Servcies_Portal.Models;

namespace Municipal_Servcies_Portal.Repositories
{
    public class IssueRepository : Repository<Issue>, IIssueRepository
    {
        public IssueRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Issue>> GetRecentIssuesAsync(int count)
        {
            return await _dbSet
                .Where(i => i.IsActive)
                .OrderByDescending(i => i.DateReported)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Issue>> GetIssuesByCategoryAsync(string category)
        {
            return await _dbSet
                .Where(i => i.IsActive && i.Category == category)
                .OrderByDescending(i => i.DateReported)
                .ToListAsync();
        }

        public async Task<IEnumerable<Issue>> GetIssuesByStatusAsync(string status)
        {
            return await _dbSet
                .Where(i => i.IsActive && i.Status == status)
                .OrderByDescending(i => i.DateReported)
                .ToListAsync();
        }
    }
}