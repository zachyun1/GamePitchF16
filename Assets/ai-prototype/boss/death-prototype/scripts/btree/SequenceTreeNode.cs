using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTree
{
    public class SequenceTreeNode : BehaviourTree.Node
    {
        private BehaviourTree.Node[] children;

        public SequenceTreeNode(BehaviourTree.Node[] children)
        {
            this.children = children;
        }

        public override void Execute(BehaviourTree tree)
        {
            foreach (BehaviourTree.Node child in children)
            {
                child.Tick(tree);
                BehaviourTree.State state = child.State;
                if (state == BehaviourTree.State.FAILURE)
                {
                    this.State = state;
                    return;
                }
                else if (!child.IsComplete())
                {
                    return;
                }
            }
            State = BehaviourTree.State.SUCCESS;
        }

        public override BehaviourTree.Node[] GetChildren()
        {
            return children;
        }
    }
}
