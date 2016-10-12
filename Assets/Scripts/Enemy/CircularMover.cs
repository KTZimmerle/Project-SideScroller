using UnityEngine;
using System.Collections;

public class CircularMover : MonoBehaviour {

    public int hitPoints;
    public int scoreValue;
    public EnemyShip ES;
    public GameObject projectile;
    GameObject proj;
    public float delayManuvuer = 2.0f;
    public GameObject explosion;
    //GameObject exp;
    float stopManuvuer = 1.19f;

    public float turnSpeed;
    float newTurnSpeed;
	Rigidbody enemyShip;
	Quaternion qAngle;
    bool isManuevering = false;
    float newSpeed;
    public float speed;

    void Awake()
    {
        //exp = Instantiate(explosion);
        proj = Instantiate(projectile);
        ES = new EnemyShip(hitPoints, scoreValue);
        enemyShip = GetComponent<Rigidbody>();
        newSpeed = -5 * 100;
        
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        newTurnSpeed = 0.0f;
        enemyShip.velocity = transform.right * speed;
        isManuevering = false;
        GetComponentInChildren<AutoRotate>().enabled = false;
        transform.GetChild(1).rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
        if (enemyShip.transform.position.y >= 2.5f)
        {
            newTurnSpeed = turnSpeed - 1; 
            stopManuvuer = 1.78f;
        }
        else if (enemyShip.transform.position.y <= 0.0f &&
                 enemyShip.transform.position.y > -2.5f)
        {
            newTurnSpeed = -turnSpeed * 2;
            stopManuvuer = 1.19f;
        }
        else if (enemyShip.transform.position.y <= -2.5f)
        {
            newTurnSpeed = -turnSpeed + 1;
            stopManuvuer = 1.78f;
        }
        else
        {
            newTurnSpeed = turnSpeed * 2;
            stopManuvuer = 1.19f;
        }
		StartCoroutine(circlarMovement());

    }

    void OnDisable()
    {
        ES.Init(hitPoints);
        GetComponentInChildren<AutoRotate>().enabled = false;
    }

    IEnumerator circlarMovement()
    {
		yield return new WaitForSeconds(delayManuvuer);
        GetComponentInChildren<AutoRotate>().enabled = true;
        GetComponentInChildren<ParticleSystem>().Play();
        Vector3 offset = transform.position + new Vector3(-0.5f, 0.0f, 0.0f);
        //ES.Shoot(projectile, offset, transform.rotation);
        ES.Shoot(proj, offset, transform.rotation);
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
            qAngle = Quaternion.Euler(enemyShip.rotation.eulerAngles + new Vector3(0.0f, 0.0f, -newTurnSpeed));
            enemyShip.velocity = transform.right * newSpeed * Time.deltaTime;
            enemyShip.rotation = qAngle;;
        }
    }
}
