using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public bool revived = false;
	public float speed;
	public GameObject projectile;
	public float cooldownRate = 1.0f;
	float firerate;
	public int PROJECTILE_LIMIT = 3;
    PowerUpSystem powers;
    PowerSelector selector;
    Rect gameBounds;
    float flickerTime;

	void Awake () {
        flickerTime = 4.0f;
        firerate = cooldownRate;
        powers = GetComponent<PowerUpSystem>();
        selector = GetComponent<PowerSelector>();
    }

    void Start()
    {
        if(revived)
            StartCoroutine(flicker());
        Vector3 bottomleft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 topright = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));
        gameBounds = new Rect(bottomleft.x, bottomleft.y, topright.x - bottomleft.x, topright.y - bottomleft.y);
    }
    
	void FixedUpdate () {

        //powers = GetComponent<PowerUpSystem>();
        Vector3 DirResultant = Vector3.zero;

        //Check for Vertical Movement & for Horizontal Movement
        DirResultant.y = Input.GetAxis("PlayerShipV") * Time.deltaTime;
        DirResultant.x = Input.GetAxis("PlayerShipH") * Time.deltaTime;

        //Combine both horizontal and vertical to create movement!
        GetComponent<Rigidbody>().velocity = speed*DirResultant;
        Collider range = GetComponent<Collider>();
		GetComponent<Rigidbody>().position = new Vector3 
			(Mathf.Clamp(GetComponent<Rigidbody>().position.x, gameBounds.xMin + range.bounds.size.x / 2, gameBounds.xMax - range.bounds.size.x / 2),
			 Mathf.Clamp(GetComponent<Rigidbody>().position.y, gameBounds.yMin + range.bounds.size.y * 4.5f, gameBounds.yMax - range.bounds.size.y * 3.5f), 
			 0.0f);
	}

    void Update()
    {
        firerate -= Time.deltaTime;
        powers.missileRate -= Time.deltaTime;
        powers.altfireRate -= Time.deltaTime;
        powers.laserRate -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Keypad0) || Input.GetKey(KeyCode.V))
        {
            Shoot();
            powers.AltShoot();
            powers.LaserShoot();
        }

        if (Input.GetKey(KeyCode.KeypadPeriod) || Input.GetKey(KeyCode.C))
        {
            powers.FireMissiles();
        }

        if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Z))
        {
            selector.selectPower(powers);
        }
    }

	void Shoot()
	{
		if (firerate < 0.0f && 
		    GameObject.FindGameObjectsWithTag("PlayerProjectile").Length + 1
		    <= PROJECTILE_LIMIT && !powers.laserPowUp) 
		{
			GameObject clone;
			clone = Instantiate (projectile, transform.position, transform.rotation) as GameObject;
            clone.GetComponent<ProjectileBehavior>().isFriendly = true;
			firerate = cooldownRate;
		}
	}

    IEnumerator flicker()
    {
        for (int ticks = 0; ticks < 80; ticks++)
        {
            GetComponent<MeshRenderer>().enabled = !GetComponent<MeshRenderer>().enabled;
            yield return new WaitForSeconds(0.05f);
        }
        GetComponent<MeshRenderer>().enabled = true;
    }
}
