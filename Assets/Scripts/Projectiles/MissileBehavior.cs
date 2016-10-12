using UnityEngine;
using System.Collections;

public class MissileBehavior : ProjectileBehavior {

    public Missile missile;
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

    void OnEnable()
    {
        base.OnEnable();
        /*foreach (Collider potenTarget in targets)
        {
            potenTarget = null;
        }*/
    }

    /*void Start()
    {
        if (isFriendly)
            gameController.GetComponent<PlayerProjectileList>().removeWeapon(gameObject);
    }*/

    void FixedUpdate()
    {
        //find the closest enemy
        if (closestTarget != null && closestTarget.gameObject.activeSelf)
        {
            Vector3 targetPos = closestTarget.transform.position;
            targetPos.z = 0.0f;
            transform.right = targetPos - transform.position;
        }
        else //keep searching the closest enemy available
        {
            Collider[] targets;
            targets = Physics.OverlapSphere(transform.position, 25.0f, 1 << 8 | 1 << 15, QueryTriggerInteraction.Collide);
            float dist = Mathf.Infinity;
            Vector3 pos = transform.position;
            foreach (Collider potenTarget in targets)
            {
                if (potenTarget.gameObject.activeSelf)
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
        }
        
        GetComponent<Rigidbody>().velocity = transform.right * veloc;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!isFriendly || isHit)
            return;

        GameObject exp = gameController.GetComponent<SpecialFXPool>().playMissileExplosion();
        if (other.GetComponent<BossArmorBehavior>() != null)
        {
            //gameController.GetComponent<PlayerProjectileList>().addWeapon(gameObject);
            //Destroy(gameObject);
            gameObject.SetActive(false);
            exp.transform.position = transform.position;
            exp.SetActive(true);
            return;
        }

        enemy = GetComponent<OnHitHandler>().OnHitHandle(other, gameController);

        if (enemy == null)
            return;

        if (enemy.getDeathStatus())
            return;

        isHit = true;
        if (enemy.takeDamage(missile.damage) <= 0)
        {
            GetComponent<OnHitHandler>().OnHitLogic(other, gameController, enemy);
        }

        //gameController.GetComponent<PlayerProjectileList>().addWeapon(gameObject);
        //Destroy(gameObject);
        exp.transform.position = transform.position;
        exp.SetActive(true);
        gameObject.SetActive(false);
    }
}
