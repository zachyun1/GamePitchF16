using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFire2D : MonoBehaviour
{

    public int numberOfProjectiles;
    public GameObject prefab;
    public GameObject target;
    public GameObject fireFrom;
    public float projectileVelocity;
    public float fireDelay;
    public float fireRate;

    // Use this for initialization
    void Start()
    {
        //Load in the projectile to fire
        //prefab = Resources.Load("RollerBall") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Decrement timer until its time to fire
        if (fireDelay > 1.0)
        {
            fireDelay -= Time.deltaTime;
        }
        //Fire
        else
        {
            //Instantiate the projectile at the location of the given 'fireFrom' object
            Vector2 destination = target.transform.position;
            Vector2 center = fireFrom.transform.position;
            Quaternion rot = Quaternion.FromToRotation(Vector2.left, destination - center);

            GameObject projectile = Instantiate(prefab, center, rot) as GameObject;
            //projectile.transform.position = fireFrom.transform.position;


            //Get the rigid body and apply a force towards the target with given velocity
            Rigidbody2D rigidbody = projectile.GetComponent<Rigidbody2D>();
            rigidbody.velocity = (destination - center).normalized * projectileVelocity;


            //Reset the fire timer
            fireDelay += fireRate;
        }
    }
}
