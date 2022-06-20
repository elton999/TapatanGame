using System;

namespace BehaviorTrees
{
    public class SequenceNode : Node
    {
        public override NodeStatus Tick()
        {
            if (_nodes.Count == 0)
                throw new NullReferenceException();

            for (int i = 0; i < Count; i++)
            {
                if (_nodes[i].Tick() == NodeStatus.RUNNING)
                    return NodeStatus.RUNNING;

                if (_nodes[i].Tick() == NodeStatus.FAILURE)
                    return NodeStatus.FAILURE;
            }

            return NodeStatus.SUCCESS;
        }
    }
}