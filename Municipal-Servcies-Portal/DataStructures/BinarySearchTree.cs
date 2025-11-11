namespace Municipal_Servcies_Portal.DataStructures
{
    // Binary Search Tree for fast searching
    // This helps find service requests quickly by ID
    public class BinarySearchTree<T>
    {
        // Internal node structure for the tree
        private class Node
        {
            public int Key { get; set; }
            public T Data { get; set; }
            public Node? Left { get; set; }
            public Node? Right { get; set; }

            public Node(int key, T data)
            {
                Key = key;
                Data = data;
            }
        }

        private Node? _root;

        // Insert a new item into the tree
        public void Insert(int key, T data)
        {
            _root = InsertRecursive(_root, key, data);
        }

        // Recursive insert helper method
        // Smaller keys go to the left, larger keys go to the right
        private Node InsertRecursive(Node? node, int key, T data)
        {
            // Base case: found the spot to insert
            if (node == null)
                return new Node(key, data);

            // Recursively insert into left or right subtree
            if (key < node.Key)
                node.Left = InsertRecursive(node.Left, key, data);
            else if (key > node.Key)
                node.Right = InsertRecursive(node.Right, key, data);
            // Note: if key equals node.Key, we don't insert duplicates

            return node;
        }

        // Search for an item by key
        // TODO: Could optimize this with iterative approach instead of recursion
        public T? Search(int key)
        {
            var result = SearchRecursive(_root, key);
            return result != null ? result.Data : default(T);
        }

        // Recursive search helper
        // This gives us O(log n) time complexity for balanced trees
        private Node? SearchRecursive(Node? node, int key)
        {
            // Base case - not found
            if (node == null)
                return null;

            // Found it!
            if (key == node.Key)
                return node;

            // Search left or right based on key
            // Smaller keys go left, larger go right
            if (key < node.Key)
                return SearchRecursive(node.Left, key);
            else
                return SearchRecursive(node.Right, key);
        }

        // Clear the entire tree
        public void Clear()
        {
            _root = null; // Garbage collector will clean up the nodes
        }
    }
}