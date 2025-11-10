namespace Municipal_Servcies_Portal.DataStructures
{
    public class Graph<T> where T : notnull
    {
        private readonly Dictionary<T, List<T>> _adjacencyList;

        public Graph()
        {
            _adjacencyList = new Dictionary<T, List<T>>();
        }

        public void AddVertex(T vertex)
        {
            if (!_adjacencyList.ContainsKey(vertex))
            {
                _adjacencyList[vertex] = new List<T>();
            }
        }

        public void AddEdge(T source, T destination)
        {
            if (!_adjacencyList.ContainsKey(source))
                AddVertex(source);

            if (!_adjacencyList.ContainsKey(destination))
                AddVertex(destination);

            if (!_adjacencyList[source].Contains(destination))
                _adjacencyList[source].Add(destination);
        }

        public List<T> BreadthFirstSearch(T startVertex)
        {
            var visited = new HashSet<T>();
            var result = new List<T>();
            var queue = new Queue<T>();

            if (!_adjacencyList.ContainsKey(startVertex))
                return result;

            queue.Enqueue(startVertex);
            visited.Add(startVertex);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                result.Add(current);

                foreach (var neighbor in _adjacencyList[current])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return result;
        }

        public void Clear()
        {
            _adjacencyList.Clear();
        }
    }
}