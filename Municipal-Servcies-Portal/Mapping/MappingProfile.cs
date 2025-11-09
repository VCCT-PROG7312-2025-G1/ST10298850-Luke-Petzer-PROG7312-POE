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
        }
    }
}