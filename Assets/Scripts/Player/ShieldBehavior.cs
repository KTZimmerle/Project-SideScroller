using UnityEngine;
using System.Collections;

public class ShieldBehavior : MonoBehaviour {

    int hitpoints;
    PowerUpSystem PowUp;
    Collider player;
    protected GameController gameController;
    protected AbstractEnemy enemy;
    GameUI gameUI;

    void Awake ()
    {
        gameUI = (GameUI)FindObjectOfType(typeof(GameUI));
        GameObject target = GameObject.FindWithTag("GameController");
        if (target != null)
            gameController = target.GetComponent<GameController>();
        player = transform.parent.GetComponent<Collider>();
        player.isTrigger = false;
        gameController.setShieldStatus(false);
        PowUp = transform.parent.GetComponent<PowerUpSystem>();
        //gameObject.SetActive(false);
    }

    void OnEnable()
    {
        hitpoints = PowUp.shieldHits;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("EnemyProj"))
        {
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
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
            enemy = GetComponent<OnHitHandler>().OnHitHandle(other, gameController);

            if (enemy == null)
                return;

            if (enemy.getDeathStatus())
                return;

            if (enemy.takeDamage(1) <= 0)
            {
                GetComponent<OnHitHandler>().OnHitLogic(other, gameController, enemy);
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
            //Destroy(gameObject);
            gameUI.RestoreShieldText();
            gameObject.SetActive(false);
        }
    }
}
