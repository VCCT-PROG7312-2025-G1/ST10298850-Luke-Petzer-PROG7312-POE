namespace Municipal_Servcies_Portal.DataStructures
{
    // Graph implementation using adjacency list
    // This is used to track dependencies between service requests
    // TODO: Maybe add weighted edges later for priority levels?
    public class Graph<T> where T : notnull
    {
        // Adjacency list - each vertex maps to a list of its neighbors
        private readonly Dictionary<T, List<T>> _adjacencyList;

        public Graph()
        {
            _adjacencyList = new Dictionary<T, List<T>>();
        }

        // Add a vertex (node) to the graph
        // TODO: Should we check for duplicates here?
        public void AddVertex(T vertex)
        {
            if (!_adjacencyList.ContainsKey(vertex))
            {
                _adjacencyList[vertex] = new List<T>();
            }
        }

        // Add a directed edge from source to destination
        // Represents a dependency relationship
        public void AddEdge(T source, T destination)
        {
            // Make sure both vertices exist
            if (!_adjacencyList.ContainsKey(source))
                AddVertex(source);

            if (!_adjacencyList.ContainsKey(destination))
                AddVertex(destination);

            // Add the edge if it doesn't already exist
            if (!_adjacencyList[source].Contains(destination))
                _adjacencyList[source].Add(destination);
        }

        // Breadth-First Search traversal starting from a vertex
        // This finds all connected nodes (dependencies)
        public List<T> BreadthFirstSearch(T startVertex)
        {
            // BFS to find all connected nodes
            var visited = new HashSet<T>();
            var queue = new Queue<T>();
            var result = new List<T>();

            // Check if vertex exists in graph
            if (!_adjacencyList.ContainsKey(startVertex))
                return result; // Return empty list if not found

            // Start BFS
            queue.Enqueue(startVertex);
            visited.Add(startVertex);

            // Process queue until empty
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                result.Add(current);

                // Get neighbors of current vertex
                if (_adjacencyList.ContainsKey(current)) // Double-check it exists
                {
                    var neighbors = _adjacencyList[current];
                    
                    // Visit each unvisited neighbor
                    foreach (var neighbor in neighbors)
                    {
                        if (!visited.Contains(neighbor))
                        {
                            visited.Add(neighbor);
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }

            return result;
        }

        // Clear all vertices and edges
        public void Clear()
        {
            _adjacencyList.Clear();
        }
    }
}