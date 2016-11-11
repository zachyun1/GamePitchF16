using System;

namespace BTree
{
	public class ActionTreeNode: BehaviourTreeNode
	{
		private Func<bool> action;

		public ActionTreeNode (Func<bool> action)
		{
			this.action = action;
		}

		public void Execute() {
			action.Invoke();
		}
	}
}

