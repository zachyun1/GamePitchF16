using System;

namespace BTree
{
	public class ActionTreeNode<T>: BehaviourTreeNode<T>
	{
		public Func<BehaviourTreeNode<T>, BehaviourTree.State> Action;

		public ActionTreeNode (Func<BehaviourTreeNode<T>, BehaviourTree.State> action)
		{
			this.Action = action;
		}

		public override BehaviourTree.State Execute(BehaviourTree tree)
        {
            state = Action.Invoke(this);
            return state;
		}

        public override BehaviourTree.Node[] GetChildren()
        {
            return new BehaviourTree.Node[] { };
        }
    }
}

