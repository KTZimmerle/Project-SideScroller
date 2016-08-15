using UnityEngine;
using System.Collections;

public class MissileBehavior : ProjectileBehavior {

    public Missile missile;
    Collider[] targets;
    Collider closestTarget;
    float explosionRadius;

    protected override void Awake()
    {
        base.Awake();
        missile = new Missile();
        closestTarget = null;
        Rigidbody rb = GetComponent<Rigidbody>();
        veloc = speed;
    }

    void Start()
    {
        if (isFriendly)
            gameController.GetComponent<PlayerProjectileList>().addMissile(gameObject);
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
        
        GetComponent<Rigidbody>().velocity = transform.right * veloc;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!isFriendly || isHit)
            return;

        if (other.GetComponent<BossArmorBehavior>() != null)
        {
            gameController.GetComponent<PlayerProjectileList>().removeMissile(gameObject);
            Destroy(gameObject);
            return;
        }

        enemy = GetComponent<OnHitHandler>().OnHitHandle(other);

        if (enemy == null)
            return;

        if (enemy.getDeathStatus())
            return;

        if (enemy.takeDamage(missile.damage) <= 0)
        {
            enemy.DropOnDeath(other.transform.position, other.transform.rotation);
            isHit = true;
            if (!enemy.isBoss())
            {
                gameController.ModifyScore(enemy.getScoreValue());
                enemy.PlayExplosion(other.transform.position, other.transform.rotation);
                Destroy(other.gameObject);
            }
            else
            {
                BE = other.GetComponent<FirstBossRoutine>();
                BE.killBoss();
            }
        }

        gameController.GetComponent<PlayerProjectileList>().removeMissile(gameObject);
        Destroy(gameObject);
    }
}
