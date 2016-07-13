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
    public GameObject gunPointOne;
    public GameObject gunPointTwo;
    public GameObject gunPointThree;
    public GameObject gunPointFour;
    GameController gameController;

    private IEnumerator BossAttacks;

    // Use this for initialization
    void Awake ()
    {
        BossAttacks = BossRoutine();
        notDead = true;
        speed = 1.0f;
        BE = new BossShip();
        GameObject target = GameObject.FindWithTag("GameController");
        if (target.GetComponent<GameController>() != null)
            gameController = target.GetComponent<GameController>();
        StartCoroutine(BossAttacks);
    }
    
    IEnumerator BossRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        while (notDead)
        {
            //phase 1
            yield return new WaitForSeconds(4.0f);
            for (int i = 0; i < 50; i++)
            {
                BE.Shoot(projectile, gunPointOne.transform.position, projectile.transform.rotation);
                BE.Shoot(projectile, gunPointTwo.transform.position, projectile.transform.rotation);
                BE.Shoot(projectile, gunPointThree.transform.position, projectile.transform.rotation);
                BE.Shoot(projectile, gunPointFour.transform.position, projectile.transform.rotation);
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(2.0f);

            //phase 2
            GetComponent<Rigidbody>().velocity = transform.up * speed;
            for (int i = 0; i < 2; i++)
            {
                BE.Shoot(projectileTwo, gunPointOne.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointTwo.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointThree.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointFour.transform.position, projectileTwo.transform.rotation);
                yield return new WaitForSeconds(1.0f);
            }
            if (this.transform.position.y < 1.8f)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            GetComponent<Rigidbody>().velocity = -transform.up * speed;
            for (int i = 0; i < 4; i++)
            {
                BE.Shoot(projectileTwo, gunPointOne.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointTwo.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointThree.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointFour.transform.position, projectileTwo.transform.rotation);
                yield return new WaitForSeconds(1.0f);
            }
            if (this.transform.position.y > -1.8f)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            GetComponent<Rigidbody>().velocity = transform.up * speed;
            for (int i = 0; i < 4; i++)
            {
                BE.Shoot(projectileTwo, gunPointOne.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointTwo.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointThree.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointFour.transform.position, projectileTwo.transform.rotation);
                yield return new WaitForSeconds(1.0f);
            }
            if (this.transform.position.y < 1.8f)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            GetComponent<Rigidbody>().velocity = -transform.up * speed;
            for (int i = 0; i < 4; i++)
            {
                BE.Shoot(projectileTwo, gunPointOne.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointTwo.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointThree.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointFour.transform.position, projectileTwo.transform.rotation);
                yield return new WaitForSeconds(1.0f);
            }
            if (this.transform.position.y > -1.8f)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            GetComponent<Rigidbody>().velocity = transform.up * speed;
            for (int i = 0; i < 2; i++)
            {
                BE.Shoot(projectileTwo, gunPointOne.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointTwo.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointThree.transform.position, projectileTwo.transform.rotation);
                BE.Shoot(projectileTwo, gunPointFour.transform.position, projectileTwo.transform.rotation);
                yield return new WaitForSeconds(1.0f);
            }
            if (this.transform.position.y < 0.0f)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().position = new Vector3(GetComponent<Rigidbody>().position.x, 0.0f, 0.0f);

            //phase 3
            yield return new WaitForSeconds(2.0f);
            BE.ShootLaser(laser, gunPointTwo.transform.position, laser.transform.rotation);
            BE.ShootLaser(laser, gunPointThree.transform.position, laser.transform.rotation);
            //yield return new WaitForSeconds(10.0f);

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
}
