using UnityEngine;
using System.Collections;

public class MissileBehavior : ProjectileBehavior {

    Missile missile;
    GameObject[] targets;
    GameObject closestTarget;
    public float veloc;

    protected override void Awake()
    {
        base.Awake();
        missile = new Missile();
        closestTarget = null;
        Rigidbody rb = GetComponent<Rigidbody>();
        veloc = GetComponent<Mover>().speed;
    }

    void FixedUpdate()
    {
        //find the closest enemy
        if (closestTarget != null)
        {
            Vector3 targetPos = closestTarget.transform.position;
            targetPos.z = 0.0f;
            transform.right = targetPos - transform.position;
        }
        else //keep searching the closest enemy available
        {
            targets = GameObject.FindGameObjectsWithTag("Hazard");
            float dist = Mathf.Infinity;
            Vector3 pos = transform.position;
            foreach (GameObject potenTarget in targets)
            {
                Vector3 difference = potenTarget.transform.position - pos;
                float currentDist = difference.sqrMagnitude;
                if (currentDist < dist)
                {
                    closestTarget = potenTarget;
                    dist = currentDist;
                }
            }
        }

        //GetComponent<Rigidbody>().velocity = new Vector3(transform.right, GetComponent<Rigidbody>().velocity.y, 0.0f);
        GetComponent<Rigidbody>().velocity = transform.right * veloc;
	}

    protected override void OnTriggerEnter(Collider other)
    {
        /*if(other.CompareTag("Hazard"))
        {
            Ray self = new Ray(transform.position, transform.up);
            //Physics.SphereCastAll(self, 15.0f);
            Physics.OverlapSphere(transform.position, 15.0f, 0, QueryTriggerInteraction.Collide);
            Debug.DrawRay(self.origin, self.direction, Color.red, 5.0f );
        }*/
        //Debug.Log("hit something");
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

        if (enemy.takeDamage(missile.damage) <= 0)
        {
            gameController.ModifyScore(enemy.getScoreValue());
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }
}
