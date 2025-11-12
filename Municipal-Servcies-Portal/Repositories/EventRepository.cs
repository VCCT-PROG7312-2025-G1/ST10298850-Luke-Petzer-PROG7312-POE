using Microsoft.EntityFrameworkCore;
using Municipal_Servcies_Portal.Data;
using Municipal_Servcies_Portal.Models;

namespace Municipal_Servcies_Portal.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Event>> GetEventsByCategoryAsync(string category)
        {
            return await _dbSet
                .Where(e => e.IsActive && e.Category == category)
                .OrderBy(e => e.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
        {
            var today = DateTime.Today;
            return await _dbSet
                .Where(e => e.IsActive && e.StartDate >= today)
                .OrderBy(e => e.StartDate)
                .Take(10)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> SearchEventsAsync(
            string searchTerm, 
            string? category, 
            DateTime? startDate, 
            DateTime? endDate)
        {
            var query = _dbSet.Where(e => e.IsActive);

            // Search term filter (title or description)
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e => 
                    e.Title.Contains(searchTerm) || 
                    e.Description.Contains(searchTerm));
            }

            // Category filter
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(e => e.Category == category);
            }

            // Date range filter
            if (startDate.HasValue)
            {
                query = query.Where(e => e.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.StartDate <= endDate.Value);
            }

            return await query
                .OrderBy(e => e.StartDate)
                .ToListAsync();
        }
    }
}
