using System.Collections.Generic;
namespace BehaviorTrees
{
    public abstract class Node
    {
        protected List<Node> _nodes = new List<Node>();

        public int Count { get => _nodes.Count; }

        public enum NodeStatus { RUNNING, FAILURE, SUCCESS };

        public abstract NodeStatus Tick();

        public virtual void Add(Node node) => _nodes.Add(node);
    }
}