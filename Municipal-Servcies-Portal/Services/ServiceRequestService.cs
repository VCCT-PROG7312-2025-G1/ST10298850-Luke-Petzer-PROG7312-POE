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
            Console.WriteLine("Loading service requests from database..."); // DEBUG
            
            _allIssues = (await _issueRepository.GetAllAsync()).ToList();
            
            Console.WriteLine($"Loaded {_allIssues.Count} issues"); // DEBUG

            // Clear existing data structures before populating
            _bst.Clear();
            _heap.Clear();
            _graph.Clear();

            Console.WriteLine("Building data structures..."); // DEBUG
            
            foreach (var issue in _allIssues)
            {
                // Load into BST for fast lookup by ID (O(log n))
                _bst.Insert(issue.Id, issue);

                // Load into MinHeap for priority processing
                _heap.Insert(issue.Priority, issue);

                // Load into Graph for dependency tracking
                _graph.AddVertex(issue.Id);
                
                // Add edges for dependencies if they exist
                if (issue.Dependencies != null && issue.Dependencies.Any())
                {
                    foreach (var depId in issue.Dependencies)
                    {
                        _graph.AddEdge(issue.Id, depId);
                        Console.WriteLine($"Added dependency edge: {issue.Id} -> {depId}"); // DEBUG
                    }
                }
            }
            
            Console.WriteLine("Data structures initialized successfully"); // DEBUG
        }

        public async Task<List<ServiceRequestListViewModel>> GetAllRequestsAsync()
        {
            await EnsureInitializedAsync();
            return _mapper.Map<List<ServiceRequestListViewModel>>(_allIssues);
        }

        public async Task<ServiceRequestDetailViewModel?> GetRequestDetailsAsync(int id)
        {
            // Make sure data structures are loaded
            await EnsureInitializedAsync();

            Console.WriteLine($"Searching for issue #{id} using BST..."); // DEBUG
            
            // Use BST to find the issue quickly (O(log n) instead of O(n))
            var issue = _bst.Search(id);

            if (issue == null)
            {
                Console.WriteLine($"Issue #{id} not found"); // DEBUG
                return null; // Not found
            }

            Console.WriteLine($"Found issue #{id}: {issue.Description}"); // DEBUG
            
            // Convert to view model for the view
            var details = _mapper.Map<ServiceRequestDetailViewModel>(issue);

            // Find related issues using Graph BFS traversal
            Console.WriteLine($"Finding dependencies for issue #{id}..."); // DEBUG
            var relatedIds = _graph.BreadthFirstSearch(id);
            
            // Remove the first item (the issue itself)
            if (relatedIds.Count > 0)
                relatedIds.RemoveAt(0);

            // Get the actual issue objects for the dependencies
            var dependencies = new List<Issue>();
            foreach (var depId in relatedIds)
            {
                var depIssue = _allIssues.FirstOrDefault(i => i.Id == depId);
                if (depIssue != null)
                {
                    dependencies.Add(depIssue);
                    Console.WriteLine($"  - Dependency found: #{depId}"); // DEBUG
                }
            }
            
            details.DependencyIssues = dependencies;
            
            Console.WriteLine($"Total dependencies: {dependencies.Count}"); // DEBUG

            return details;
        }

        public async Task<List<ServiceRequestListViewModel>> GetRequestsByPriorityAsync()
        {
            await EnsureInitializedAsync();

            Console.WriteLine("Sorting issues by priority using MinHeap..."); // DEBUG
            
            // Re-create a copy of the heap to extract from it
            // We can't reuse the original heap because ExtractMin removes items
            var priorityHeap = new MinHeap<Issue>();
            foreach (var issue in _allIssues)
            {
                priorityHeap.Insert(issue.Priority, issue);
            }

            Console.WriteLine($"Heap contains {priorityHeap.Count} items"); // DEBUG
            
            // Extract all items in priority order (lowest priority number first)
            var sortedIssues = new List<Issue>();
            while (priorityHeap.Count > 0)
            {
                var minIssue = priorityHeap.ExtractMin();
                if (minIssue != null)
                {
                    sortedIssues.Add(minIssue);
                    Console.WriteLine($"Extracted: Priority {minIssue.Priority} - Issue #{minIssue.Id}"); // DEBUG
                }
            }

            Console.WriteLine("Sorting complete"); // DEBUG
            
            return _mapper.Map<List<ServiceRequestListViewModel>>(sortedIssues);
        }

        public async Task<List<Issue>> GetRequestDependenciesAsync(int id)
        {
            await EnsureInitializedAsync();

            var dependencyIds = _graph.BreadthFirstSearch(id).Skip(1).ToList();
            return _allIssues.Where(i => dependencyIds.Contains(i.Id)).ToList();
        }
        
        /// <summary>
        /// Search for service requests using multiple strategies
        /// </summary>
        public async Task<ServiceRequestSearchViewModel> SearchRequestsAsync(string? searchTerm, string? category)
        {
            await EnsureInitializedAsync();
            
            Console.WriteLine($"Search started - Term: '{searchTerm}', Category: '{category}'"); // DEBUG
            
            var results = _allIssues.AsQueryable();
            
            // STRATEGY 1: Search by ID using BST (fast O(log n) lookup)
            if (!string.IsNullOrWhiteSpace(searchTerm) && int.TryParse(searchTerm.Trim(), out int searchId))
            {
                Console.WriteLine($"Searching for ID {searchId} using BST..."); // DEBUG
                var issue = _bst.Search(searchId);
                
                if (issue != null)
                {
                    Console.WriteLine($"Found issue #{searchId}"); // DEBUG
                    // Found exact ID match using BST
                    return new ServiceRequestSearchViewModel
                    {
                        Results = new List<ServiceRequestListViewModel> 
                        { 
                            _mapper.Map<ServiceRequestListViewModel>(issue) 
                        },
                        SearchTerm = searchTerm,
                        CategoryFilter = category,
                        AvailableCategories = await GetAllCategoriesAsync(),
                        TotalResults = 1
                    };
                }
                else
                {
                    Console.WriteLine($"Issue #{searchId} not found"); // DEBUG
                    // ID not found
                    return new ServiceRequestSearchViewModel
                    {
                        Results = new List<ServiceRequestListViewModel>(),
                        SearchTerm = searchTerm,
                        CategoryFilter = category,
                        AvailableCategories = await GetAllCategoriesAsync(),
                        TotalResults = 0
                    };
                }
            }
            
            // STRATEGY 2: Text search in multiple fields (O(n) but simple)
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower();
                Console.WriteLine($"Text search for: '{term}'"); // DEBUG
                
                results = results.Where(i =>
                    (i.Category != null && i.Category.ToLower().Contains(term)) ||
                    (i.Location != null && i.Location.ToLower().Contains(term)) ||
                    (i.Description != null && i.Description.ToLower().Contains(term))
                );
            }
            
            // STRATEGY 3: Category filter (O(n))
            if (!string.IsNullOrWhiteSpace(category) && category != "All")
            {
                Console.WriteLine($"Filtering by category: '{category}'"); // DEBUG
                results = results.Where(i => 
                    i.Category != null && i.Category.Equals(category, StringComparison.OrdinalIgnoreCase)
                );
            }
            
            // Convert to list and sort by date (most recent first)
            var filteredIssues = results.OrderByDescending(i => i.DateReported).ToList();
            
            Console.WriteLine($"Search complete - {filteredIssues.Count} results found"); // DEBUG
            
            return new ServiceRequestSearchViewModel
            {
                Results = _mapper.Map<List<ServiceRequestListViewModel>>(filteredIssues),
                SearchTerm = searchTerm,
                CategoryFilter = category,
                AvailableCategories = await GetAllCategoriesAsync(),
                TotalResults = filteredIssues.Count
            };
        }
        
        /// <summary>
        /// Get all unique categories from issues
        /// </summary>
        public async Task<List<string>> GetAllCategoriesAsync()
        {
            await EnsureInitializedAsync();
            
            return _allIssues
                .Where(i => !string.IsNullOrEmpty(i.Category))
                .Select(i => i.Category!)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }
    }
}
