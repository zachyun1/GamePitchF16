using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTree;
using Steer2D;

[RequireComponent(typeof(BTreeTickBehaviour))]
[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (CircleCollider2D))]
[RequireComponent(typeof (Rigidbody2D))]
[RequireComponent(typeof (Seek))]
public class EnemyAIActor : MonoBehaviour, Actor {

    public float meleeDelay = 0.1f;
    public float meleeTime = 0.1f;

    public float rangeDelay = 0.1f;
    public float rangeTime = 0.1f;
    public float rangeParticleHeight = 1f;

	public float mediumRadius;
	public float smallRadius;

	private Animator animator;
	private GameObject target;
    private Seek seek;

	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator>();
        seek = gameObject.GetComponent<Seek>();
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
		if (target != null && (target.transform.position - transform.position).magnitude <= mediumRadius) {
			Debug.Log("Death is near for you!");
			return true;
		}
		return false;
	}

	public bool IsTargetInMeleeAttackRange() {
		if (target != null && (target.transform.position - transform.position).magnitude <= smallRadius) {
			return true;
		}
		return false;
	}

    public bool IsTargetInHorizontalProjectileRange()
    {
        if (target == null)
        {
            return false;
        }
        float y = target.transform.position.y - transform.position.y;
        float offset = rangeParticleHeight / 2;
        if ((-offset < y) && (y < offset))
        {
            return true;
        }
        return false;
    }

	public BehaviourTree.State Idle(BehaviourTreeNode<System.Object> node) {
		animator.SetBool("idle", true);
		return BehaviourTree.State.SUCCESS;
	}

	public BehaviourTree.State Wake(BehaviourTreeNode<System.Object> node) {
		animator.SetBool("idle", false);
		return BehaviourTree.State.SUCCESS;
	}

	public BehaviourTree.State LookAtTarget(BehaviourTreeNode<System.Object> node) {
        if (target == null)
        {
            return BehaviourTree.State.SUCCESS;
        }
        setAnimatorLookAtX(target.transform.position);
        return BehaviourTree.State.SUCCESS;
	}
    
    public BehaviourTree.State MoveTowardsTarget(BehaviourTreeNode<System.Object> node)
    {
        if (target == null)
        {
            seek.TargetPoint = transform.position;
            animator.SetBool("move", false);
            return BehaviourTree.State.FAILURE;
        }
        seek.TargetPoint = target.transform.position;
        animator.SetBool("move", true);
        setAnimatorLookAtX(target.transform.position);
        if (IsTargetInMeleeAttackRange())
        {
            seek.TargetPoint = transform.position;
            animator.SetBool("move", false);
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State PrepareMeleeAttack(BehaviourTreeNode<float> node) {
        animator.SetBool("melee", true);
        animator.SetBool("prepare", true);
        node.Result += Time.deltaTime;
        if (node.Result > meleeDelay)
        {
            node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
	}

	public BehaviourTree.State MeleeAttack(BehaviourTreeNode<float> node) {
        animator.SetBool("melee", true);
        animator.SetBool("prepare", false);
        node.Result += Time.deltaTime;
        if (node.Result > meleeTime)
        {
            Debug.Log("Muahahaha!");
            node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
	}

    public BehaviourTree.State PrepareRangeAttack(BehaviourTreeNode<float> node)
    {
        animator.SetBool("range", true);
        animator.SetBool("prepare", true);
        node.Result += Time.deltaTime;
        if (node.Result > rangeTime)
        {
            node.Result = 0;
            animator.SetBool("range", false);
            animator.SetBool("prepare", false);
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

	public BehaviourTree.State WithdrawAttack(BehaviourTreeNode<System.Object> node) {
		animator.SetBool("prepare", false);
		animator.SetBool("melee", false);
        animator.SetBool("range", false);
		return BehaviourTree.State.SUCCESS;
	}

    private void setAnimatorLookAtX(Vector3 target)
    {
        float lookX = target.x - transform.position.x;
        animator.SetFloat("lookX", lookX > 0 ? 1 : -1);
    }

	#region Actor implementation

	public BehaviourTree.Node GetBehaviourTree() {
        return new RepeatTreeNode(new SelectorTreeNode(
            IsTargetInRange,
            new SequenceTreeNode(new BehaviourTree.Node[] {
                new ActionTreeNode<System.Object>(LookAtTarget),
                new SelectorTreeNode(
                    IsTargetInHorizontalProjectileRange,
                    new SequenceTreeNode(new BehaviourTree.Node[] {
                        new ActionTreeNode<System.Object>(Wake),
                        new ActionTreeNode<float>(PrepareRangeAttack)
                    }),
                    new ActionTreeNode<System.Object>(node => BehaviourTree.State.SUCCESS)
                ),
                new SelectorTreeNode(
                    IsTargetInMeleeRange,
                    new SequenceTreeNode(new BehaviourTree.Node[] {
                        new ActionTreeNode<System.Object>(Wake),
                        new ActionTreeNode<System.Object>(MoveTowardsTarget),
                        new SelectorTreeNode(
                            IsTargetInMeleeAttackRange,
                            new SequenceTreeNode(new BehaviourTree.Node[] {
                                new ActionTreeNode<float>(PrepareMeleeAttack),
                                new ActionTreeNode<float>(MeleeAttack)
                            }),
                            new ActionTreeNode<System.Object>(WithdrawAttack)
                        )
                    }),
                    new SequenceTreeNode(new BehaviourTree.Node[] {
                        new ActionTreeNode<System.Object>(WithdrawAttack),
                        new ActionTreeNode<System.Object>(Idle)
                    })
                )
            }),
            new SequenceTreeNode(new BehaviourTree.Node[]
            {
                new ActionTreeNode<System.Object>(WithdrawAttack),
                new ActionTreeNode<System.Object>(Idle)
            })
        ));
	}

	#endregion
}
