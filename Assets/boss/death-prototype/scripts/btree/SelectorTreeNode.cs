using System;

namespace BTree
{
	public class SelectorTreeNode: BehaviourTreeNode
	{
		private Func<bool> condition;
		private BehaviourTreeNode[] actionsIfTrue;
		private BehaviourTreeNode[] actionsIfFalse;

		public SelectorTreeNode (Func<bool> condition, BehaviourTreeNode[] actionsIfTrue, BehaviourTreeNode[] actionsIfFalse)
		{
			this.condition = condition;
			this.actionsIfTrue = actionsIfTrue;
			this.actionsIfFalse = actionsIfFalse;
		}

		public void Execute() {
			BehaviourTreeNode[] actions = condition.Invoke() ? actionsIfTrue : actionsIfFalse;
			foreach (BehaviourTreeNode action in actions) {
				action.Execute();
			}
		}
	}
}

