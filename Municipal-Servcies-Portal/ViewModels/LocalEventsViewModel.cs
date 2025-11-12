using System.ComponentModel.DataAnnotations;
using Municipal_Servcies_Portal.Models;

namespace Municipal_Servcies_Portal.ViewModels
{
    public class LocalEventsViewModel
    {
        // Search Parameters (for filtering)
        public string? SearchTerm { get; set; }
        public string? Category { get; set; }
        
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        // Display Properties (for individual event - matches Event model)
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public string ImagePath { get; set; } = string.Empty;

        // Collection properties for page display
        public List<Event>? Events { get; set; }
        public List<Announcement>? Announcements { get; set; }
        public List<string>? Categories { get; set; }
        public List<Event>? RecommendedEvents { get; set; }
    }
}