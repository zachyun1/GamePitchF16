﻿using System.Collections;
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

    public float health = 100;

    public float meleeDelay = 0.1f;
    public float meleeTime = 0.1f;

    public float rangeDelay = 0.1f;
    public float rangeTime = 0.1f;
    public float rangeParticleHeight = 1f;
    public GameObject rangeProjectile;

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

    public void TakeDamage(float damage)
    {
        health -= damage;
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
        setLookAtX(target.transform.position);
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
        setLookAtX(target.transform.position);
        if (IsTargetInMeleeAttackRange())
        {
            seek.TargetPoint = transform.position;
            animator.SetBool("move", false);
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State PrepareMeleeAttack(BehaviourTreeNode<float> node) {
        if (target == null)
        {
            animator.SetBool("melee", false);
            animator.SetBool("prepare", false);
            return BehaviourTree.State.FAILURE;
        }
        animator.SetBool("melee", true);
        animator.SetBool("prepare", true);
        setLookAtX(target.transform.position);
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
        if (target == null)
        {
            animator.SetBool("range", false);
            animator.SetBool("prepare", false);
            return BehaviourTree.State.FAILURE;
        }
        animator.SetBool("range", true);
        animator.SetBool("prepare", true);
        setLookAtX(target.transform.position);
        node.Result += Time.deltaTime;
        if (node.Result > rangeDelay)
        {
            node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State RangeAttack(BehaviourTreeNode<Tuple<float, GameObject>> node)
    {
        if (node.Result == null)
        {
            node.Result = new Tuple<float, GameObject>();
        }
        animator.SetBool("range", true);
        animator.SetBool("prepare", false);
        node.Result.Val1 += Time.deltaTime;
        if (node.Result.Val2 == null) {
            Transform parent = transform.FindChild("ProjectilePivot");
            node.Result.Val2 = Instantiate(rangeProjectile, parent);
            node.Result.Val2.transform.position = parent.position;
        }
        if (node.Result.Val1 > rangeTime)
        {
            Destroy(node.Result.Val2);
            node.Result.Val1 = 0;
            node.Result.Val2 = null;
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

    private float setLookAtX(Vector3 target)
    {
        float lookX = target.x - transform.position.x;
        animator.SetFloat("lookX", lookX > 0 ? 1 : -1);
        return lookX;
    }

    private BehaviourTree.Node GetMeleeTree()
    {
        return new BinaryTreeNode(
            IsTargetInMeleeRange,
            new SequenceTreeNode(new BehaviourTree.Node[] {
                new ActionTreeNode<System.Object>(Wake),
                new ActionTreeNode<System.Object>(MoveTowardsTarget),
                new BinaryTreeNode(
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
                new ActionTreeNode<System.Object>(Idle),
                new ActionTreeNode<System.Object>(node => BehaviourTree.State.FAILURE)
            })
        );
    }

    private BehaviourTree.Node GetRangeTree()
    {
        return new BinaryTreeNode(
            IsTargetInHorizontalProjectileRange,
            new SequenceTreeNode(new BehaviourTree.Node[] {
                new ActionTreeNode<System.Object>(Wake),
                new ActionTreeNode<float>(PrepareRangeAttack),
                new ActionTreeNode<Tuple<float, GameObject>>(RangeAttack),
                new ActionTreeNode<System.Object>(WithdrawAttack)
            }),
            new SequenceTreeNode(new BehaviourTree.Node[] {
                new ActionTreeNode<System.Object>(WithdrawAttack)
            })
        );
    }

	#region Actor implementation

	public BehaviourTree.Node GetBehaviourTree() {
        return new RepeatTreeNode(new BinaryTreeNode(
            IsTargetInRange,
            new SequenceTreeNode(new BehaviourTree.Node[] {
                new ActionTreeNode<System.Object>(LookAtTarget),
                new SelectorTreeNode(new BehaviourTree.Node[] {
                    GetMeleeTree(),
                    GetRangeTree()
                })
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
