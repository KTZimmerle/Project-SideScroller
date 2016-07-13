using UnityEngine;
using System.Collections;

public class ShieldBehavior : MonoBehaviour {

    int hitpoints;
    PowerUpSystem PowUp;
    Collider player;
    protected GameController gameController;
    protected AbstractEnemy enemy;
    protected CircularMover E1;
    protected RotatorMover E2;
    protected StraightMover E3;
    protected WavyMover EP;

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
        else if(other.CompareTag("Hazard"))
        {
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

            if (enemy.takeDamage(1) <= 0)
            {
                gameController.ModifyScore(enemy.getScoreValue());
                enemy.DropOnDeath(other.GetComponent<Mover>().drop, other.transform.position, other.transform.rotation);
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
