using UnityEngine;
using System.Collections;

public class RotatorMover : Mover {

    public GameObject projectile;
    public GameObject target;
    float seconds;
    float secondsToShoot;
    float rotSpeed;
    int minSpeed;
    int maxSpeed;

    void Start()
    {
        minSpeed = 1;
        maxSpeed = 4;
        seconds = 3.0f;
        secondsToShoot = 2.5f;
        rotSpeed = 100.0f;
        speed = Random.Range(minSpeed, maxSpeed);
        if (GetComponent<Rigidbody>().transform.position.y < 0.0f)
            GetComponent<Rigidbody>().velocity = transform.up * speed;
        else
            GetComponent<Rigidbody>().velocity = -transform.up * speed;
    }

	// Update is called once per frame
	void Update () 
    {
        if (seconds > 0.0f)
        {
            transform.Rotate(0.0f, 0.0f, Time.deltaTime * 1000, Space.Self);
            seconds -= Time.deltaTime;
        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;

            //find a player ship to target and lock on to
            if (target != null)
            {
                Vector3 targetPos = target.transform.position;
                targetPos.z = 0.0f;
                transform.right = targetPos - transform.position;

                if (secondsToShoot < 0.0f)
                {
                    Shoot();
                    secondsToShoot = 2.5f;
                }
                else
                    secondsToShoot -= Time.deltaTime;
            }
            else //keep searching for a new player ship
            {
                target = GameObject.FindGameObjectWithTag("PlayerShip");
            }
        }
	}

    void Shoot()
    {
        GameObject clone;
        clone = Instantiate(projectile, transform.position, Quaternion.Euler(0.0f, 0.0f, 180.0f) * transform.rotation) as GameObject;
        //clone.GetComponent<Rigidbody>().AddForce(transform.right);
    }
}
