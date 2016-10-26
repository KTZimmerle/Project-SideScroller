using UnityEngine;
using System.Collections;

public class OnHitHandler : MonoBehaviour {

    protected CircularMover E1;
    protected RotatorMover E2;
    protected StraightMover E3;
    protected TrackingMover E4;
    protected WavyMover EP;
    protected FirstBossRoutine BE;
    protected BossBarrierBehavior Barrier;
    protected ScoreBoard sb;
    protected SpawnWaves sw;
    GameObject exp;

    void Start()
    {
        sb = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreBoard>();
        sw = GameObject.FindGameObjectWithTag("GameController").GetComponent<SpawnWaves>();
    }

    public AbstractEnemy OnHitHandle(Collider other, GameController gameController)
    {
        AbstractEnemy e;
        if (other.GetComponent<CircularMover>() != null)
        {
            E1 = other.GetComponent<CircularMover>();
            e = E1.ES;
            exp = gameController.GetComponent<SpecialFXPool>().playSmallExplosion();
        }
        else if (other.GetComponent<RotatorMover>() != null)
        {
            E2 = other.GetComponent<RotatorMover>();
            e = E2.ES;
            exp = gameController.GetComponent<SpecialFXPool>().playMediumExplosion();
        }
        else if (other.GetComponent<StraightMover>() != null)
        {
            E3 = other.GetComponent<StraightMover>();
            e = E3.ES;
            exp = gameController.GetComponent<SpecialFXPool>().playSmallExplosion();
        }
        else if (other.GetComponent<TrackingMover>() != null)
        {
            E4 = other.GetComponent<TrackingMover>();
            e = E4.ES;
            exp = gameController.GetComponent<SpecialFXPool>().playMediumExplosion();
        }
        else if (other.GetComponent<WavyMover>() != null)
        {
            EP = other.GetComponent<WavyMover>();
            e = EP.ES;
            exp = gameController.GetComponent<SpecialFXPool>().playSmallExplosion();
        }
        else if (other.GetComponent<FirstBossRoutine>() != null)
        {
            BE = other.GetComponent<FirstBossRoutine>();
            e = BE.BE;
        }
        else if (other.GetComponent<BossBarrierBehavior>() != null)
        {
            Barrier = other.GetComponent<BossBarrierBehavior>();
            e = Barrier.ES;
            exp = gameController.GetComponent<SpecialFXPool>().playSmallExplosion();
        }
        else
        {
            e = null;
            exp = null;
        }

        return e;
    }

    public void OnHitLogic(Collider other, GameController gameController, AbstractEnemy enemy)
    {
        enemy.DropOnDeath(other.transform.position, other.transform.rotation);
        if (!enemy.isBoss())
        {
            gameController.ModifyScore(enemy.getScoreValue());
            //exp = gameController.GetComponent<SpecialFXPool>().playSmallExplosion();
            //enemy.PlayExplosion(other.transform.position, other.transform.rotation);
            exp.transform.position = other.transform.position;
            exp.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 359.0f));
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            exp.SetActive(true);
            if (other.CompareTag("Hazard"))
            {
                sb.incrementHit();
                sb.incrementEnemyHit(other.gameObject);
                sw.decrementEnemyCount();
            }
        }
        else
        {
            BE = other.GetComponent<FirstBossRoutine>();
            BE.killBoss();
        }
    }
}
