using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTree
{
    public class SequenceTreeNode : BehaviourTreeNode<Object>
    {
        private BehaviourTree.Node[] children;

        public SequenceTreeNode(BehaviourTree.Node[] children)
        {
            this.children = children;
        }

        public override BehaviourTree.State Execute(BehaviourTree tree)
        {
            state = BehaviourTree.State.SUCCESS;
            foreach (BehaviourTree.Node child in children)
            {
                state = child.Tick(tree);
                if (state != BehaviourTree.State.SUCCESS)
                {
                    return state;
                }
            }
            return state;
        }

        public override BehaviourTree.Node[] GetChildren()
        {
            return children;
        }
    }
}
