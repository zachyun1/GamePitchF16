using System;

namespace BTree
{
	public class SelectorTreeNode : BehaviourTreeNode<bool>
	{
		private Func<bool> condition;
		private BehaviourTree.Node actionIfTrue;
		private BehaviourTree.Node actionIfFalse;

		public SelectorTreeNode (Func<bool> condition, BehaviourTree.Node actionIfTrue, BehaviourTree.Node actionIfFalse)
		{
			this.condition = condition;
			this.actionIfTrue = actionIfTrue;
			this.actionIfFalse = actionIfFalse;
		}

		public override BehaviourTree.State Execute(BehaviourTree tree) {
            Result = condition.Invoke();
            if (Result) {
                state = actionIfTrue.Tick(tree);
            } else
            {
                state = actionIfFalse.Tick(tree);
            }
            return state;
		}

        public override BehaviourTree.Node[] GetChildren()
        {
            return new BehaviourTree.Node[] { Result ? actionIfTrue : actionIfFalse };
        }
    }
}

