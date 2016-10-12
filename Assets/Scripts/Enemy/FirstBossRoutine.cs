using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FirstBossRoutine : MonoBehaviour {

    public int hitPoints;
    public int scoreValue;
    public float veloc;
    public float speed;
    public bool notDead;
    public BossShip BE;
    public GameObject projectile;
    public GameObject projectileTwo;
    public GameObject laser;
    public GameObject gunPointOne;
    public GameObject gunPointTwo;
    public GameObject gunPointThree;
    public GameObject gunPointFour;
    public GameObject gunPointFive;
    public GameObject gunPointSix;
    public GameObject gunPointSeven;
    public GameObject gunPointEight;
    public GameObject lasPointOne;
    public GameObject lasPointTwo;
    GameController gameController;

    protected IEnumerator BossAttacks;
    protected IEnumerator LaserPattern;
    protected bool stopLaser = false;

    void Init()
    {
        BossAttacks = BossRoutine();
        notDead = true;
        veloc = 1.0f;
        GetComponent<Rigidbody>().velocity = transform.right * speed;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
    }

    // Use this for initialization
    protected virtual void Awake()
    {
        BE = new BossShip(hitPoints, scoreValue);
        GameObject target = GameObject.FindWithTag("GameController");
        if (target.GetComponent<GameController>() != null)
            gameController = target.GetComponent<GameController>();
        gameObject.SetActive(false);
    }
    

    protected void OnEnable()
    {
        Init();
        if(GetComponent<FirstBossRoutineHard>() == null)
            StartCoroutine(BossAttacks);
    }

    void OnDisable()
    {
        BE.Init(hitPoints);
        //CancelInvoke();
    }

    protected void Start()
    {
    }
    
    IEnumerator BossRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            ps.Stop();
        }

        while (notDead)
        {
            //phase 1
            yield return new WaitForSeconds(2.0f);
            for (int i = 0; i < 50; i++)
            {
                BE.Shoot(FireBullet(), gunPointOne.transform.position, transform.rotation);
                BE.Shoot(FireBullet(), gunPointTwo.transform.position, transform.rotation);
                BE.Shoot(FireBullet(), gunPointThree.transform.position, transform.rotation);
                BE.Shoot(FireBullet(), gunPointFour.transform.position, transform.rotation);
                foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
                {
                    if (ps.CompareTag("Muzzle_FX"))
                        ps.Play();
                }
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(2.0f);

            //phase 2
            GetComponent<Rigidbody>().velocity = transform.up * veloc;

            for(int i = 0; i < 16; i++)
            {
                BE.Shoot(FireBolt(), gunPointFive.transform.position, transform.rotation);
                BE.Shoot(FireBolt(), gunPointSix.transform.position, transform.rotation);
                BE.Shoot(FireBolt(), gunPointSeven.transform.position, transform.rotation);
                BE.Shoot(FireBolt(), gunPointEight.transform.position, transform.rotation);
                yield return new WaitForSeconds(1.0f);

                if (!(this.transform.position.y > -1.8f && this.transform.position.y < 1.8f))
                {
                    //GetComponent<Rigidbody>().velocity = Vector3.zero;
                    GetComponent<Rigidbody>().velocity *= -1;
                }
            }

            if (this.transform.position.y < 0.0f)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().position = new Vector3(GetComponent<Rigidbody>().position.x, 0.0f, 0.0f);

            //phase 3
            yield return new WaitForSeconds(2.0f);
            PlayLasFX();
            BE.ShootLaser(FireLaser(), lasPointOne.transform.position, transform.rotation);
            BE.ShootLaser(FireLaser(), lasPointTwo.transform.position, transform.rotation);
            LaserPattern = LaserPatternEasy();
            StartCoroutine(LaserPattern);
            yield return new WaitForSeconds(16.0f);
            //repeat from the top
        }

    }

    public void killBoss()
    {
        if (!notDead)
            return;
        
        StopCoroutine(BossAttacks);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        //StartCoroutine(destroyBoss());
        InvokeRepeating("destroyBoss", 3.0f, 0.5f);
        notDead = false;
        stopLaser = true;
        gameController.ModifyScore(BE.getScoreValue());
        //GetComponent<BoxCollider>().enabled = false;

    }

    void destroyBoss()
    {
        if (transform.GetChild(2).gameObject.activeSelf)
        {
            //Destroy(transform.GetChild(transform.childCount - 1).gameObject);
            transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (transform.GetChild(1).gameObject.activeSelf)
        {
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else if (transform.GetChild(0).gameObject.activeSelf)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            CancelInvoke();
            Invoke("destroyCore", 1.0f);
        }
        //gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    void destroyCore()
    {
        gameObject.SetActive(false);
    }

    /*IEnumerator destroyBoss()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(1.0f);
        while (transform.childCount > 0)
        {
            //Destroy(transform.GetChild(transform.childCount - 1).gameObject);
            transform.GetChild(transform.childCount - 1).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }*/

    IEnumerator LaserPatternEasy()
    {
        int[] angles = { 120, -120, 135, -135, 150, -150 };
        yield return new WaitForSeconds(3.0f);
        if (stopLaser)
        {
            StopLasFX();
            yield break;
        }

        for (int i = 0; i < 3; i++)
        {
            BE.ShootRLaser(gunPointSix.transform.position, angles[i * 2]);
            BE.ShootRLaser(gunPointSeven.transform.position, angles[i * 2 + 1]);
            yield return new WaitForSeconds(4.0f);
            if (stopLaser)
            {
                StopLasFX();
                yield break;
            }
        }
    }

    protected void PlayLasFX()
    {
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            if (ps.CompareTag("Laser_FX"))
                ps.Play();
        }
    }

    protected void StopLasFX()
    {
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            if (ps.CompareTag("Laser_FX"))
                ps.time = ps.startLifetime - (ps.startLifetime * 0.95f);
        }
    }

    protected GameObject FireBullet()
    {
        return gameController.GetComponent<ProjectilePool>().FireNextBullet(this.gameObject);
    }

    protected GameObject FireBolt()
    {
        return gameController.GetComponent<ProjectilePool>().FireNextBolt(this.gameObject);
    }

    protected GameObject FireLaser()
    {
        return gameController.GetComponent<ProjectilePool>().FireNextLaser(this.gameObject);
    }
}
