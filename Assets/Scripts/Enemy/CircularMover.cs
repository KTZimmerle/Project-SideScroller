using UnityEngine;
using System.Collections;

public class CircularMover : Mover {

    public GameObject projectile;
    public float delayManuvuer = 2.0f;
    float stopManuvuer = 1.19f;

	public float turnSpeed;
	Rigidbody enemyShip;
	Quaternion angle;
    bool isManuevering = false;
    float newSpeed;

	void Start()
	{
        enemyShip = GetComponent<Rigidbody>();
        if (enemyShip.transform.position.y >= 2.5f)
        {
            newSpeed = -5 * 2;
            turnSpeed = turnSpeed - 1; 
            stopManuvuer = 1.78f;
        }
        else if (enemyShip.transform.position.y <= 0.0f &&
                 enemyShip.transform.position.y > -2.5f)
        {
            newSpeed = -5 * 2;
            turnSpeed = -turnSpeed * 2;
        }
        else if (enemyShip.transform.position.y <= -2.5f)
        {
            newSpeed = -5 * 2;
            turnSpeed = -turnSpeed + 1;
            stopManuvuer = 1.78f;
        }
        else
        {
            newSpeed = -5 * 2;
            turnSpeed *= 2;
        }
        
		StartCoroutine(circlarMovement());
	}

    IEnumerator circlarMovement()
    {
		yield return new WaitForSeconds(delayManuvuer);
        Shoot();
        isManuevering = true;
        yield return new WaitForSeconds(stopManuvuer);
        isManuevering = false;
	}

    void FixedUpdate()
    {
        if (isManuevering)
        {
            angle = Quaternion.Euler(enemyShip.rotation.eulerAngles + new Vector3(0.0f, 0.0f, -turnSpeed));
            enemyShip.velocity = transform.right * newSpeed;
            enemyShip.rotation = angle; 
        }
        else
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            enemyShip.velocity = transform.right * speed;
        }
    }

    void Shoot()
    {
        GameObject clone;
        clone = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
    }

}
