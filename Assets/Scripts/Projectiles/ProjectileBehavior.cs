using UnityEngine;
using System.Collections;

public class ProjectileBehavior : MonoBehaviour {

    protected Bullet bullet;
    public float acceleration;
    public bool isFriendly;
    public bool isHit;
    protected GameController gameController;
    protected AbstractEnemy enemy;
    protected CircularMover E1;
    protected RotatorMover E2;
    protected StraightMover E3;
    protected TrackingMover E4;
    protected WavyMover EP;
    protected FirstBossRoutine BE;
    protected BossBarrierBehavior Barrier;
    //Ray r;

    protected virtual void Awake()
    {
        GameObject target = GameObject.FindWithTag("GameController");
        if (target.GetComponent<GameController>() != null)
            gameController = target.GetComponent<GameController>();
        bullet = new Bullet();
        isHit = false;
    }

    void FixedUpdate()
	{
        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x + acceleration, GetComponent<Rigidbody>().velocity.y, 0.0f);
	}

    protected virtual void OnTriggerEnter(Collider other)
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

        if (enemy.takeDamage(bullet.damage) <= 0)
        {
            if(other.GetComponent<Mover>() != null)
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
