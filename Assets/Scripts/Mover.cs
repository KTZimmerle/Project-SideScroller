using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	
	public float speed;
    public bool followPlayer = false;
    protected GameObject player;
    Vector3 heading;
    float distance;
    Vector3 direction;

    protected virtual void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("PlayerShip");
        if (player != null)
        {
            heading = player.transform.position - transform.position;
            distance = heading.magnitude;
            direction = heading / distance;
        }

        if(followPlayer)
            GetComponent<Rigidbody>().velocity = direction * speed;
        else
            GetComponent<Rigidbody>().velocity = transform.right * speed;
    }
}
