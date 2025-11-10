namespace Municipal_Servcies_Portal.DataStructures
{
    public class BinarySearchTree<T>
    {
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

        public void Insert(int key, T data)
        {
            _root = InsertRecursive(_root, key, data);
        }

        private Node InsertRecursive(Node? node, int key, T data)
        {
            if (node == null)
                return new Node(key, data);

            if (key < node.Key)
                node.Left = InsertRecursive(node.Left, key, data);
            else if (key > node.Key)
                node.Right = InsertRecursive(node.Right, key, data);

            return node;
        }

        public T? Search(int key)
        {
            var result = SearchRecursive(_root, key);
            return result != null ? result.Data : default;
        }

        private Node? SearchRecursive(Node? node, int key)
        {
            if (node == null || node.Key == key)
                return node;

            if (key < node.Key)
                return SearchRecursive(node.Left, key);

            return SearchRecursive(node.Right, key);
        }

        public void Clear()
        {
            _root = null;
        }
    }
}