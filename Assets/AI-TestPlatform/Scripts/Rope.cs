using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour {

    public Vector2 destination;
    public float speed;
    public float overlapFactor = 1;
    public GameObject self;
    public GameObject prefab;
    public GameObject lastNode;
    public GameObject secondLastNode;
    public GameObject hookHead;
    public GameObject target;
    GameObject[] nodes;
    GameObject player;
    GameObject misc;
    bool forward = true;
    bool con = false;
    bool noHit = true;
    int count = 0;

	// Use this for initialization
	void Start () {
        self = GameObject.FindGameObjectWithTag("Boss");
        lastNode = transform.gameObject;
        float dist = (int)Vector2.Distance(self.transform.position, target.transform.position);
        int size = (int)(dist / overlapFactor) + 2;
        Debug.Log(size);
        nodes = new GameObject[size];

	}
	
	// Update is called once per frame
	void Update () {
        if (forward && noHit)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed);

            if ((Vector2)transform.position != destination)
            {
                if (Vector2.Distance(self.transform.position, lastNode.transform.position) > overlapFactor)
                {
                    makeNode();
                }
            }
            else if (con == false)
            {
                con = true;
                lastNode.GetComponent<HingeJoint2D>().connectedBody = self.GetComponent<Rigidbody2D>();
            }
            else
            {
                forward = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, self.transform.position, speed / 2);
            if (count > 0)
            {
                if (Vector2.Distance(self.transform.position, nodes[count - 1].transform.position) < overlapFactor)
                {
                    killNode();
                }
            }
            else
            {
                Destroy(misc);
                Destroy(hookHead);
            }
           
        }
        if(misc)
        {
            pull();
        }

	}

    void makeNode()
    {
        Vector2 createPos = self.transform.position - lastNode.transform.position;
        createPos.Normalize();
        createPos *= overlapFactor;
        createPos += (Vector2)lastNode.transform.position;

        Vector2 destination = target.transform.position;
        Quaternion rot = Quaternion.FromToRotation(Vector2.left, destination - (Vector2)self.transform.position);
        GameObject node = (GameObject)Instantiate(prefab, createPos, rot);
        node.transform.Rotate(0, 0, 90);
        nodes[count] = node;
        count++;
        Debug.Log(count);

        node.transform.SetParent(transform);

        lastNode.GetComponent<HingeJoint2D>().connectedBody = node.GetComponent<Rigidbody2D>();
        lastNode = node;
    }

    void killNode()
    {
        Destroy(nodes[count]);
        count--;
        Debug.Log(count);
        nodes[count].GetComponent<HingeJoint2D>().connectedBody = self.GetComponent<Rigidbody2D>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            noHit = false;
        }
        else if(collision.gameObject.tag == "Misc")
        {
            misc = collision.gameObject;
            noHit = false;
        }
    }

    void pull()
    {
        misc.transform.position = Vector2.MoveTowards(misc.transform.position,
                                            self.transform.position, speed / 2);
    }

}
