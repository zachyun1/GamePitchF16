using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiply : MonoBehaviour {

    public GameObject prefab;
    private float multiplyDelay = 5.0f;
    public float multiplyRate;
    public float velocity;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (multiplyDelay > 1.0)
        {
            multiplyDelay -= Time.deltaTime;
        }
        else
        {
            Vector2 center = transform.position;
            Vector2 pos = new Vector2(Random.Range(-1.0f,1.0f), Random.Range(-1.0f,1.0f));
            GameObject projectile = Instantiate(prefab) as GameObject;

            //Get the rigid body 2D and apply a force towards the target with given velocity
            Rigidbody2D rigidbody = projectile.GetComponent<Rigidbody2D>();
            rigidbody.velocity = (pos - center).normalized * velocity;

            //Reset the fire timer
            multiplyDelay += multiplyRate;
        }
    }
}
