using UnityEngine;
using System.Collections;

public class FirstBossRoutine : MonoBehaviour {

    public int hitPoints;
    public int scoreValue;
    public float speed;
    public bool notDead;
    public BossShip BE;
    public GameObject projectile;
    public GameObject projectileTwo;
    public GameObject laser;
    public GameObject reflectLaser;
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

    // Use this for initialization
    protected virtual void Awake()
    {
        BossAttacks = BossRoutine();
        notDead = true;
        speed = 1.0f;
        BE = new BossShip(hitPoints, scoreValue);
        GameObject target = GameObject.FindWithTag("GameController");
        if (target.GetComponent<GameController>() != null)
            gameController = target.GetComponent<GameController>();
    }

    protected void Start()
    {
        StartCoroutine(BossAttacks);
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
                BE.Shoot(projectile, gunPointOne.transform.position, projectile.transform.rotation);
                BE.Shoot(projectile, gunPointTwo.transform.position, projectile.transform.rotation);
                BE.Shoot(projectile, gunPointThree.transform.position, projectile.transform.rotation);
                BE.Shoot(projectile, gunPointFour.transform.position, projectile.transform.rotation);
                foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
                {
                    if (ps.CompareTag("Muzzle_FX"))
                        ps.Play();
                }
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(2.0f);

            //phase 2
            GetComponent<Rigidbody>().velocity = transform.up * speed;

            for(int i = 0; i < 16; i++)
            {
                BE.Shoot(projectileTwo, gunPointFive.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointSix.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointSeven.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointEight.transform.position, projectileTwo.transform.rotation);
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
            BE.ShootLaser(laser, lasPointOne.transform.position, laser.transform.rotation);
            BE.ShootLaser(laser, lasPointTwo.transform.position, laser.transform.rotation);
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
        StartCoroutine(destroyBoss());
        notDead = false;
        stopLaser = true;
        gameController.ModifyScore(BE.getScoreValue());
        //GetComponent<BoxCollider>().enabled = false;

    }

    IEnumerator destroyBoss()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(1.0f);
        while (transform.childCount > 0)
        {
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

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
            BE.ShootLaser(reflectLaser, gunPointSix.transform.position, laser.transform.rotation, angles[i * 2]);
            BE.ShootLaser(reflectLaser, gunPointSeven.transform.position, laser.transform.rotation, angles[i * 2 + 1]);
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
}
