using System;

namespace BehaviorTrees
{
    public class SucceederNode : DecoratorNode
    {
        public SucceederNode(Node node) : base(node) { }

        public override NodeStatus Tick()
        {
            if (_node.Tick() == NodeStatus.RUNNING)
                return NodeStatus.RUNNING;

            return NodeStatus.SUCCESS;
        }
    }
}