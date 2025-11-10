namespace Municipal_Servcies_Portal.DataStructures
{
    // MinHeap implementation for priority queue
    // Smallest priority value is always at the root (index 0)
    // Used to sort service requests by priority level
    public class MinHeap<T>
    {
        // Node to store priority and data together
        private class HeapNode
        {
            public int Priority { get; set; }
            public T Data { get; set; }

            public HeapNode(int priority, T data)
            {
                Priority = priority;
                Data = data;
            }
        }

        private readonly List<HeapNode> _heap;

        public MinHeap()
        {
            _heap = new List<HeapNode>();
        }

        public int Count => _heap.Count;

        // Insert a new element with its priority
        public void Insert(int priority, T data)
        {
            // Add to end of list
            _heap.Add(new HeapNode(priority, data));
            
            // Fix heap property by moving up
            HeapifyUp(_heap.Count - 1);
        }

        // Remove and return the minimum element (root)
        public T? ExtractMin()
        {
            if (_heap.Count == 0)
                return default(T);

            // Save the minimum value
            var min = _heap[0].Data;
            
            // Move last element to root
            _heap[0] = _heap[_heap.Count - 1];
            _heap.RemoveAt(_heap.Count - 1);

            // Fix heap property by moving down
            if (_heap.Count > 0)
                HeapifyDown(0);

            return min;
        }

        // Clear all elements
        public void Clear()
        {
            _heap.Clear();
        }

        // Move element up to maintain heap property
        // TODO: This could be optimized with iterative approach
        private void HeapifyUp(int index)
        {
            // Keep moving up until we're at root or parent is smaller
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                
                // If parent is smaller, we're done
                if (_heap[index].Priority >= _heap[parentIndex].Priority)
                    break;
                
                // Swap with parent (old-school way)
                var temp = _heap[index];
                _heap[index] = _heap[parentIndex];
                _heap[parentIndex] = temp;
                
                index = parentIndex;
            }
        }

        // Move element down to maintain heap property
        private void HeapifyDown(int index)
        {
            while (true)
            {
                int leftChild = 2 * index + 1;
                int rightChild = 2 * index + 2;
                int smallest = index;

                // Check if left child is smaller
                if (leftChild < _heap.Count && _heap[leftChild].Priority < _heap[smallest].Priority)
                    smallest = leftChild;

                // Check if right child is smaller
                if (rightChild < _heap.Count && _heap[rightChild].Priority < _heap[smallest].Priority)
                    smallest = rightChild;

                // If current node is smallest, we're done
                if (smallest == index)
                    break;

                // Swap with smallest child
                var temp = _heap[index];
                _heap[index] = _heap[smallest];
                _heap[smallest] = temp;
                
                index = smallest;
            }
        }
    }
}
