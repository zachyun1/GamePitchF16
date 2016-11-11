using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTree;

[RequireComponent(typeof (Animator))]
public class EnemyAIActor : MonoBehaviour, Actor {

	public float mediumRadius;
	public float smallRadius;

	private Animator animator;
	private GameObject target;

	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator>();
	}

	void OnTriggerStay2D(Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			target = collider.gameObject;
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			target = null;
		}
	}

	public bool IsTargetInRange() {
		return target != null;
	}

	public bool IsTargetInMeleeRange() {
		if ((target.transform.position - transform.position).magnitude <= mediumRadius) {
			Debug.Log("Death is near for you!");
			return true;
		}
		return false;
	}

	public bool IsTargetInMeleeAttackRange() {
		if ((target.transform.position - transform.position).magnitude <= smallRadius) {
			return true;
		}
		return false;
	}

	public bool Idle() {
		Debug.Log("I am bored.");
		animator.SetBool("idle", true);
		return true;
	}

	public bool Wake() {
		animator.SetBool("idle", false);
		return true;
	}

	public bool LookAtTarget() {
		animator.SetFloat("lookX", target.transform.position.x - transform.position.x);
		return true;
	}

	public bool PrepareMeleeAttack() {
		Debug.Log("Prepare to melee attack!");
		animator.SetBool("melee", true);
		animator.SetBool("prepare", true);
		return true;
	}

	public bool MeleeAttack() {
		Debug.Log("Melee Attack!");
		animator.SetBool("prepare", false);
		return true;
	}

	public bool WithdrawAttack() {
		animator.SetBool("prepare", false);
		animator.SetBool("melee", false);
		return true;
	}

	#region Actor implementation

	public BehaviourTreeNode GetBehaviourTree() {
		ActionTreeNode defaultNode = new ActionTreeNode(Idle);
		return new SelectorTreeNode(
			IsTargetInRange, 
			new BehaviourTreeNode[] {
				new ActionTreeNode(LookAtTarget), 
				new SelectorTreeNode(
					IsTargetInMeleeRange,
					new BehaviourTreeNode[] {
						new ActionTreeNode(Wake),
						new ActionTreeNode(PrepareMeleeAttack),
						new SelectorTreeNode(
							IsTargetInMeleeAttackRange,
							new BehaviourTreeNode[] {
								new ActionTreeNode(MeleeAttack)
							},
							new BehaviourTreeNode[] { new ActionTreeNode(PrepareMeleeAttack) }
						)
					},
					new BehaviourTreeNode[] { 
						new ActionTreeNode(WithdrawAttack),
						defaultNode
					}
				)
			}, 
			new BehaviourTreeNode[] { defaultNode }
		);
	}

	#endregion
}
