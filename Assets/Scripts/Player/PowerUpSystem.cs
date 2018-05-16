using UnityEngine;
using System.Collections;

public class PowerUpSystem : MonoBehaviour {

    public bool missilePowUp;
    public bool altfirePowUp;
    public bool laserPowUp;
    public float missileRate;
    public float missileRateTwo;
    public float missileRateThree;
    public float altfireRate;
    public int shieldHits = 5;
    public float laserRate;
    float speedModifier;
    public bool isShielded;
    float sharpness;
    public string pool;
    GameObject orbiterOne;
    GameObject orbiterTwo;

    public GameObject shields;
    GameObject shield;

    protected GameController gameController;
    protected AbstractEnemy enemy;
    ProjectilePool projPoolMain;
    ProjectilePool projPoolOne;
    ProjectilePool projPoolTwo;

    void Awake()
    {
        projPoolMain = GameObject.FindGameObjectWithTag("ProjectilePool").GetComponent<ProjectilePool>();
        projPoolOne = GameObject.FindGameObjectWithTag("OrbiterProjPoolOne").GetComponent<ProjectilePool>();
        projPoolTwo = GameObject.FindGameObjectWithTag("OrbiterProjPoolTwo").GetComponent<ProjectilePool>();
        sharpness = 0.0f;
        Vector3 offsetX = transform.position;
        offsetX.x += 0.25f;
        shield = Instantiate(shields, offsetX, transform.rotation) as GameObject;
        shield.transform.SetParent(transform);
        GameObject target = GameObject.FindWithTag("GameController");
        if (target != null)
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
        missileRateTwo = 0.5f;
        missileRateThree = 0.5f;
        altfireRate = 0.25f;
        laserRate = 1.0f;
    }

    void FixedUpdate()
    {
        float verticalDir = Input.GetAxis("PlayerShipV") * Time.smoothDeltaTime;
        float horizontalDir = Input.GetAxis("PlayerShipH") * Time.smoothDeltaTime;

        if (horizontalDir > 0.01f || horizontalDir < -0.01f)
        {
            sharpness = 0.0f;
        }
        else if (verticalDir > 0.01f)
        {
            sharpness = 2.5f;
        }
        else if (verticalDir < -0.01f)
        {
            sharpness = -2.5f;
        }
        
    }

    //speed up
    public void changeSpeed()
    {
        if(GetComponent<PlayerController>().speed + .6f <= 6.0f)
            GetComponent<PlayerController>().speed += 0.6f;
        else
            GetComponent<PlayerController>().speed = 3.0f;

    }

    public void LaunchMissile(Vector3 pos, Quaternion rot, GameObject missile)
    {
        missile.GetComponent<ProjectileBehavior>().isFriendly = true;
        missile.transform.position = pos;
        missile.transform.rotation = rot;
        missile.gameObject.SetActive(true);
    }

    //missiles
    public void FireMissiles()
    {
        GameObject missileOne = projPoolMain.RequestMissile();
        GameObject missileTwo = null;
        GameObject missileThree = null;

        if (orbiterOne != null && orbiterOne.activeSelf)
            missileTwo = projPoolOne.RequestMissile();

        if (orbiterTwo != null && orbiterTwo.activeSelf)
            missileThree = projPoolTwo.RequestMissile();

        Vector3 offset = transform.position;
        offset.x += 0.5f;
        offset.y -= 0.25f;
        if (missilePowUp)
        {
            if (missileRate < 0.0f && missileOne != null)
            {
                LaunchMissile(offset, transform.rotation, missileOne);
                missileRate = 0.5f;
            }

            if (missileRateTwo < 0.0f && orbiterOne != null && orbiterOne.activeSelf && missileTwo != null)
            {
                LaunchMissile(orbiterOne.transform.position, transform.rotation, missileTwo);
                missileRateTwo = 0.5f;
            }

            if (missileRateThree < 0.0f && orbiterTwo != null && orbiterTwo.activeSelf && missileThree != null)
            {
                LaunchMissile(orbiterTwo.transform.position, transform.rotation, missileThree);
                missileRateThree = 0.5f;
            }
        }
    }

