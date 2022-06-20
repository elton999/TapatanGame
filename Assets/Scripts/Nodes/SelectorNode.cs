using System;

namespace BehaviorTrees
{
    public class SelectorNode : Node
    {
        public override NodeStatus Tick()
        {
            if (_nodes.Count == 0)
                throw new NullReferenceException();

            for (int i = 0; i < Count; i++)
            {
                if (_nodes[i].Tick() == NodeStatus.RUNNING)
                    return NodeStatus.RUNNING;

                if (_nodes[i].Tick() == NodeStatus.SUCCESS)
                    return NodeStatus.SUCCESS;
            }

            return NodeStatus.FAILURE;
        }
    }
}