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
    GameObject shield;

    protected GameController gameController;
    protected AbstractEnemy enemy;
    
    void Awake()
    {
        Vector3 offsetX = transform.position;
        offsetX.x += 0.25f;
        shield = Instantiate(shields, offsetX, transform.rotation) as GameObject;
        shield.transform.SetParent(transform);
        GameObject target = GameObject.FindWithTag("GameController");
        if (target.GetComponent<GameController>() != null)
            gameController = target.GetComponent<GameController>();
    }

    void OnEnable()
    {
        isShielded = false;
        speedModifier = 1.0f;
        if (altfirePowUp || laserPowUp)
        {
            altfirePowUp = false;
            laserPowUp = false;
        }
        else
        {
            missilePowUp = false;
        }
        missileRate = 0.5f;
        altfireRate = 0.25f;
        laserRate = 1.0f;
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
        int numMissiles = GameObject.FindGameObjectWithTag("GameController").GetComponent<ProjectilePool>().getMissiles();
        GameObject miss = GameObject.FindGameObjectWithTag("GameController").GetComponent<ProjectilePool>().RequestMissile();
        if (missileRate < 0.0f && missilePowUp && miss != null)
        {
            //GameObject clone;
            Vector3 offset = transform.position;
            offset.x += 0.5f;
            offset.y -= 0.25f;
            miss.transform.position = offset;
            miss.transform.rotation = transform.rotation;
            miss.gameObject.SetActive(true);
            //clone = Instantiate(missile, offset, transform.rotation) as GameObject;
            miss.GetComponent<ProjectileBehavior>().isFriendly = true;
            missileRate = 0.5f;
        }
    }

    //alternate shot
    public void AltShoot()
    {
        int numAltFires = GameObject.FindGameObjectWithTag("GameController").GetComponent<ProjectilePool>().getAltfires();
        if (numAltFires >= 2 && altfireRate < 0.0f && altfirePowUp && !laserPowUp)
        {
            float[] angles = {30.0f, -30.0f };
            for (int i = 0; i < 2; i++)
            {
                GameObject altfire1 = GameObject.FindGameObjectWithTag("GameController").GetComponent<ProjectilePool>().RequestAltfire();
                if (altfire1 != null)
                {
                    Vector3 offsetX = transform.position;
                    offsetX.x += 0.75f;
                    altfire1.transform.position = offsetX;
                    altfire1.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angles[i]) * Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    altfire1.GetComponent<ProjectileBehavior>().isFriendly = true;
                    altfire1.gameObject.SetActive(true);
                }
            }
            altfireRate = 0.25f;
        }
    }

    //lasers
    public void LaserShoot()
    {
        int numLasers = GameObject.FindGameObjectWithTag("GameController").GetComponent<ProjectilePool>().getLasers();
        GameObject laser = GameObject.FindGameObjectWithTag("GameController").GetComponent<ProjectilePool>().RequestLaser();
        if (laserRate < 0.0f && laserPowUp && laser != null)
        {
            foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
            {
                if (ps.CompareTag("Laser_FX"))
                    ps.Play();
            }

            //GameObject clone;
            //clone = Instantiate(laser, transform.position, transform.rotation) as GameObject;
            laser.transform.position = transform.position;
            laser.transform.rotation = transform.rotation;
            laser.GetComponent<LaserBehavior>().isFriendly = true;
            laser.gameObject.SetActive(true);
            laserRate = 1.0f;
        }
    }

    //shields
    public void ActivateShields()
    {
        if (!isShielded)
        {
            shield.gameObject.SetActive(true);
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
            enemy = GetComponent<OnHitHandler>().OnHitHandle(potenTarget, gameController);

            if (enemy == null)
                continue;


            gameController.ModifyScore(enemy.getScoreValue());
            enemy.DropOnDeath(potenTarget.transform.position, potenTarget.transform.rotation);
            GetComponent<OnHitHandler>().OnHitLogic(potenTarget, gameController, enemy);
            potenTarget.gameObject.SetActive(false);
        }

        foreach (Collider potenTarget in projs)
        {
            //Destroy(potenTarget.gameObject);
            potenTarget.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.5f);
    }
}
