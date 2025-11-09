using AutoMapper;
using Municipal_Servcies_Portal.Models;
using Municipal_Servcies_Portal.ViewModels;

namespace Municipal_Servcies_Portal.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map from IssueCreateViewModel to Issue
            CreateMap<IssueCreateViewModel, Issue>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.DateReported, opt => opt.Ignore())
                .ForMember(dest => dest.AttachmentPathsJson, opt => opt.Ignore())
                .ForMember(dest => dest.AttachmentPaths, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdated, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedTo, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            // Map from Issue to IssueViewModel for display
            CreateMap<Issue, IssueViewModel>();
            
            
            // Event mappings (Phase 2)
            CreateMap<Event, LocalEventsViewModel>()
                .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EventEndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.SearchTerm, opt => opt.Ignore())
                .ForMember(dest => dest.StartDate, opt => opt.Ignore())
                .ForMember(dest => dest.EndDate, opt => opt.Ignore())
                .ForMember(dest => dest.Events, opt => opt.Ignore())
                .ForMember(dest => dest.Announcements, opt => opt.Ignore())
                .ForMember(dest => dest.Categories, opt => opt.Ignore())
                .ForMember(dest => dest.RecommendedEvents, opt => opt.Ignore());

            CreateMap<LocalEventsViewModel, Event>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.EventDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EventEndDate))
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());
        }
    }
}