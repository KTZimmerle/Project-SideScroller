using UnityEngine;
using System.Collections;

public class MissileBehavior : ProjectileBehavior {

    Missile missile;
    //GameObject[] targets;
    Collider[] targets;
    Collider closestTarget;
    float explosionRadius;
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
            //targets = GameObject.FindGameObjectsWithTag("Hazard");
            targets = Physics.OverlapSphere(transform.position, 25.0f, 1 << 8, QueryTriggerInteraction.Collide);
            float dist = Mathf.Infinity;
            Vector3 pos = transform.position;
            foreach (Collider potenTarget in targets)
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
        if (!isFriendly || isHit)
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
        else if (other.GetComponent<TrackingMover>() != null)
        {
            E4 = other.GetComponent<TrackingMover>();
            enemy = E4.ES;
        }
        else if (other.GetComponent<WavyMover>() != null)
        {
            EP = other.GetComponent<WavyMover>();
            enemy = EP.ES;
        }
        else if (other.GetComponent<FirstBossRoutine>() != null)
        {
            BE = other.GetComponent<FirstBossRoutine>();
            enemy = BE.BE;
        }
        else if (other.GetComponent<BossBarrierBehavior>() != null)
        {
            Barrier = other.GetComponent<BossBarrierBehavior>();
            enemy = Barrier.ES;
        }
        else if (other.GetComponent<BossArmorBehavior>() != null)
        {
            Destroy(gameObject);
            return;
        }
        else
            return;

        if (enemy.takeDamage(missile.damage) <= 0)
        {
            if (other.GetComponent<Mover>() != null)
                enemy.DropOnDeath(other.GetComponent<Mover>().drop, other.transform.position, other.transform.rotation);
            isHit = true;
            if (!enemy.isBoss())
            {
                gameController.ModifyScore(enemy.getScoreValue());
                Destroy(other.gameObject);
            }
            else
                BE.killBoss();
        }

        Destroy(gameObject);
    }
}
