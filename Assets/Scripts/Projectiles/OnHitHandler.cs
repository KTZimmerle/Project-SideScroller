using UnityEngine;
using System.Collections;

public class OnHitHandler : MonoBehaviour {

    protected CircularMover E1;
    protected RotatorMover E2;
    protected StraightMover E3;
    protected TrackingMover E4;
    protected WavyMover EP;
    protected IBossKill BK;
    protected FirstBossRoutine BE;
    protected AIBossDrone BE2;
    protected BossBarrierBehavior Barrier;
    protected ScoreBoard sb;
    protected SpawnWaves sw;
    protected SpecialFXPool gfx;
    protected ShipPool itemTable;
    GameObject exp;
    GameObject drop;
    Vector3 lastPosition;

    void Start()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        if (gameController != null)
        {
            sb = gameController.GetComponent<ScoreBoard>();
            sw = gameController.GetComponent<SpawnWaves>();
        }
        else
        {
            sb = null;
            sw = null;
        }
        gfx = GameObject.FindGameObjectWithTag("GFXPool").GetComponent<SpecialFXPool>();
        itemTable = GameObject.FindGameObjectWithTag("ShipPool").GetComponent<ShipPool>();
    }

    public AbstractEnemy OnHitHandle(Collider other, GameController gameController)
    {
        AbstractEnemy e;
        if (other.GetComponent<CircularMover>() != null)
        {
            E1 = other.GetComponent<CircularMover>();
            e = E1.ES;
            exp = gfx.playSmallExplosion();
            drop = itemTable.SpawnSpeedPowerUp();
        }
        else if (other.GetComponent<RotatorMover>() != null)
        {
            E2 = other.GetComponent<RotatorMover>();
            e = E2.ES;
            exp = gfx.playMediumExplosion();
            drop = itemTable.SpawnLaserPowerUp();
        }
        else if (other.GetComponent<StraightMover>() != null)
        {
            E3 = other.GetComponent<StraightMover>();
            e = E3.ES;
            exp = gfx.playSmallExplosion();
            drop = null;
        }
        else if (other.GetComponent<TrackingMover>() != null)
        {
            E4 = other.GetComponent<TrackingMover>();
            e = E4.ES;
            exp = gfx.playMediumExplosion();
            drop = itemTable.SpawnMissilePowerUp();
        }
        else if (other.GetComponent<WavyMover>() != null)
        {
            EP = other.GetComponent<WavyMover>();
            e = EP.ES;
            exp = gfx.playSmallExplosion();
            drop = null;
        }
        else if (other.GetComponent<FirstBossRoutine>() != null)
        {
            BE = other.GetComponent<FirstBossRoutine>();
            e = BE.BE;
        }
        else if (other.GetComponent<AIBossDrone>() != null)
        {
            BE2 = other.GetComponent<AIBossDrone>();
            e = BE2.BE;
        }
        else if (other.GetComponent<BossBarrierBehavior>() != null)
        {
            Barrier = other.GetComponent<BossBarrierBehavior>();
            e = Barrier.ES;
            exp = gfx.playSmallExplosion();
        }
        else
        {
            e = null;
            exp = null;
            drop = null;
        }

        return e;
    }

    public void OnHitLogic(Collider other, GameController gameController, AbstractEnemy enemy)
    {
        lastPosition = other.transform.position;
        enemy.DropOnDeath(lastPosition, other.transform.rotation, drop);
        if (!enemy.isBoss())
        {
            gameController.ModifyScore(enemy.getScoreValue());
            //exp = gfx.playSmallExplosion();
            //enemy.PlayExplosion(other.transform.position, other.transform.rotation);
            exp.transform.position = other.transform.position;
            exp.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 359.0f));
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            exp.SetActive(true);
            if (other.CompareTag("Hazard"))
            {
                if (sb != null)
                {
                    sb.incrementHit();
                    sb.incrementEnemyHit(other.gameObject);
                }

                if (sw != null)
                {
                    sw.decrementEnemyCount();
                }
            }
        }
        else
        {
            BK = other.GetComponent<IBossKill>();
            BK.Kill();
            gameObject.SetActive(false);
        }
    }
}
