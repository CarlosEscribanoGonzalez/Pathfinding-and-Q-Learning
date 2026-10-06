namespace Pathfinding
{
    public class SortedQueue
    {
        public Node first, last;
        private float length = 0;

        public Node Pop()
        {
            Node aux = this.first;
            if (first.GetNext() != null) this.first = this.first.GetNext();
            first.SetPrevious(null);
            length--;
            return aux;
        }

        public void Add(Node node)
        {
            Node currentNode = this.first;
            bool added = false;
            for (int i = 0; i < length && !added; i++)
            {
                if (node.GetF() < currentNode.GetF())
                {
                    if (i == 0) first = node;
                    node.SetNext(currentNode);
                    node.SetPrevious(currentNode.GetPrevious());
                    if (currentNode.GetPrevious() != null) currentNode.GetPrevious().SetNext(node);
                    currentNode.SetPrevious(node);
                    added = true;
                }

                if (i == length - 1) last = currentNode;

                currentNode = currentNode.GetNext();
            }
            if (!added) 
            {
                if (length == 0)
                {
                    first = node;
                    last = node;
                }
                else
                {
                    last.SetNext(node);
                    node.SetPrevious(last);
                    last = node;
                }
            }
            length++;
        }
    }
}