using AutoMapper;
using Municipal_Servcies_Portal.Models;
using Municipal_Servcies_Portal.ViewModels;

namespace Municipal_Servcies_Portal.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add mappings as you create ViewModels
            // Examples for when you need them:
            
            // Issue mappings
            // CreateMap<Issue, IssueViewModel>().ReverseMap();
            
            // Events mappings
            // CreateMap<Events, EventViewModel>().ReverseMap();
            
            // LocalEvents mappings (you already have this ViewModel)
            // CreateMap<Event, LocalEventsViewModel>()
            //     .ForMember(dest => dest.Events, opt => opt.Ignore())
            //     .ForMember(dest => dest.Categories, opt => opt.Ignore())
            //     .ForMember(dest => dest.RecommendedSearches, opt => opt.Ignore());
        }
    }
}