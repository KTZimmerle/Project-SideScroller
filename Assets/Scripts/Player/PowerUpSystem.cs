using UnityEngine;
using System.Collections;

public class PowerUpSystem : MonoBehaviour {

    public bool missilePowUp;
    public bool altfirePowUp;
    public bool laserPowUp;
    public float missileRate;
    public float altfireRate;
    public int shieldHits = 5;
    public int MISSILE_LIMIT = 1;
    public int ALTFIRE_LIMIT = 4;
    public float laserRate;
    float speedModifier;
    public bool isShielded;

    public GameObject missile;
    public GameObject altfire;
    public GameObject laser;
    public GameObject shields;

    protected GameController gameController;
    protected AbstractEnemy enemy;
    protected CircularMover E1;
    protected RotatorMover E2;
    protected StraightMover E3;
    protected WavyMover EP;

    public bool hasMissilePowerUp()
    {
        return missilePowUp;
    }

    //speed up
    public void changeSpeed()
    {
        if(GetComponent<PlayerController>().speed + 50.0f <= 500.0f)
            GetComponent<PlayerController>().speed += 50.0f;
        else
            GetComponent<PlayerController>().speed = 250.0f;

    }

    //missiles
    public void FireMissiles()
    {
        if (missileRate < 0.0f && missilePowUp && GameObject.FindGameObjectsWithTag("MissileProjectile").Length + 1
            <= MISSILE_LIMIT)
        {
            GameObject clone;
            clone = Instantiate(missile, transform.position, transform.rotation) as GameObject;
            clone.GetComponent<ProjectileBehavior>().isFriendly = true;
            missileRate = 0.5f;
        }
    }

    //alternate shot
    public void AltShoot()
    {
        if (altfireRate < 0.0f && altfirePowUp && GameObject.FindGameObjectsWithTag("AltfireProjectile").Length + 2
            <= ALTFIRE_LIMIT && !laserPowUp)
        {
            GameObject clone;
            clone = Instantiate(altfire, transform.position, Quaternion.Euler(0.0f, 0.0f, 30.0f) * transform.rotation) as GameObject;
            clone.GetComponent<ProjectileBehavior>().isFriendly = true;
            clone = Instantiate(altfire, transform.position, Quaternion.Euler(0.0f, 0.0f, -30.0f) * transform.rotation) as GameObject;
            clone.GetComponent<ProjectileBehavior>().isFriendly = true;
            altfireRate = 0.25f;
        }
    }

    //lasers
    public void LaserShoot()
    {
        if (laserRate < 0.0f && laserPowUp)
        {
            GameObject clone;
            clone = Instantiate(laser, transform.position, transform.rotation) as GameObject;
            clone.GetComponent<LaserBehavior>().isFriendly = true;
            laserRate = 1.0f;
        }
    }

    //shields
    public void ActivateShields()
    {
        if (!isShielded)
        {
            GameObject clone;
            clone = Instantiate(shields, transform.position, transform.rotation) as GameObject;
            clone.transform.SetParent(transform);
        }
    }

    //super bomb
    public void ActivateSuperBomb()
    {
        Collider[] targets;
        Collider[] projectiles;
        targets = Physics.OverlapSphere(transform.position, 25.0f, 1 << 8, QueryTriggerInteraction.Collide);
        projectiles = Physics.OverlapSphere(transform.position, 25.0f, 1 << 9, QueryTriggerInteraction.Collide);
        StartCoroutine(AnnihilateEnemies(targets, projectiles));
    }

    IEnumerator AnnihilateEnemies(Collider[] targets, Collider[] projs)
    {
        foreach (Collider potenTarget in targets)
        {
            if (potenTarget.GetComponent<CircularMover>() != null)
            {
                E1 = potenTarget.GetComponent<CircularMover>();
                enemy = E1.ES;
            }
            else if (potenTarget.GetComponent<RotatorMover>() != null)
            {
                E2 = potenTarget.GetComponent<RotatorMover>();
                enemy = E2.ES;
            }
            else if (potenTarget.GetComponent<StraightMover>() != null)
            {
                E3 = potenTarget.GetComponent<StraightMover>();
                enemy = E3.ES;
            }
            else if (potenTarget.GetComponent<WavyMover>() != null)
            {
                EP = potenTarget.GetComponent<WavyMover>();
                enemy = EP.ES;
            }


            gameController.ModifyScore(enemy.getScoreValue());
            enemy.DropOnDeath(potenTarget.GetComponent<Mover>().drop, potenTarget.transform.position, potenTarget.transform.rotation);
            Destroy(potenTarget.gameObject);
        }

        foreach (Collider potenTarget in projs)
        {
            Destroy(potenTarget.gameObject);
        }
        yield return new WaitForSeconds(0.5f);
    }

    // Use this for initialization
    void Awake ()
    {
        GameObject target = GameObject.FindWithTag("GameController");
        if (target.GetComponent<GameController>() != null)
            gameController = target.GetComponent<GameController>();
        isShielded = false;
        speedModifier = 1.0f;
        missilePowUp = false;
        missileRate = 0.5f;
        altfireRate = 0.25f;
        laserRate = 1.0f;
    }
}
