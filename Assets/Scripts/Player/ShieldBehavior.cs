using UnityEngine;
using System.Collections;

public class ShieldBehavior : MonoBehaviour {

    int hitpoints;
    PowerUpSystem PowUp;
    Collider player;
    protected GameController gameController;
    protected AbstractEnemy enemy;

    void Start ()
    {
        GameObject target = GameObject.FindWithTag("GameController");
        if (target != null)
            gameController = target.GetComponent<GameController>();
        player = transform.parent.GetComponent<Collider>();
        player.isTrigger = false;
        gameController.setShieldStatus(true);
        PowUp = transform.parent.GetComponent<PowerUpSystem>();
        hitpoints = PowUp.shieldHits;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("EnemyProj"))
        {
            Destroy(other.gameObject);
            TakeHit();
            return;
        }
        else if (other.CompareTag("BossLaser"))
        {
            TakeHit();
            return;
        }
        else if (other.CompareTag("Hazard"))
        {
            enemy = GetComponent<OnHitHandler>().OnHitHandle(other);

            if (enemy == null)
                return;

            if (enemy.getDeathStatus())
                return;

            if (enemy.takeDamage(1) <= 0)
            {
                gameController.ModifyScore(enemy.getScoreValue());
                enemy.DropOnDeath(other.transform.position, other.transform.rotation);
                enemy.PlayExplosion(other.transform.position, other.transform.rotation);
                Destroy(other.gameObject);
            }

            TakeHit();
        }
    }

    void TakeHit()
    {
        hitpoints--;
        if (hitpoints < 0)
        {
            player.isTrigger = true;
            gameController.setShieldStatus(false);
            PowUp.isShielded = false;
            Destroy(gameObject);
        }
    }
}
