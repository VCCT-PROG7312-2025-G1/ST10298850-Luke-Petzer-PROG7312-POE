using Municipal_Servcies_Portal.ViewModels;

namespace Municipal_Servcies_Portal.Services
{
    public interface ILocalEventsService
    {
        Task<LocalEventsViewModel> GetEventsPageViewModelAsync(
            string? searchTerm, 
            string? category, 
            DateTime? date);
        
        Task<LocalEventsViewModel?> GetEventByIdAsync(int id);
    }
}