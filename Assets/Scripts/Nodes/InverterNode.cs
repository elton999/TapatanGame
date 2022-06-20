using System;

namespace BehaviorTrees
{
    public class InverterNode : DecoratorNode
    {
        public InverterNode(Node node) : base(node) { }

        public override NodeStatus Tick()
        {
            if (_node.Tick() == NodeStatus.FAILURE)
                return NodeStatus.SUCCESS;

            if (_node.Tick() == NodeStatus.SUCCESS)
                return NodeStatus.FAILURE;

            return NodeStatus.RUNNING;
        }
    }
}