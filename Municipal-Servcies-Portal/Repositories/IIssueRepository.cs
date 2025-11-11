using Municipal_Servcies_Portal.Models;

namespace Municipal_Servcies_Portal.Repositories
{
    public interface IIssueRepository : IRepository<Issue>
    {
        Task<IEnumerable<Issue>> GetRecentIssuesAsync(int count);
        Task<IEnumerable<Issue>> GetIssuesByCategoryAsync(string category);
        Task<IEnumerable<Issue>> GetIssuesByStatusAsync(string status);
        Task<int> GetResolvedIssuesCountAsync();
        Task<int> GetActiveIssuesCountAsync();
        Task<double> GetAverageResponseTimeInHoursAsync();
    }
}