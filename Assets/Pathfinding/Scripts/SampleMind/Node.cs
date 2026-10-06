using UnityEngine;

namespace Pathfinding
{
    public class Node
    {
        private Node parent;
        private float f = 0, g = 0, h = 0;
        public int posX, posY, goalX, goalY;
        private Node next, previous;

        public Node(int x, int y)
        {
            this.posX = x;
            this.posY = y;
        }

        public Node GetNext()
        {
            return this.next;
        }

        public void SetNext(Node node)
        {
            this.next = node;
        }

        public Node GetPrevious()
        {
            return this.previous;
        }

        public void SetPrevious(Node node)
        {
            this.previous = node;
        }

        public void SetGoal(int x, int y)
        {
            this.goalX = x;
            this.goalY = y;
        }

        public void SetParent(Node parent)
        {
            this.parent = parent;
        }

        public Node GetParent()
        {
            return this.parent;
        }

        public float GetG()
        {
            return this.g;
        }

        public float GetF()
        {
            return this.f;
        }

        public void CalculateF()
        {
            g = parent.GetG() + 1;
            h = Mathf.Abs(goalX - this.posX) + Mathf.Abs(goalY - this.posY);
            f = g + h;
        }

        public bool IsGoal()
        {
            if (posX == goalX && posY == goalY) return true;
            return false;
        }

        public bool IsFirstNode()
        {
            return parent == this;
        }
    }
}