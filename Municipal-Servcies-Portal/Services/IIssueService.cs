using Municipal_Servcies_Portal.ViewModels;

namespace Municipal_Servcies_Portal.Services
{
    public interface IIssueService
    {
        Task<int> CreateIssueAsync(IssueCreateViewModel viewModel);
        Task<IEnumerable<IssueViewModel>> GetRecentIssuesAsync(int count);
        Task<IssueViewModel?> GetIssueByIdAsync(int id);
    }
}