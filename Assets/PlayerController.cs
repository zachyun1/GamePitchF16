using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float m_MaxSpeed = 10f;
    [SerializeField]
    private LayerMask m_WhatIsGround;

    public float speed = 5.0f;

    private Animator m_Anim;
    private Rigidbody2D m_Rigidbody2D;

    private float lookx;
    private bool m_FacingRight = true;
    private bool m_Grounded;


    // Use this for initialization
    void Start () {
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        if (h != 0)
        {
            Move(h);
        }
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        if (v != 0)
        {
            MoveVert(v);
        }
        m_Anim.SetFloat("lookY", m_Rigidbody2D.velocity.y);
    }

    public void Move(float move)
    {
        // The Speed animator parameter is set to the absolute value of the horizontal input.
        m_Anim.SetFloat("lookX", Mathf.Abs(move));

        // Move the character
        m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
            lookx = 1;
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
            lookx = -1;
        }
    }

    public void MoveVert(float move)
    {
        // The Speed animator parameter is set to the absolute value of the horizontal input.
        m_Anim.SetFloat("lookY", Mathf.Abs(move));

        // Move the character
        m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, move * m_MaxSpeed);

    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
