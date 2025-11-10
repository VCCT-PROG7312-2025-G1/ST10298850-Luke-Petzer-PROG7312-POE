using AutoMapper;
using Municipal_Servcies_Portal.DataStructures;
using Municipal_Servcies_Portal.Models;
using Municipal_Servcies_Portal.Repositories;
using Municipal_Servcies_Portal.ViewModels;

namespace Municipal_Servcies_Portal.Services
{
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;

        // In-memory data structures (POE requirement)
        private readonly BinarySearchTree<Issue> _bst = new();
        private readonly MinHeap<Issue> _heap = new();
        private readonly Graph<int> _graph = new();
        private List<Issue> _allIssues = new();
        private bool _isInitialized;

        public ServiceRequestService(IIssueRepository issueRepository, IMapper mapper)
        {
            _issueRepository = issueRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Ensures data structures are initialized before use.
        /// Only initializes once per service instance.
        /// </summary>
        private async Task EnsureInitializedAsync()
        {
            if (_isInitialized)
                return;

            await InitializeAsync();
            _isInitialized = true;
        }

        /// <summary>
        /// Loads all issues from database into in-memory data structures.
        /// This method demonstrates BST, MinHeap, and Graph population.
        /// </summary>
        public async Task InitializeAsync()
        {
            _allIssues = (await _issueRepository.GetAllAsync()).ToList();

            // Clear existing data
            _bst.Clear();
            _heap.Clear();
            _graph.Clear();

            foreach (var issue in _allIssues)
            {
                // Load into BST for fast lookup by ID (O(log n))
                _bst.Insert(issue.Id, issue);

                // Load into MinHeap for priority processing
                _heap.Insert(issue.Priority, issue);

                // Load into Graph for dependency tracking
                _graph.AddVertex(issue.Id);
                foreach (var depId in issue.Dependencies)
                {
                    _graph.AddEdge(issue.Id, depId);
                }
            }
        }

        public async Task<List<ServiceRequestListViewModel>> GetAllRequestsAsync()
        {
            await EnsureInitializedAsync();
            return _mapper.Map<List<ServiceRequestListViewModel>>(_allIssues);
        }

        public async Task<ServiceRequestDetailViewModel?> GetRequestDetailsAsync(int id)
        {
            await EnsureInitializedAsync();

            // Use BST for O(log n) retrieval (POE requirement)
            var issue = _bst.Search(id);

            if (issue == null)
                return null;

            var viewModel = _mapper.Map<ServiceRequestDetailViewModel>(issue);

            // Use Graph for dependency traversal (POE requirement)
            var dependencyIds = _graph.BreadthFirstSearch(id).Skip(1).ToList(); // Skip self
            viewModel.DependencyIssues = _allIssues
                .Where(i => dependencyIds.Contains(i.Id))
                .ToList();

            return viewModel;
        }

        public async Task<List<ServiceRequestListViewModel>> GetRequestsByPriorityAsync()
        {
            await EnsureInitializedAsync();

            // Re-create a copy of the heap to extract from it
            var priorityHeap = new MinHeap<Issue>();
            foreach (var issue in _allIssues)
            {
                priorityHeap.Insert(issue.Priority, issue);
            }

            var sortedIssues = new List<Issue>();
            while (priorityHeap.Count > 0)
            {
                var minIssue = priorityHeap.ExtractMin();
                if (minIssue != null)
                    sortedIssues.Add(minIssue);
            }

            return _mapper.Map<List<ServiceRequestListViewModel>>(sortedIssues);
        }

        public async Task<List<Issue>> GetRequestDependenciesAsync(int id)
        {
            await EnsureInitializedAsync();

            var dependencyIds = _graph.BreadthFirstSearch(id).Skip(1).ToList();
            return _allIssues.Where(i => dependencyIds.Contains(i.Id)).ToList();
        }
    }
}
