using UnityEngine;
using System.Collections;

public class TrackingMover : MonoBehaviour {
    
    public int hitPoints;
    public int scoreValue;
    public EnemyShip ES;
    float seconds;
    public Collider[] search;
    public GameObject explosion;
    Vector3 direction;
    public float speed;

    void Awake ()
    {
        //base.Awake();
        ES = new EnemyShip(hitPoints, scoreValue, explosion);
        seconds = 1.0f;
        search = Physics.OverlapSphere(transform.position, 25.0f, 1 << 11, QueryTriggerInteraction.Collide);
        findTarget();

        GetComponent<Rigidbody>().velocity = direction * speed;
    }
	
	void Update () {
        seconds -= Time.deltaTime;

        //check if a second has passed by before recalculating
        if (seconds < 0.0f)
        {
            findTarget();

            if (direction != transform.right)
            {
                GetComponent<Rigidbody>().velocity = direction * speed;
            }
            else
            {
                GetComponent<Rigidbody>().velocity = transform.right * speed;
            }
            seconds = 1.0f;
            speed += 0.1f;
        }
	}

    void findTarget()
    {
        search = Physics.OverlapSphere(transform.position, 25.0f, 1 << 11, QueryTriggerInteraction.Collide);
        if (search.Length > 0 && search[0] != null)
        {
            Vector3 heading = search[0].transform.position - transform.position;
            float distance = heading.magnitude;
            direction = heading / distance;
            Vector3 targetPos = search[0].transform.position;
            targetPos.z = 0.0f;
            transform.right = targetPos - transform.position;
        }
        else
            direction = transform.right;
    }

}
