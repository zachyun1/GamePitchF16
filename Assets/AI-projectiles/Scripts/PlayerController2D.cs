using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class PlayerController2D : MonoBehaviour {

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
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        gameObject.transform.position = pos;
    }
}
