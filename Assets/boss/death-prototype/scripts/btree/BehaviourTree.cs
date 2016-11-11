using System;
using UnityEngine;
using BTree;

namespace BTree
{
	public class BehaviourTree
	{
		private BehaviourTreeNode rootNode;

		public BehaviourTree (BehaviourTreeNode rootNode, GameObject actor)
		{
			this.rootNode = rootNode;
		}

		public void Execute() {
			rootNode.Execute();
		}
	}
}

