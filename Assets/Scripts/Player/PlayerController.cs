using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    public bool revived = false;
	public float speed;
	public float cooldownRate = 1.0f;
	float firerate;
    PowerUpSystem powers;
    PowerSelector selector;
    Rect gameBounds;
    float flickerTime;
    PauseGame pause;
    AudioClip projectile_SFX;
    GameObject orbiterOne;
    GameObject orbiterTwo;
    Vector3 DirResultant;
    ProjectilePool projPoolMain;
    ProjectilePool projPoolOne;
    ProjectilePool projPoolTwo;

    void Awake () {
        pause = GetComponent<PauseGame>();
        powers = GetComponent<PowerUpSystem>();
        selector = GetComponent<PowerSelector>();
        projPoolMain = GameObject.FindGameObjectWithTag("ProjectilePool").GetComponent<ProjectilePool>();
        projPoolOne = GameObject.FindGameObjectWithTag("OrbiterProjPoolOne").GetComponent<ProjectilePool>();
        projPoolTwo = GameObject.FindGameObjectWithTag("OrbiterProjPoolTwo").GetComponent<ProjectilePool>();
        //Vector3 bottomleft = new Vector3(-10.0f, -4.5f, 0.0f);
        //Vector3 topright = new Vector3(10.0f, 4.5f, 0.0f);
        gameObject.SetActive(false);
    }

    void Start()
    {
        Camera c = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector3 bottomleft = c.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, c.nearClipPlane + 2.0f));
        Vector3 topright = c.ScreenToWorldPoint(new Vector3(c.pixelWidth, c.pixelHeight, c.nearClipPlane + 2.0f));
        gameBounds = new Rect(bottomleft.x, bottomleft.y, topright.x * 2, topright.y * 2);
        DirResultant = Vector3.zero;
        /*shooters[0] = this.gameObject;
        shooters[1] = orbiterOne;
        shooters[2] = orbiterTwo;*/
    }

    void OnEnable()
    {
        speed = 3.0f;
        flickerTime = 4.0f;
        firerate = cooldownRate;
        if (revived)
            StartCoroutine(flicker());
    }

    private void OnDisable()
    {
        GameObject orbSpawn = null;
        if (orbiterOne != null && orbiterOne.activeSelf)
        {
            orbSpawn = GameObject.FindWithTag("ShipPool").GetComponent<ShipPool>().SpawnOrbiterPowerUp();
            orbSpawn.transform.position = orbiterOne.transform.position;
            orbSpawn.transform.rotation = orbiterOne.transform.rotation;
            orbSpawn.SetActive(true);
            orbiterOne.GetComponent<Orbiter>().Despawn();
        }

        if (orbiterTwo != null && orbiterTwo.activeSelf)
        {
            orbSpawn = GameObject.FindWithTag("ShipPool").GetComponent<ShipPool>().SpawnOrbiterPowerUp();
            orbSpawn.transform.position = orbiterTwo.transform.position;
            orbSpawn.transform.rotation = orbiterTwo.transform.rotation;
            orbSpawn.SetActive(true);
            orbiterTwo.GetComponent<Orbiter>().Despawn();
        }
    }

    void Update()
    {
        if (pause.isGamePaused())
            return;

        if(firerate > 0.0f)
            firerate -= Time.deltaTime;

        if(powers.missileRate > 0.0f)
            powers.missileRate -= Time.deltaTime;

        if (powers.missileRateTwo > 0.0f)
            powers.missileRateTwo -= Time.deltaTime;

        if (powers.missileRateThree > 0.0f)
            powers.missileRateThree -= Time.deltaTime;

        if (powers.altfireRate > 0.0f)
            powers.altfireRate -= Time.deltaTime;

        if(powers.laserRate > 0.0f)
            powers.laserRate -= Time.deltaTime;

        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
            if (ps.CompareTag("Engine_FX"))
            {
                if (GetComponent<Rigidbody>().velocity.x < 0.0f)
                    ps.transform.localScale = new Vector3(0.5f, 1.0f, 1.0f);
                else if (GetComponent<Rigidbody>().velocity.x > 0.0f)
                    ps.transform.localScale = new Vector3(2.5f, 1.0f, 1.0f);
                else
                    ps.transform.localScale = new Vector3(1.5f, 1.0f, 1.0f);
            }

        //The ship turns side ways based on up and down directions - KTZ
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(30.0f, 0.0f, 0.0f), 0.1f);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-30.0f, 0.0f, 0.0f), 0.1f);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, 0.0f), 0.1f);
        }

        if (Input.GetKey(KeyCode.Keypad0) || Input.GetKey(KeyCode.Z))
        {
            Shoot();
            powers.FireMissiles();
            powers.AltShoot();
            powers.LaserShoot();
        }

        //PlayerMovement();

        /*if (Input.GetKey(KeyCode.KeypadPeriod) || Input.GetKey(KeyCode.X))
        {
        }*/

        /*if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.C))
        {
            selector.selectPower(powers);
        }*/
        PlayerMovement();
    }

	public void Shoot()
	{
        //int numBullets = GameObject.FindGameObjectWithTag("ProjectilePool").GetComponent<ProjectilePool>().getBullets();
        GameObject projOne = projPoolMain.RequestBullet();
        GameObject projTwo = null;
        GameObject projThree = null;

        if (orbiterOne != null && orbiterOne.activeSelf)
            projTwo = projPoolOne.RequestBullet();

        if (orbiterTwo != null && orbiterTwo.activeSelf)
            projThree = projPoolTwo.RequestBullet();
        
        if (firerate < 0.0f && projOne != null && !powers.laserPowUp)
        {
            foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
            {
                if (ps.CompareTag("Muzzle_FX"))
                    ps.Play();
            }

            //GameObject clone;
            Vector3 offsetX = transform.position;
            offsetX.x += 0.75f;
            projOne.GetComponent<ProjectileBehavior>().isFriendly = true;
            projOne.transform.position = offsetX;
            projOne.gameObject.SetActive(true);

            if (orbiterOne != null && orbiterOne.activeSelf && projTwo != null)
            {
                projTwo.GetComponent<ProjectileBehavior>().isFriendly = true;
                projTwo.transform.position = orbiterOne.transform.position;
                projTwo.gameObject.SetActive(true);
            }

            if (orbiterTwo != null && orbiterTwo.activeSelf && projThree != null)
            {
                projThree.GetComponent<ProjectileBehavior>().isFriendly = true;
                projThree.transform.position = orbiterTwo.transform.position;
                projThree.gameObject.SetActive(true);
            }

            //clone = Instantiate (projectile, offsetX, transform.rotation) as GameObject;
            firerate = cooldownRate;
		}
	}

    IEnumerator flicker()
    {
        for (int ticks = 0; ticks < 80; ticks++)
        {
            GetComponent<MeshRenderer>().enabled = !GetComponent<MeshRenderer>().enabled;
            foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
            {
                ps.Stop();
            }
            yield return new WaitForSeconds(0.05f);
            foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
            {
                ps.Play();
            }
        }
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play();
        }
        GetComponent<MeshRenderer>().enabled = true;
    }

    public void SetOrbiterOneRef(GameObject orbOne)
    {
        orbiterOne = orbOne;
    }

    public void SetOrbiterTwoRef(GameObject orbTwo)
    {
        orbiterTwo = orbTwo;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public Rect GetGameBOunds()
    {
        return gameBounds;
    }

    public void PlayerMovement()
    {
        DirResultant = Vector3.zero;
        DirResultant.y = Input.GetAxisRaw("PlayerShipV") * Time.deltaTime;
        DirResultant.x = Input.GetAxisRaw("PlayerShipH") * Time.deltaTime;

        if (transform.position.x + DirResultant.x / (Time.deltaTime * 4) < gameBounds.xMin ||
           transform.position.x + DirResultant.x / (Time.deltaTime * 4) > gameBounds.xMax)
        {
            DirResultant.x = 0;
        }

        if (transform.position.y + DirResultant.y / (Time.deltaTime * 2) < gameBounds.yMin ||
           transform.position.y + DirResultant.y / (Time.deltaTime * 2) > gameBounds.yMax)
        {
            DirResultant.y = 0;
        }

        //Combine both horizontal and vertical to create movement!
        transform.Translate(DirResultant * speed, Space.World);
    }
}
