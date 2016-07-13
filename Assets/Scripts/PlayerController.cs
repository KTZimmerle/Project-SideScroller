using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour {

	public float speed = 5.0f;
	public Boundary boundary;
	public GameObject projectile;
	public float cooldownRate = 1.0f;
	float firerate;
	public int PROJECTILE_LIMIT = 2;
    PowerUpSystem powers;
    PowerSelector selector;
    Rect gameBounds;

	void Awake () {
        firerate = cooldownRate;
        powers = GetComponent<PowerUpSystem>();
        selector = GetComponent<PowerSelector>();
    }

    void Start()
    {
        Vector3 bottomleft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 topright = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));
        gameBounds = new Rect(bottomleft.x, bottomleft.y, topright.x - bottomleft.x, topright.y - bottomleft.y);
    }

	// Update is called once per frame
	void FixedUpdate () {

        //powers = GetComponent<PowerUpSystem>();
        Vector3 DirResultant = Vector3.zero;
		firerate -= Time.deltaTime;
        powers.missileRate -= Time.deltaTime;
        //Debug.Log (cooldown);

        //Check for Vertical Movement
        /*if (Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) 
		{
			DirResultant.y = Time.deltaTime;	
		}
		else if (Input.GetKey (KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) 
		{
			DirResultant.y = -Time.deltaTime;	
		}
        
		//Check for Horizontal Movement
		if (Input.GetKey (KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) 
		{
			DirResultant.x = Time.deltaTime;	
		}
		else if (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) 
		{
			DirResultant.x = -Time.deltaTime;		
		}*/

        //Check for Vertical Movement & for Horizontal Movement
        DirResultant.y = Input.GetAxis("PlayerShipV") * Time.deltaTime;
        DirResultant.x = Input.GetAxis("PlayerShipH") * Time.deltaTime;

        if (Input.GetKey (KeyCode.Keypad0) || Input.GetKey (KeyCode.Space)) 
		{
			Shoot();
        }

        if (Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.C))
        {
            powers.FireMissiles();
        }

        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Z))
        {
            selector.selectPower(powers);
        }
        //Combine both horizontal and vertical to create movement!
        GetComponent<Rigidbody>().velocity = speed*DirResultant;
        Collider range = GetComponent<Collider>();
		GetComponent<Rigidbody>().position = new Vector3 
			(Mathf.Clamp(GetComponent<Rigidbody>().position.x, gameBounds.xMin + range.bounds.size.x / 2, gameBounds.xMax - range.bounds.size.x / 2),
			 Mathf.Clamp(GetComponent<Rigidbody>().position.y, gameBounds.yMin + range.bounds.size.y * 4.5f, gameBounds.yMax - range.bounds.size.y * 3.5f), 
			 0.0f);
	}

	void Shoot()
	{
		if (firerate < 0.0f && 
		    GameObject.FindGameObjectsWithTag("PlayerProjectile").Length + 1
		    <= PROJECTILE_LIMIT) 
		{
			GameObject clone;
			clone = Instantiate (projectile, transform.position, transform.rotation) as GameObject;
            clone.GetComponent<ProjectileBehavior>().isFriendly = true;
			firerate = cooldownRate;
		}

	}
}
