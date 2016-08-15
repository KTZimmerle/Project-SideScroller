using UnityEngine;
using System.Collections;

public class CircularMover : MonoBehaviour {

    public int hitPoints;
    public int scoreValue;
    public EnemyShip ES;
    public GameObject projectile;
    public float delayManuvuer = 2.0f;
    public GameObject explosion;
    float stopManuvuer = 1.19f;

	public float turnSpeed;
	Rigidbody enemyShip;
	Quaternion qAngle;
    bool isManuevering = false;
    bool isDone = false;
    float newSpeed;
    public float speed;

    void Awake()
	{
        ES = new EnemyShip(hitPoints, scoreValue, explosion);
        enemyShip = GetComponent<Rigidbody>();
        enemyShip.velocity = transform.right * speed;
        newSpeed = -5 * 100;
        if (enemyShip.transform.position.y >= 2.5f)
        {
            turnSpeed = turnSpeed - 1; 
            stopManuvuer = 1.78f;
        }
        else if (enemyShip.transform.position.y <= 0.0f &&
                 enemyShip.transform.position.y > -2.5f)
        {
            turnSpeed = -turnSpeed * 2;
        }
        else if (enemyShip.transform.position.y <= -2.5f)
        {
            turnSpeed = -turnSpeed + 1;
            stopManuvuer = 1.78f;
        }
        else
        {
            turnSpeed *= 2;
        }
        
		StartCoroutine(circlarMovement());
	}

    IEnumerator circlarMovement()
    {
		yield return new WaitForSeconds(delayManuvuer);
        GetComponentInChildren<AutoRotate>().enabled = true;
        GetComponentInChildren<ParticleSystem>().Play();
        Vector3 offset = transform.position + new Vector3(-0.5f, 0.0f, 0.0f);
        ES.Shoot(projectile, offset, transform.rotation);
        isManuevering = true;
        yield return new WaitForSeconds(stopManuvuer);
        isManuevering = false;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        enemyShip.velocity = transform.right * speed;
    }

    void FixedUpdate()
    {
        if (isManuevering)
        {
            qAngle = Quaternion.Euler(enemyShip.rotation.eulerAngles + new Vector3(0.0f, 0.0f, -turnSpeed));
            enemyShip.velocity = transform.right * newSpeed * Time.deltaTime;
            enemyShip.rotation = qAngle;;
        }
    }
}
