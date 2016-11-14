using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpray2D : MonoBehaviour {

    public int numberOfProjectiles;
    public GameObject prefab;
    public GameObject fireFrom;
    public GameObject target;
    public float projectileVelocity;
    public float fireDelay;
    public float fireRate;
    public float angle;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Decrement timer until its time to fire
        if (fireDelay > 1.0)
        {
            fireDelay -= Time.deltaTime;
        } 
        else
        {

            Vector2 destination = target.transform.position;
            Vector2 center = fireFrom.transform.position;
            Quaternion rot = Quaternion.FromToRotation(Vector2.left, destination - center);

            int test1 = numberOfProjectiles / 2;
            int test2 = (numberOfProjectiles - 1) / 2;
            if(numberOfProjectiles%2 == 0)
            {

            }
            for (int i = -test1; i < test1 + 1; i++)
            {
              //  rot *= Quaternion.Euler(0, 0, 20 * i);
                GameObject projectile = Instantiate(prefab, center, rot) as GameObject;
                projectile.transform.Rotate(0, 0, angle * i);
                //projectile.transform.position = fireFrom.transform.position;


                //Get the rigid body and apply a force towards the target with given velocity
                Rigidbody2D rigidbody = projectile.GetComponent<Rigidbody2D>();
                Vector2 spreadVec = projectile.transform.rotation * Vector2.left;
                rigidbody.velocity = spreadVec * projectileVelocity;
            }


            //if(numberOfProjectiles%2 == 0)
            //{
            //    int spread = numberOfProjectiles / 2;
            //    float space = angle / numberOfProjectiles;
            //    for(int i = 0; i < spread; i++)
            //    {
            //        GameObject projectile = Instantiate(prefab, center, rot) as GameObject;
            //    }
            //}

            //Reset the fire timer
            fireDelay += fireRate;

        }
    }

    Vector2 getProjectilePosition(Vector2 center, int count, float r)
    {
        Vector2 position;
        float ang = 45 / numberOfProjectiles * count;
        position.x = center.x + r * Mathf.Sin(ang * Mathf.Deg2Rad);
        position.y = center.y + r * Mathf.Cos(ang * Mathf.Deg2Rad);
        return position;
    }
}
