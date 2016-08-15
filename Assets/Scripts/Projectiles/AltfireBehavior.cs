using UnityEngine;
using System.Collections;

public class AltfireBehavior : ProjectileBehavior {

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
	}

    void Start()
    {
        if(isFriendly)
            gameController.GetComponent<PlayerProjectileList>().addAltFire(gameObject);
    }
	
	void FixedUpdate ()
    {
        if (isFriendly)
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y, 0.0f) * acceleration;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!isFriendly || isHit)
            return;

        if (other.GetComponent<BossArmorBehavior>() != null)
        {
            gameController.GetComponent<PlayerProjectileList>().removeAltFire(gameObject);
            Destroy(gameObject);
            return;
        }
        
        enemy = GetComponent<OnHitHandler>().OnHitHandle(other);

        if (enemy == null)
            return;

        if (enemy.getDeathStatus())
            return;

        if (enemy.takeDamage(bullet.damage) <= 0)
        {
            gameController.ModifyScore(enemy.getScoreValue());
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

        gameController.GetComponent<PlayerProjectileList>().removeAltFire(gameObject);
        Destroy(gameObject);
    }
}
