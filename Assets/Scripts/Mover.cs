using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	
	public float speed;
    public bool followPlayer = false;
    protected GameObject player;
    protected Vector3 heading;
    protected float distance;
    protected Vector3 direction;
    public GameObject drop;
    //public float angle = 0.0f;

    protected virtual void Awake ()
    {
        //angle *= Mathf.Rad2Deg;
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
            GetComponent<Rigidbody>().velocity = direction * speed;
        else
            GetComponent<Rigidbody>().velocity = transform.right * speed;
    }
}
