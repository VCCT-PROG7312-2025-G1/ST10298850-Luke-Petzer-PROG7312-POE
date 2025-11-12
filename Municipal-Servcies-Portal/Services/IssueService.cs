using AutoMapper;
using Municipal_Servcies_Portal.Models;
using Municipal_Servcies_Portal.Repositories;
using Municipal_Servcies_Portal.ViewModels;

namespace Municipal_Servcies_Portal.Services
{
    /// <summary>
    /// Service layer for managing issues using repository pattern and AutoMapper.
    /// Handles business logic and file uploads.
    /// </summary>
    public class IssueService : IIssueService
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public IssueService(
            IIssueRepository issueRepository,
            IMapper mapper,
            IWebHostEnvironment environment)
        {
            _issueRepository = issueRepository;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<int> CreateIssueAsync(IssueCreateViewModel viewModel)
        {
            // Map ViewModel to Model
            var issue = _mapper.Map<Issue>(viewModel);

            // Set properties not in ViewModel
            issue.Status = "Pending";
            issue.DateReported = DateTime.Now;
            issue.IsActive = true;

            // Handle file uploads if present
            if (viewModel.Attachments != null && viewModel.Attachments.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var uploadedFiles = new List<string>();

                foreach (var file in viewModel.Attachments)
                {
                    if (file != null && file.Length > 0)
                    {
                        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        uploadedFiles.Add(uniqueFileName);
                    }
                }

                issue.AttachmentPaths = uploadedFiles;
            }

            // Save to database
            var savedIssue = await _issueRepository.AddAsync(issue);
            
            // Return the generated ID
            return savedIssue.Id;
        }

        public async Task<IEnumerable<IssueViewModel>> GetRecentIssuesAsync(int count)
        {
            var issues = await _issueRepository.GetRecentIssuesAsync(count);
            return _mapper.Map<IEnumerable<IssueViewModel>>(issues);
        }

        public async Task<IssueViewModel?> GetIssueByIdAsync(int id)
        {
            var issue = await _issueRepository.GetByIdAsync(id);
            return issue != null ? _mapper.Map<IssueViewModel>(issue) : null;
        }
    }
}

