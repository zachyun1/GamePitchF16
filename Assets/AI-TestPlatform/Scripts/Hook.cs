using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {

    public GameObject hook;
    public GameObject target;
    public GameObject self;

    GameObject hookGo;

	// Use this for initialization
	void Start () {
        self = GameObject.FindGameObjectWithTag("Boss");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            //Vector2 destination = target.transform.position;
            Vector2 destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Quaternion rot = Quaternion.FromToRotation(Vector2.left, destination - (Vector2)self.transform.position);
            hookGo = (GameObject)Instantiate(hook, transform.position, rot);
            hookGo.transform.Rotate(0, 0, -90);

            hookGo.GetComponent<Rope>().destination = destination;
            hookGo.GetComponent<Rope>().target = target;
            hookGo.GetComponent<Rope>().hookHead = hookGo;
        }
	}

}
