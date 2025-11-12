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

        public async Task<int> GetResolvedIssuesCountAsync()
        {
            // Count issues with status "Resolved" or "Completed"
            return await _dbSet
                .Where(i => i.IsActive && (i.Status == "Resolved" || i.Status == "Completed"))
                .CountAsync();
        }

        public async Task<int> GetActiveIssuesCountAsync()
        {
            // Count issues with status "Pending", "In Progress", or "Under Review"
            return await _dbSet
                .Where(i => i.IsActive && (i.Status == "Pending" || i.Status == "In Progress" || i.Status == "Under Review"))
                .CountAsync();
        }

        public async Task<double> GetAverageResponseTimeInHoursAsync()
        {
            // Calculate average time between DateReported and LastUpdated for resolved issues
            var resolvedIssues = await _dbSet
                .Where(i => i.IsActive && 
                           (i.Status == "Resolved" || i.Status == "Completed") && 
                           i.LastUpdated.HasValue)
                .Select(i => new { i.DateReported, i.LastUpdated })
                .ToListAsync();

            if (!resolvedIssues.Any())
                return 0;

            // Calculate average hours
            var averageHours = resolvedIssues
                .Average(i => (i.LastUpdated!.Value - i.DateReported).TotalHours);

            return averageHours;
        }
    }
}