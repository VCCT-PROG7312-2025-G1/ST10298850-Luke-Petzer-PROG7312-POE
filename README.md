# Municipal Services Portal

> Portfolio of Evidence for PROG7312 - ASP.NET Core MVC Application

This is my Portfolio of Evidence for PROG7312, an ASP.NET Core MVC application for a municipal services portal. The project is built using a clean architecture (MVCS + Repository Pattern) and demonstrates the practical use of advanced data structures.

---

## 📚 Table of Contents

- [Repository & Video Links](#-repository--video-links)
- [Project Features](#-project-features)
- [Why SQLite?](#-why-sqlite)
- [Technologies & Patterns](#-technologies--patterns)
- [Project Structure](#-project-structure)
- [Setup & Installation](#-setup--installation)
- [Usage Guide](#-usage-guide)
- [Part 3: Data Structures Explained](#-part-3-data-structures-explained)

---

## 🔗 Repository & Video Links

**GitHub Repository:**  
https://github.com/ST10298850/ST10298850-Luke-Petzer-PROG7312-POE.git

**YouTube Video Demo:**  
https://youtu.be/Y9FKudbk_m0

---

## 🎯 Project Features

This application is built in three main parts:

### 1️⃣ Report Issues
This feature allows users to submit service requests (like potholes, graffiti, or water leaks) to the municipality. The form collects a location, category, description, and allows for file uploads. All submissions are saved to the database.

### 2️⃣ Local Events
This page displays upcoming events and announcements. I built a search and filter system for this, along with a recommendation feature that suggests other events based on what the user has searched for in their current session.

### 3️⃣ Track Service Requests (Part 3)
This is the main feature of the POE. It's a dashboard that allows users to track the status of all submitted issues. It uses several data structures to work efficiently:

- **Binary Search Tree (BST):** Used for instant lookups when a user searches for a specific issue ID.
- **Min-Heap:** Used to provide a "Sort by Priority" view, so critical issues (like gas leaks) always appear at the top.
- **Graph:** Used to model and visualize dependencies between tasks (e.g., "Repave Road" can't start until "Fix Water Main" is complete).

---

## 💾 Why SQLite?

This project uses **SQLite** as its database. I chose it for several key reasons:

| Advantage | Description |
|-----------|-------------|
| **Zero Configuration** | It requires no database server (like SQL Server or MySQL) to be installed. It just works out of the box. |
| **Cross-Platform** | The project runs identically on Windows, macOS, or Linux. |
| **Portability** | The entire database is a single file (`municipal_services.db`), which makes it perfect for an academic project. |
| **No Hassle** | It eliminated any potential setup or environment issues for my lecturer, allowing them to focus on the application's logic. |

---

## 🛠️ Technologies & Patterns

### Core Technologies
- **Framework:** ASP.NET Core MVC (.NET 8)
- **Language:** C# 12
- **ORM:** Entity Framework Core 8
- **Database:** SQLite 3 (file-based)
- **Frontend:** Razor Views, Bootstrap 5, Bootstrap Icons, custom CSS (using BEM)

### Architecture Patterns
- **MVCS** (Model-View-Controller-Service)
- **Repository Pattern**
- **Service Layer**
- **Dependency Injection**
- **AutoMapper**

### Data Structures

**Part 2 - Local Events:**
- `SortedDictionary`
- `Dictionary`
- `HashSet`
- `PriorityQueue`

**Part 3 - Service Request Status:**
- Custom-built `BinarySearchTree<T>`
- Custom-built `MinHeap<T>`
- Custom-built `Graph<T>`

---

## 📁 Project Structure

I focused on a clean architecture. Here are the most important folders and what they do:

```
Municipal-Servcies-Portal/
├── Controllers/
│   ├── HomeController.cs
│   ├── IssueController.cs              # Database-backed issue reporting
│   ├── LocalEventsController.cs        # Events, filtering, recommendations
│   └── ServiceRequestController.cs     # Service request tracking & search (Part 3)
│
├── Data/
│   ├── AppDbContext.cs                 # EF Core DbContext
│   └── DbSeeder.cs                     # Seeds events + service requests with dependencies
│
├── DataStructures/                     # Part 3: Advanced data structures
│   ├── BinarySearchTree.cs             # O(log n) ID lookups
│   ├── MinHeap.cs                      # Priority queue implementation
│   └── Graph.cs                        # Dependency tracking with BFS
│
├── Extensions/
│   └── ServiceCollectionExtensions.cs  # DI configuration
│
├── Mapping/
│   └── MappingProfile.cs               # AutoMapper configuration
│
├── Migrations/
│   ├── 20251014160425_InitialCreate.cs
│   ├── 20251015092707_AddIssuesTable.cs
│   └── 20251109195810_AddPriorityAndDependenciesToIssue.cs
│
├── Models/
│   ├── Issue.cs                        # Database entity with validation
│   ├── Event.cs                        # Event entity
│   ├── Announcement.cs                 # Announcement entity
│   └── ErrorViewModel.cs
│
├── Repositories/                       # Data access layer
│   ├── IRepository.cs                  # Generic repository interface
│   ├── Repository.cs                   # Generic repository implementation
│   ├── IIssueRepository.cs             # Issue-specific repository
│   ├── IssueRepository.cs
│   ├── IEventRepository.cs             # Event-specific repository
│   └── EventRepository.cs
│
├── Services/                           # Business logic layer
│   ├── IIssueService.cs                # Issue service interface
│   ├── IssueService.cs                 # Database CRUD for issues
│   ├── ILocalEventsService.cs          # Events service interface
│   ├── LocalEventsService.cs           # Advanced data structures & search
│   ├── SearchHistoryService.cs         # Session-based tracking
│   ├── IServiceRequestService.cs       # Service request interface (Part 3)
│   └── ServiceRequestService.cs        # Search, filter, BST/Heap/Graph operations
│
├── ViewModels/
│   ├── LocalEventsViewModel.cs         # Events composite view model
│   ├── IssueCreateViewModel.cs         # Issue creation
│   ├── IssueViewModel.cs               # Issue display
│   ├── ServiceRequestListViewModel.cs  # Service request list (Part 3)
│   ├── ServiceRequestDetailViewModel.cs # Service request details
│   └── ServiceRequestSearchViewModel.cs # Search wrapper with filters
│
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml
│   │   └── ComingSoon.cshtml
│   ├── Issue/
│   │   ├── Create.cshtml
│   │   └── Confirmation.cshtml         # Shows DB reference number
│   ├── LocalEvents/
│   │   └── Index.cshtml                # Search, filter, recommendations
│   ├── ServiceRequest/                 # Part 3: Service request tracking
│   │   ├── Index.cshtml                # Search interface + table view
│   │   └── Details.cshtml              # Request details with dependencies
│   └── Shared/
│       ├── _Layout.cshtml              # Includes Bootstrap Icons CDN
│       ├── _ValidationScriptsPartial.cshtml
│       └── Error.cshtml
│
├── wwwroot/
│   ├── css/
│   │   ├── site.css
│   │   ├── home.css
│   │   ├── issues.css
│   │   ├── localevents.css
│   │   ├── servicerequest.css          # Part 3: Table + search styles (BEM)
│   │   ├── confirmation.css
│   │   └── comingsoon.css
│   ├── images/icons/
│   ├── js/
│   │   ├── site.js
│   │   └── localevents.js
│   └── uploads/                        # User-uploaded files
│
├── appsettings.json                    # Connection strings
├── appsettings.Development.json
├── Program.cs                          # DI, services, middleware, session config
└── Municipal-Servcies-Portal.csproj
```

---

## 🚀 Setup & Installation

### Prerequisites
- .NET 8.0 SDK
- A code editor (like Visual Studio 2022 or VS Code)

### Installation Steps

#### 1. Clone the Repository
```bash
git clone https://github.com/ST10298850/ST10298850-Luke-Petzer-PROG7312-POE.git
cd ST10298850-Luke-Petzer-PROG7312-POE/Municipal-Servcies-Portal
```

#### 2. Database Configuration (No Setup Required!)
The project uses SQLite. The database file `municipal_services.db` will be **automatically created** in the project directory when you run the application.

The connection string is already set up in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=municipal_services.db"
}
```

#### 3. Restore Dependencies
```bash
dotnet restore
```

#### 4. Apply Database Migrations
This command will automatically create the `municipal_services.db` file and build all the tables and seed them with test data.

```bash
dotnet ef database update
```

**Expected Output:**
```
Build succeeded.
Applying migration '20251014160425_InitialCreate'.
Applying migration '20251015092707_AddIssuesTable'.
Applying migration '20251109195810_AddPriorityAndDependenciesToIssue'.
Done.
```

This seeds the database with:
- 27 sample events for Part 2
- 23 sample service requests with pre-defined priorities and dependencies to test the Part 3 features

#### 5. Build and Run
```bash
dotnet run
```

Or, if using Visual Studio, just press **F5**.

#### 6. Open in Browser
Navigate to the URL shown in the terminal (e.g., `https://localhost:5001`).

---

## 📖 Usage Guide

### Reporting an Issue

1. Click **"Report Issues"** in the navigation bar
2. Fill in the form (location, category, description)
3. Optionally, drag and drop image files into the upload box
4. Click **"Submit Report"**
5. You will be redirected to a confirmation page with your unique tracking number

### Browsing Local Events

1. Click **"Local Events"** in the navigation bar
2. Use the search box, category dropdown, or date picker to filter events
3. The page updates automatically as you type
4. The **"Recommended for You"** section will update based on your search history

### Tracking a Service Request (Part 3)

#### 1. View All Requests
1. Click **"Service Request Status"** in the navigation bar
2. You will see a table of all submitted issues

#### 2. Search for Requests

**Quick ID Search (BST):**
- Type an ID number (e.g., "10") into the search box
- Click "Search" for an instant result

**Text Search:**
- Type a keyword (e.g., "pothole")
- Searches across location, category, and description

**Category Filter:**
- Use the dropdown to see all issues for a specific category

#### 3. Sort by Priority (Heap)
- Click the **"Sort by Priority"** button
- The list will re-order, showing "Priority 1" (Critical) issues at the top
- Thanks to the Min-Heap implementation

#### 4. Track Dependencies (Graph)
- Click the **"View"** button on any issue
- On the "Details" page, scroll down to the **"Dependencies"** section
- If an issue is blocked by another, it will be shown here
- Example: `Issue #21: Install streetlights` might be blocked by `Issue #18: Repave road`

---

## 🔍 Part 3: Data Structures Explained

This section provides the in-depth explanation of *how* the data structures for Part 3 were implemented and *why* they were chosen.

### Architecture: Hybrid In-Memory Model

To meet the POE requirements for efficiency, I did not query the database for every single operation. Instead, I used a hybrid approach:

1. When the `ServiceRequestService` is first used, it fetches all `Issues` from the database (via the `IssueRepository`) into a `List<Issue>`
2. It then populates the **BST**, **Min-Heap**, and **Graph** data structures in memory
3. All "read" operations (searching, sorting, tracking dependencies) are then performed instantly on these in-memory structures

```
┌─────────────┐     ┌──────────────────┐     ┌─────────────────┐
│  Database   │ ──> │ IssueRepository  │ ──> │ ServiceRequest  │
│  (SQLite)   │     │   (EF Core)      │     │    Service      │
└─────────────┘     └──────────────────┘     └─────────────────┘
                                                       │
                           ┌───────────────────────────┼───────────────────────────┐
                           │                           │                           │
                    ┌──────▼──────┐           ┌────────▼────────┐       ┌─────────▼────────┐
                    │   Binary    │           │    Min Heap     │       │      Graph       │
                    │ Search Tree │           │ (Priority Q.)   │       │  (Dependencies)  │
                    └─────────────┘           └─────────────────┘       └──────────────────┘
```

---

### Data Structure 1: Binary Search Tree (BST)

#### Role in Feature
The BST is used for the **"Quick ID Search"** feature. Its sole purpose is to find a specific issue by its `Id` as fast as possible.

#### Implementation Details
- A custom generic `BinarySearchTree<T>` class was built in `DataStructures/BinarySearchTree.cs`
- In the `ServiceRequestService`, I loop through all issues and call `_bst.Insert(issue.Id, issue)`
- The `Id` is the key, and the `Issue` object is the data
- When the user searches for an ID, the service calls `_bst.Search(key)`

#### Efficiency Contribution

| Operation | Linear Search (`List.First()`) | Binary Search Tree (`Search()`) |
|-----------|--------------------------------|----------------------------------|
| **Time (Big O)** | **O(n)** | **O(log n)** |
| **Comparisons** | 10,000 (worst case) | ~14 (worst case) |

**Example:**

If there are 10,000 issues in the system:
- A **Linear Search** (like `List.First(i => i.Id == 9876)`) would have to check each item one by one. In the worst case, it would take **10,000 comparisons**.
- A **BST Search** divides the search area in half with each step. It would take a maximum of **~14 comparisons** (since log₂(10,000) ≈ 13.3).
- This makes the search feel instantaneous, regardless of the database size.

#### Real-World Benefit
It provides an instant, sub-second lookup for users and admins, even if the system scales to millions of service requests.

---

### Data Structure 2: Min-Heap (Priority Queue)

#### Role in Feature
The Min-Heap is used to implement the **"Sort by Priority"** page. It ensures that the most critical issues (e.g., "Priority 1: Gas Leak") are always at the top of the list, allowing municipal staff to address them first.

#### Implementation Details
- A custom generic `MinHeap<T>` class was built in `DataStructures/MinHeap.cs`
- It uses a `List<T>` internally to manage the heap structure
- When the user clicks the "Sort by Priority" button, the service populates a *new* Min-Heap by calling `_heap.Insert(issue.Priority, issue)` for all issues
- It then calls `_heap.ExtractMin()` repeatedly to pull items out in perfect priority order (Priority 1 first, then 2, etc.)

#### Efficiency Contribution

| Operation | `List.Sort()` | Min-Heap |
|-----------|---------------|----------|
| **Time (Big O)** | **O(n log n)** | **O(n log n)** (to build + extract all) |
| **Get Next Item** | (N/A) | **O(log n)** (to extract) or **O(1)** (to peek) |

The *real* benefit of a heap isn't just sorting an entire list; it's efficiently managing a *dynamic* queue where new, high-priority items can be added and immediately "bubble up" to the top.

#### Example Scenario

A service desk is processing requests:

1. Queue: `[#38: Streetlight (P3)]`, `[#51: Graffiti (P4)]`
2. A new request comes in: `[#42: Gas Leak (P1)]`
3. **Without a Heap (FIFO):** The P1 request is stuck behind the P3 and P4 issues
4. **With a Min-Heap:** The `Insert()` operation places the P1 request at the top of the queue in `O(log n)` time. The next `ExtractMin()` call will immediately return the gas leak, preventing a critical issue from being missed.

#### Real-World Benefit
The Min-Heap provides a structurally sound way to manage a priority queue, ensuring critical issues are never lost in the list and are always processed first.

---

### Data Structure 3: Graph (Dependency Tracking)

#### Role in Feature
The Graph is used to model and visualize **dependencies between service requests**. This is crucial for project management, as some tasks cannot begin until others are finished.

#### Implementation Details
- A custom `Graph<T>` class was built in `DataStructures/Graph.cs`
- It uses a `Dictionary<T, List<T>>` to store an adjacency list (e.g., `Issue_ID -> List_of_Dependencies`)
- The `Issue` model was updated with a `DependenciesJson` field, which stores a list of other `Issue.Id`s (e.g., `[8, 12]`)
- When the service initializes, it populates the graph:
  1. `_graph.AddVertex(issue.Id)` for every issue
  2. `_graph.AddEdge(issue.Id, dependencyId)` for every item in the `Dependencies` list
- When a user views the "Details" page, the service calls `_graph.BreadthFirstSearch(currentIssueId)` to find all connected nodes in the dependency chain

#### Efficiency Contribution

| Operation | Naive Recursive Search | Graph with BFS |
|-----------|------------------------|----------------|
| **Time (Big O)** | **O(n!)** (in worst case) | **O(V + E)** |

Where `V` is the number of issues (vertices) and `E` is the number of dependencies (edges).

#### Example Scenario

We have a complex project:
- `Issue #15: Fix water main`
- `Issue #18: Repave road` (Depends on #15)
- `Issue #21: Paint road markings` (Depends on #18)

When a citizen views `Issue #21`, they are confused about why it's "Pending."

- The `Graph.BreadthFirstSearch(21)` is called, which returns `[21, 18, 15]`
- The "Details" page shows the user the full chain:

```
Service Request #21: Paint road markings
Status: Waiting on Dependencies

Dependencies:
├─ #18: Repave road (In Progress)
│   └─ #15: Fix water main (Resolved)
└─ Estimated start: After #18 completes
```

#### Real-World Benefit
The Graph provides an extremely fast `O(V + E)` way to traverse complex relationships. This stops staff from working in the wrong order and clearly communicates to citizens *why* their request is delayed, reducing confusion.

---

### Design Decisions and Trade-offs

#### Why In-Memory Data Structures?

For this POE, I used in-memory data structures (loaded from the database at startup) instead of querying the database every time.

**✅ Advantages:**
- **Performance:** All lookups, sorting, and traversals are "lightning-fast" because they happen in RAM, not on disk
- **Meets POE:** It allowed me to demonstrate my custom-built BST, Heap, and Graph classes, which was a core requirement
- **Reduced DB Load:** The database is only hit once on initialization, not every time a user searches or sorts

**⚠️ Trade-offs (and real-world solutions):**

| Issue | Real-World Solution |
|-------|---------------------|
| **Stale Data:** If a new issue is added, the in-memory structures won't know about it until the application is restarted | Use a **Distributed Cache (like Redis)** to hold the data structures, which keeps them in-memory and fast, but also ensures all users see the same, up-to-date data |
| **Memory Usage:** If there were 100 million issues, this would use a lot of RAM | For that scale, move the logic into the database itself, using database indexes (which are B-Trees, a type of BST) and "Recursive CTEs" (for graph traversal) in SQL |

---

## 📝 License

This project is an academic submission for PROG7312.

---

**Author:** Luke Petzer  
**Student Number:** ST10298850  
**Institution:** The Independent Institute of Education  
**Date:** November 2025

