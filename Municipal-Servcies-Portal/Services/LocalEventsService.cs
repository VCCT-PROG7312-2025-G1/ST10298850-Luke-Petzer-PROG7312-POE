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
        private readonly SearchHistoryService _searchHistoryService;

        public LocalEventsService(
            IEventRepository eventRepository, 
            AppDbContext context, 
            IMapper mapper,
            SearchHistoryService searchHistoryService)
        {
            _eventRepository = eventRepository;
            _context = context;
            _mapper = mapper;
            _searchHistoryService = searchHistoryService;
        }

        public async Task<LocalEventsViewModel> GetEventsPageViewModelAsync(
            string? searchTerm, 
            string? category, 
            DateTime? date)
        {
            // Track search in history for recommendations
            // Only track if user actually searched for something
            if (!string.IsNullOrEmpty(searchTerm) || !string.IsNullOrEmpty(category) || date.HasValue)
            {
                _searchHistoryService.AddSearch(searchTerm, category, date);
            }

            // Fix date filtering: if date is provided, search from that date onwards (not exact match)
            DateTime? startDate = date;
            DateTime? endDate = null; // No end date means "all future events from start date"

            // Get filtered events based on search parameters
            var events = await _eventRepository.SearchEventsAsync(
                searchTerm ?? string.Empty, 
                category, 
                startDate, 
                endDate);

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

            // Get personalized recommended events based on search history
            var recommendedEvents = await GetRecommendedEventsAsync();

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

        /// <summary>
        /// Generate personalized event recommendations based on user's search history
        /// Uses simple scoring algorithm: more recent searches and matching categories get higher priority
        /// </summary>
        private async Task<IEnumerable<Event>> GetRecommendedEventsAsync()
        {
            // Get user's search history
            var searchHistory = _searchHistoryService.GetSearchHistory();

            // If no search history, just return upcoming events
            if (!searchHistory.Any())
            {
                return await _eventRepository.GetUpcomingEventsAsync();
            }

            // Get all upcoming events
            var allUpcomingEvents = await _eventRepository.GetUpcomingEventsAsync();
            var eventsList = allUpcomingEvents.ToList();

            // Score each event based on search history
            var scoredEvents = new List<(Event evt, int score)>();

            foreach (var evt in eventsList)
            {
                int score = 0;

                // Check each search in history (newer searches = higher weight)
                for (int i = 0; i < searchHistory.Count; i++)
                {
                    var search = searchHistory[i];
                    // Weight: newer searches get higher multiplier
                    int weight = i + 1; 

                    // Match category (worth 3 points per weight)
                    if (!string.IsNullOrEmpty(search.Category) && 
                        evt.Category.Equals(search.Category, StringComparison.OrdinalIgnoreCase))
                    {
                        score += 3 * weight;
                    }

                    // Match search text in title or description (worth 2 points per weight)
                    if (!string.IsNullOrEmpty(search.SearchText))
                    {
                        if (evt.Title.Contains(search.SearchText, StringComparison.OrdinalIgnoreCase) ||
                            evt.Description.Contains(search.SearchText, StringComparison.OrdinalIgnoreCase))
                        {
                            score += 2 * weight;
                        }
                    }

                    // Match date proximity (worth 1 point per weight if within 7 days)
                    if (search.Date.HasValue)
                    {
                        var daysDiff = Math.Abs((evt.StartDate.Date - search.Date.Value.Date).Days);
                        if (daysDiff <= 7)
                        {
                            score += weight;
                        }
                    }
                }

                scoredEvents.Add((evt, score));
            }

            // Return top scored events, or if no matches, return upcoming events
            var topRecommended = scoredEvents
                .Where(x => x.score > 0)
                .OrderByDescending(x => x.score)
                .ThenBy(x => x.evt.StartDate)
                .Select(x => x.evt)
                .Take(6)
                .ToList();

            // If we got recommendations, return them
            if (topRecommended.Any())
            {
                return topRecommended;
            }

            // Otherwise, just return upcoming events as fallback
            return eventsList.Take(6);
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
