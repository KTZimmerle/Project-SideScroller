using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	
	public float speed;
    public bool followPlayer = false;
    public GameObject player;
    protected Vector3 heading;
    protected float distance;
    protected Vector3 direction;
    public float angle;

    protected virtual void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("PlayerShip");
        if (player != null)
        {
            if (angle >= 0.01f || angle <= -0.01f)
            {
                heading = (transform.position - player.transform.position);
                distance = Mathf.Sqrt(heading.x * heading.x + heading.y * heading.y);
                direction = (new Vector3(-heading.y, heading.x, 0.0f) / distance) * angle + player.transform.position.normalized;
            }
            else
            {
                heading = (player.transform.position - transform.position);
                distance = heading.magnitude;
                direction = (heading / distance);
            }
        }
        else
            direction = -transform.right;

        if (followPlayer)
        {
            GetComponent<Rigidbody>().velocity = direction.normalized * speed;
        }
        else
            GetComponent<Rigidbody>().velocity = transform.right * speed;
    }
}
