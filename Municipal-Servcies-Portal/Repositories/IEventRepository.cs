using Municipal_Servcies_Portal.Models;

namespace Municipal_Servcies_Portal.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<IEnumerable<Event>> GetEventsByCategoryAsync(string category);
        Task<IEnumerable<Event>> GetUpcomingEventsAsync();
        Task<IEnumerable<Event>> SearchEventsAsync(string searchTerm, string? category, DateTime? startDate, DateTime? endDate);
    }
}