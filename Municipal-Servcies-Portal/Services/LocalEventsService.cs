using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Municipal_Servcies_Portal.Data;
using Municipal_Servcies_Portal.Models;
using Municipal_Servcies_Portal.Repositories;
using Municipal_Servcies_Portal.ViewModels;

namespace Municipal_Servcies_Portal.Services
{
    public class LocalEventsService : ILocalEventsService
    {
        private readonly IEventRepository _eventRepository;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public LocalEventsService(IEventRepository eventRepository, AppDbContext context, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<LocalEventsViewModel> GetEventsPageViewModelAsync(
            string? searchTerm, 
            string? category, 
            DateTime? date)
        {
            // Get filtered events based on search parameters
            var events = await _eventRepository.SearchEventsAsync(
                searchTerm ?? string.Empty, 
                category, 
                date, 
                date);

            // Get all categories for filter dropdown
            var allEvents = await _eventRepository.GetAllAsync();
            var categories = allEvents
                .Select(e => e.Category)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            // Get announcements
            var announcements = await _context.Announcements
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.DatePosted)
                .Take(5)
                .ToListAsync();

            // Get recommended events (upcoming events for now)
            var recommendedEvents = await _eventRepository.GetUpcomingEventsAsync();

            return new LocalEventsViewModel
            {
                SearchTerm = searchTerm,
                Category = category,
                StartDate = date,
                Events = events.ToList(),
                Categories = categories,
                Announcements = announcements,
                RecommendedEvents = recommendedEvents.ToList()
            };
        }

        public async Task<LocalEventsViewModel?> GetEventByIdAsync(int id)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(id);
            return eventEntity != null 
                ? _mapper.Map<LocalEventsViewModel>(eventEntity) 
                : null;
        }
    }
}
