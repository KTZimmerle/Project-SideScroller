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
    public int LASER_LIMIT = 3;
    public float laserRate;
    float speedModifier;
    public bool isShielded;

    public GameObject missile;
    public GameObject altfire;
    public GameObject laser;
    public GameObject shields;

    protected GameController gameController;
    protected AbstractEnemy enemy;

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
        int numMissiles = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerProjectileList>().getMissiles();
        if (missileRate < 0.0f && missilePowUp && numMissiles
            < MISSILE_LIMIT)
        {
            GameObject clone;
            Vector3 offset = transform.position;
            offset.x += 0.5f;
            offset.y -= 0.25f;
            clone = Instantiate(missile, offset, transform.rotation) as GameObject;
            clone.GetComponent<ProjectileBehavior>().isFriendly = true;
            missileRate = 0.5f;
        }
    }

    //alternate shot
    public void AltShoot()
    {
        int numAltFires = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerProjectileList>().getAltFires();
        if (altfireRate < 0.0f && altfirePowUp && numAltFires
            < ALTFIRE_LIMIT && !laserPowUp)
        {
            GameObject clone;
            Vector3 offsetX = transform.position;
            offsetX.x += 0.75f;
            clone = Instantiate(altfire, offsetX, Quaternion.Euler(0.0f, 0.0f, 30.0f) * transform.rotation) as GameObject;
            clone.GetComponent<ProjectileBehavior>().isFriendly = true;
            clone = Instantiate(altfire, offsetX, Quaternion.Euler(0.0f, 0.0f, -30.0f) * transform.rotation) as GameObject;
            clone.GetComponent<ProjectileBehavior>().isFriendly = true;
            altfireRate = 0.25f;
        }
    }

    //lasers
    public void LaserShoot()
    {
        int numLasers = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerProjectileList>().getLasers();
        if (laserRate < 0.0f && laserPowUp && numLasers < LASER_LIMIT)
        {
            foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
            {
                if (ps.CompareTag("Laser_FX"))
                    ps.Play();
            }

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
            Vector3 offsetX = transform.position;
            offsetX.x += 0.25f;
            clone = Instantiate(shields, offsetX, transform.rotation) as GameObject;
            clone.transform.SetParent(transform);
        }
    }

    //super bomb
    public void ActivateSuperBomb()
    {
        Collider[] targets;
        Collider[] projectiles;
        targets = Physics.OverlapSphere(transform.position, 25.0f, 1 << 8 | 1 << 12, QueryTriggerInteraction.Collide);
        projectiles = Physics.OverlapSphere(transform.position, 25.0f, 1 << 9, QueryTriggerInteraction.Collide);
        StartCoroutine(AnnihilateEnemies(targets, projectiles));
    }

    IEnumerator AnnihilateEnemies(Collider[] targets, Collider[] projs)
    {
        foreach (Collider potenTarget in targets)
        {
            enemy = GetComponent<OnHitHandler>().OnHitHandle(potenTarget);

            if (enemy == null)
                continue;


            gameController.ModifyScore(enemy.getScoreValue());
            enemy.DropOnDeath(potenTarget.transform.position, potenTarget.transform.rotation);
            enemy.PlayExplosion(potenTarget.transform.position, potenTarget.transform.rotation);
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
        laserPowUp = false;
        missileRate = 0.5f;
        altfireRate = 0.25f;
        laserRate = 1.0f;
    }
}
