using UnityEngine;
using System.Collections;

public class TrackingMover : Mover {
    
    public int hitPoints;
    public int scoreValue;
    public EnemyShip ES;
    float seconds;

    protected override void Awake ()
    {
        base.Awake();
        ES = new EnemyShip(hitPoints, scoreValue);
        seconds = 1.0f;
        if(player != null)
        {
            Vector3 targetPos = player.transform.position;
            targetPos.z = 0.0f;
            transform.right = targetPos - transform.position;
        }
    }
	
	// Update is called once per frame
	void Update () {
        seconds -= Time.deltaTime;
        if (seconds < 0.0f)
        {
            player = GameObject.FindGameObjectWithTag("PlayerShip");
            if (player != null)
            {
                heading = player.transform.position - transform.position;
                distance = heading.magnitude;
                direction = heading / distance;
            }
            else
                direction = -transform.right;
            //new Vector3(1.0f * Mathf.Cos(angle), 1.0f * Mathf.Sin(angle), 0.0f)
            if (followPlayer)
            {
                GetComponent<Rigidbody>().velocity = direction * speed;
                if (player != null)
                {
                    Vector3 targetPos = player.transform.position;
                    targetPos.z = 0.0f;
                    transform.right = targetPos - transform.position;
                }
            }
            else
            {
                GetComponent<Rigidbody>().velocity = transform.right * speed;
            }
            seconds = 1.0f;
            speed += 0.1f;
        }
	}
}
