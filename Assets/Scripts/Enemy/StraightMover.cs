using UnityEngine;
using System.Collections;

public class StraightMover : Mover {

    public float speedMin;
    public float speedMax;
    public int hitPoints;
    public int scoreValue;
    public EnemyShip ES;
    public GameObject projectile;
    //GameObject player;
    float seconds;
    bool hasShot;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(GetComponent<Rigidbody>().velocity.x * speedMin,
                                                         GetComponent<Rigidbody>().velocity.x * speedMax), 0.0f, 0.0f);
        player = GameObject.FindGameObjectWithTag("PlayerShip");
        seconds = 2.0f;
        hasShot = false;
        ES = new EnemyShip(hitPoints, scoreValue);
    }

    void Update()
    {
        seconds -= Time.deltaTime;
        if ((seconds) <= 0.0f && !hasShot && player != null)
        {
            hasShot = true;
            ES.Shoot(projectile, transform.position, transform.rotation);
        }
    }
}
