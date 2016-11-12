using System;
using UnityEngine;
using BTree;
using System.Collections.Generic;

namespace BTree
{
	public class BehaviourTree
	{
        public enum State
        {
            WAITING,
            SUCCESS,
            FAILURE,
            RUNNING
        }

        public abstract class Node
        {
            protected State state;

            public State GetState()
            {
                return state;
            }

            public bool IsComplete()
            {
                return state != State.RUNNING && state != State.WAITING;
            }

            public State Tick(BehaviourTree tree)
            {
                this.state = State.WAITING;
                tree.Nodes.Push(this);
                State state = Execute(tree);
                if (IsComplete())
                {
                    tree.Nodes.Pop();
                }
                return state;
            }
            
            public abstract State Execute(BehaviourTree tree);

            public abstract Node[] GetChildren();
        }

        public Node rootNode;

        public Stack<Node> Nodes = new Stack<Node>();

		public BehaviourTree (Node rootNode, GameObject actor)
		{
			this.rootNode = rootNode;
		}

		public void Tick()
        {
			Node current = GetCurrentNode(rootNode);
            current.Tick(this);
		}

        private Node GetCurrentNode(Node node)
        {
            if (Nodes.Count == 0) {
                return rootNode;
            }
            node = Nodes.Pop();
            if (node.GetChildren().Length > 0)
            {
                foreach (Node child in node.GetChildren())
                {
                    if (!child.IsComplete())
                    {
                        return child;
                    }
                }
            } else if (!node.IsComplete())
            {
                return node;
            }
            return GetCurrentNode(node);
        }
	}
}

