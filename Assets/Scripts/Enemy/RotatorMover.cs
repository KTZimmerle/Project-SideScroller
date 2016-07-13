using UnityEngine;
using System.Collections;

public class RotatorMover : Mover {

    public int hitPoints;
    public int scoreValue;
    public EnemyShip ES;
    public GameObject projectile;
    //GameObject target;
    public Collider[] search;
    float seconds;
    float secondsToShoot;
    float secondsToScan;
    float rotSpeed;
    int minSpeed;
    int maxSpeed;

    protected override void Awake()
    {
        ES = new EnemyShip(hitPoints, scoreValue);
        minSpeed = 1;
        maxSpeed = 4;
        seconds = 3.0f;
        secondsToShoot = 2.5f;
        secondsToScan = 1.0f;
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
            secondsToScan -= Time.deltaTime;
        }
        //find a player ship to target and lock on to
        if (search.Length > 0 && search[0] != null)
        {
            Vector3 targetPos = search[0].transform.position;
            targetPos.z = 0.0f;
            transform.right = targetPos - transform.position;

            if (secondsToShoot < 0.0f)
            {
                ES.Shoot(projectile, transform.position, Quaternion.Euler(0.0f, 0.0f, 180.0f) * transform.rotation);
                secondsToShoot = 2.5f;
            }
            else
                secondsToShoot -= Time.deltaTime;
        }
        else //keep searching for a new player ship
        {
            //target = GameObject.FindGameObjectWithTag("PlayerShip");
            if(secondsToScan < 0.0f)
            {
                secondsToScan = 1.0f;
                search = Physics.OverlapSphere(transform.position, 25.0f, 1 << 11, QueryTriggerInteraction.Collide);
            }
        }
    }

    void isPlayerActive()
    {
        //GameObject gameController = GetComponent<GameController>();
    }
}