    //alternate shot
    public void AltShoot()
    {
        int numAltFires = projPoolMain.getAltfires();
        if (numAltFires >= 2 && altfireRate < 0.0f && altfirePowUp && !laserPowUp)
        {
            float[] angles = {90.0f, -90.0f };
            for (int i = 0; i < 2; i++)
            {
                GameObject altfire1 = projPoolMain.RequestAltfire();
                GameObject altfire2 = null;
                GameObject altfire3 = null; ;
                if (orbiterOne != null && orbiterOne.activeSelf)
                    altfire2 = projPoolOne.RequestAltfire();

                if (orbiterTwo != null && orbiterTwo.activeSelf)
                    altfire3 = projPoolTwo.RequestAltfire();

                if (altfire1 != null)
                {
                    Vector3 offsetX = transform.position;
                    offsetX.x += 0.25f;
                    altfire1.transform.position = offsetX;
                    altfire1.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angles[i]) * Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    altfire1.GetComponent<ProjectileBehavior>().isFriendly = true;
                    altfire1.gameObject.SetActive(true);
                }

                if (altfire2 != null)
                {
                    Vector3 offsetX = orbiterOne.transform.position;
                    offsetX.x += 0.25f;
                    altfire2.transform.position = offsetX;
                    altfire2.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angles[i]) * Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    altfire2.GetComponent<ProjectileBehavior>().isFriendly = true;
                    altfire2.gameObject.SetActive(true);
                }

                if (altfire3 != null)
                {
                    Vector3 offsetX = orbiterTwo.transform.position;
                    offsetX.x += 0.25f;
                    altfire3.transform.position = offsetX;
                    altfire3.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angles[i]) * Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    altfire3.GetComponent<ProjectileBehavior>().isFriendly = true;
                    altfire3.gameObject.SetActive(true);
                }
            }
            altfireRate = 0.15f;
        }
    }

    //lasers
    public void LaserShoot()
    {
        //GameObject laser = GameObject.FindGameObjectWithTag("ProjectilePool").GetComponent<ProjectilePool>().RequestLaser();
        if (laserRate < 0.0f && laserPowUp)
        {
            foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
            {
                if (ps.CompareTag("Laser_FX"))
                    ps.Play();
            }
            
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;
            StartCoroutine(FireBurst(10, pos, rot, sharpness));
            laserRate = 0.55f;
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

    IEnumerator FireBurst(int burstLength, Vector3 pos, Quaternion rot, float sharp)
    {
        Vector3 OrbOnePos = Vector3.zero;
        Vector3 OrbTwoPos = Vector3.zero;

        if (orbiterOne != null & orbiterOne.activeSelf)
        {
            OrbOnePos = orbiterOne.transform.position;
        }

        if (orbiterTwo != null & orbiterTwo.activeSelf)
        {
            OrbTwoPos = orbiterTwo.transform.position;
        }
        
        int numLasers = projPoolMain.getLasers();
        if (numLasers <= 0)
            yield break;


        GameObject[] lasers = new GameObject[burstLength];
        GameObject[] lasers2 = new GameObject[burstLength];
        GameObject[] lasers3 = new GameObject[burstLength];
        int length = lasers.Length;
        for (int index = 0; index < length; index++)
        {
            lasers[index] = projPoolMain.GetComponent<ProjectilePool>().RequestLaser();

            lasers[index].GetComponent<BezierLaserBehavior>().sharpness = sharp;
            lasers[index].transform.position = pos;
            lasers[index].transform.rotation = rot;
            lasers[index].GetComponent<BezierLaserBehavior>().isFriendly = true;
            lasers[index].SetActive(true);

            if (orbiterOne != null & orbiterOne.activeSelf)
            {
                lasers2[index] = projPoolOne.GetComponent<ProjectilePool>().RequestLaser();

                lasers2[index].GetComponent<BezierLaserBehavior>().sharpness = sharp;
                lasers2[index].transform.position = OrbOnePos;
                lasers2[index].transform.rotation = rot;
                lasers2[index].GetComponent<BezierLaserBehavior>().isFriendly = true;
                //yield return new WaitForSeconds(0.025f);
                lasers2[index].SetActive(true);
            }

            if (orbiterTwo != null & orbiterTwo.activeSelf)
            {
                lasers3[index] = projPoolTwo.GetComponent<ProjectilePool>().RequestLaser();

                lasers3[index].GetComponent<BezierLaserBehavior>().sharpness = sharp;
                lasers3[index].transform.position = OrbTwoPos;
                lasers3[index].transform.rotation = rot;
                lasers3[index].GetComponent<BezierLaserBehavior>().isFriendly = true;
                //yield return new WaitForSeconds(0.025f);
                lasers3[index].SetActive(true);
            }

            yield return new WaitForSeconds(0.025f);
        }
    }
    
    public void SetOrbiterOneRef(GameObject orbOne)
    {
        orbiterOne = orbOne;
    }

    public void SetOrbiterTwoRef(GameObject orbTwo)
    {
        orbiterTwo = orbTwo;
    }

    public bool CheckMissilePowUp()
    {
        return missilePowUp;
    }

    public bool CheckCrossFirePowUp()
    {
        return altfirePowUp;
    }

    public bool CheckLaserPowUp()
    {
        return laserPowUp;
    }
}
