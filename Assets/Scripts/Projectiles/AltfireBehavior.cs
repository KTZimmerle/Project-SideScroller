using UnityEngine;
using System.Collections;

public class AltfireBehavior : ProjectileBehavior {

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y, 0.0f) * acceleration;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!isFriendly)
            return;

        if (other.GetComponent<CircularMover>() != null)
        {
            E1 = other.GetComponent<CircularMover>();
            enemy = E1.ES;
        }
        else if (other.GetComponent<RotatorMover>() != null)
        {
            E2 = other.GetComponent<RotatorMover>();
            enemy = E2.ES;
        }
        else if (other.GetComponent<StraightMover>() != null)
        {
            E3 = other.GetComponent<StraightMover>();
            enemy = E3.ES;
        }
        else if (other.GetComponent<WavyMover>() != null)
        {
            EP = other.GetComponent<WavyMover>();
            enemy = EP.ES;
        }
        else
            return;

        if (enemy.takeDamage(bullet.damage) <= 0 && !isHit)
        {
            gameController.ModifyScore(enemy.getScoreValue());
            enemy.DropOnDeath(other.GetComponent<Mover>().drop, other.transform.position, other.transform.rotation);
            isHit = true;
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }
}
