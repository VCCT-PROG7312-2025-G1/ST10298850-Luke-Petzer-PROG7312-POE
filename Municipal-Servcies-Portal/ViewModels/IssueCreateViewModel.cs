using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Municipal_Servcies_Portal.ViewModels
{
    public class IssueCreateViewModel
    {
        [Required(ErrorMessage = "Location is required")]
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 2000 characters")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Attach Files (Optional)")]
        public IFormFile[]? Attachments { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Notification Email (Optional)")]
        public string? NotificationEmail { get; set; }

        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [Display(Name = "Notification Phone (Optional)")]
        public string? NotificationPhone { get; set; }
    }
}