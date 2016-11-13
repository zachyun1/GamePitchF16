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

		public override void Execute(BehaviourTree tree) {
            Result = condition.Invoke();
            BehaviourTree.Node action = Result ? actionIfTrue : actionIfFalse;
            action.Tick(tree);
            if (action.IsComplete())
            {
                State = BehaviourTree.State.SUCCESS;
            }
		}

        public override BehaviourTree.Node[] GetChildren()
        {
            return new BehaviourTree.Node[] { Result ? actionIfTrue : actionIfFalse };
        }
    }
}

