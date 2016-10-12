using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public bool revived = false;
	public float speed;
	public GameObject projectile;
	public float cooldownRate = 1.0f;
	float firerate;
	const int PROJECTILE_LIMIT = 2;
    PowerUpSystem powers;
    PowerSelector selector;
    Rect gameBounds;
    float flickerTime;

	void Awake () {
        powers = GetComponent<PowerUpSystem>();
        selector = GetComponent<PowerSelector>();
        Camera c = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //Vector3 bottomleft = new Vector3(-10.0f, -4.5f, 0.0f);
        //Vector3 topright = new Vector3(10.0f, 4.5f, 0.0f);
        Vector3 bottomleft = c.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, c.nearClipPlane + 2.0f));
        Vector3 topright = c.ScreenToWorldPoint(new Vector3(c.pixelWidth, c.pixelHeight, c.nearClipPlane + 2.0f));
        gameBounds = new Rect(bottomleft.x, bottomleft.y, topright.x * 2, topright.y * 2);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        speed = 250.0f;
        flickerTime = 4.0f;
        firerate = cooldownRate;
        if (revived)
            StartCoroutine(flicker());
    }
    
	void FixedUpdate () {

        //powers = GetComponent<PowerUpSystem>();
        Vector3 DirResultant = Vector3.zero;

        //Check for Vertical Movement & for Horizontal Movement

        DirResultant.y = Input.GetAxis("PlayerShipV") * Time.deltaTime;
        DirResultant.x = Input.GetAxis("PlayerShipH") * Time.deltaTime;

        if (transform.position.x + DirResultant.x < gameBounds.xMin ||
           transform.position.x + DirResultant.x > gameBounds.xMax)
        {
            DirResultant.x = 0;
            transform.position = (transform.position.x > 0.0f) ? 
                                 new Vector3(gameBounds.xMax, transform.position.y, 0.0f) : 
                                 new Vector3(gameBounds.xMin, transform.position.y, 0.0f);
        }

        if (transform.position.y + DirResultant.y < gameBounds.yMin ||
           transform.position.y + DirResultant.y > gameBounds.yMax)
        {
            DirResultant.y = 0;
            transform.position = (transform.position.y > 0.0f) ?
                                 new Vector3(transform.position.x, gameBounds.yMax, 0.0f) :
                                 new Vector3(transform.position.x, gameBounds.yMin, 0.0f);
        }

        //Combine both horizontal and vertical to create movement!
        GetComponent<Rigidbody>().velocity = speed*DirResultant;
        Collider range = GetComponent<Collider>();
	}

    void Update()
    {
        if(firerate > 0.0f)
            firerate -= Time.deltaTime;

        if(powers.missileRate > 0.0f)
            powers.missileRate -= Time.deltaTime;

        if(powers.altfireRate > 0.0f)
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
            powers.AltShoot();
            powers.LaserShoot();
        }

        if (Input.GetKey(KeyCode.KeypadPeriod) || Input.GetKey(KeyCode.X))
        {
            powers.FireMissiles();
        }

        if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.C))
        {
            selector.selectPower(powers);
        }
    }

	void Shoot()
	{
        int numBullets = GameObject.FindGameObjectWithTag("GameController").GetComponent<ProjectilePool>().getBullets();
        GameObject proj = GameObject.FindGameObjectWithTag("GameController").GetComponent<ProjectilePool>().RequestBullet();
        if (firerate < 0.0f && proj != null && !powers.laserPowUp)
        {
            foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
            {
                if (ps.CompareTag("Muzzle_FX"))
                    ps.Play();
            }
            //GameObject clone;
            Vector3 offsetX = transform.position;
            offsetX.x += 0.75f;
            proj.GetComponent<ProjectileBehavior>().isFriendly = true;
            proj.transform.position = offsetX;
            proj.gameObject.SetActive(true);
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
}
