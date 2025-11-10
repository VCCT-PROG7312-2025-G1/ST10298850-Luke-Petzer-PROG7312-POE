namespace Municipal_Servcies_Portal.DataStructures
{
    public class MinHeap<T>
    {
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

        public void Insert(int priority, T data)
        {
            _heap.Add(new HeapNode(priority, data));
            HeapifyUp(_heap.Count - 1);
        }

        public T? ExtractMin()
        {
            if (_heap.Count == 0)
                return default;

            var min = _heap[0].Data;
            _heap[0] = _heap[_heap.Count - 1];
            _heap.RemoveAt(_heap.Count - 1);

            if (_heap.Count > 0)
                HeapifyDown(0);

            return min;
        }

        public void Clear()
        {
            _heap.Clear();
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;

                if (_heap[index].Priority >= _heap[parentIndex].Priority)
                    break;

                Swap(index, parentIndex);
                index = parentIndex;
            }
        }

        private void HeapifyDown(int index)
        {
            while (true)
            {
                int leftChild = 2 * index + 1;
                int rightChild = 2 * index + 2;
                int smallest = index;

                if (leftChild < _heap.Count && _heap[leftChild].Priority < _heap[smallest].Priority)
                    smallest = leftChild;

                if (rightChild < _heap.Count && _heap[rightChild].Priority < _heap[smallest].Priority)
                    smallest = rightChild;

                if (smallest == index)
                    break;

                Swap(index, smallest);
                index = smallest;
            }
        }

        private void Swap(int i, int j)
        {
            var temp = _heap[i];
            _heap[i] = _heap[j];
            _heap[j] = temp;
        }
    }
}
