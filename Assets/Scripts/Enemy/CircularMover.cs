using UnityEngine;
using System.Collections;

public class CircularMover : Mover {

    public int hitPoints;
    public int scoreValue;
    public EnemyShip ES;
    public GameObject projectile;
    public float delayManuvuer = 2.0f;
    float stopManuvuer = 1.19f;

	public float turnSpeed;
	Rigidbody enemyShip;
	Quaternion qAngle;
    bool isManuevering = false;
    float newSpeed;

    protected override void Awake()
	{
        ES = new EnemyShip(hitPoints, scoreValue);
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
        ES.Shoot(projectile, transform.position, transform.rotation);
        isManuevering = true;
        yield return new WaitForSeconds(stopManuvuer);
        isManuevering = false;
	}

    void FixedUpdate()
    {
        if (isManuevering)
        {
            qAngle = Quaternion.Euler(enemyShip.rotation.eulerAngles + new Vector3(0.0f, 0.0f, -turnSpeed));
            enemyShip.velocity = transform.right * newSpeed;
            enemyShip.rotation = qAngle; 
        }
        else
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            enemyShip.velocity = transform.right * speed;
        }
    }
}
