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
    public int MISSILE_LIMIT = 2;
    PowerUpSystem powers;
    PowerSelector selector;

	void Start () {
        firerate = cooldownRate;
        powers = GetComponent<PowerUpSystem>();
        selector = GetComponent<PowerSelector>();
	}

	// Update is called once per frame
	void FixedUpdate () {

        //powers = GetComponent<PowerUpSystem>();
        Vector3 DirResultant = Vector3.zero;
		firerate -= Time.deltaTime;
        powers.missileRate -= Time.deltaTime;
		//Debug.Log (cooldown);

		//Check for Vertical Movement
		if (Input.GetKey (KeyCode.W)) 
		{
			DirResultant.y = Time.deltaTime;	
		}
		else if (Input.GetKey (KeyCode.S)) 
		{
			DirResultant.y = -Time.deltaTime;	
		}

		//Check for Horizontal Movement
		if (Input.GetKey (KeyCode.D)) 
		{
			DirResultant.x = Time.deltaTime;	
		}
		else if (Input.GetKey (KeyCode.A)) 
		{
			DirResultant.x = -Time.deltaTime;		
		}

		if (Input.GetKey (KeyCode.Mouse0) || Input.GetKey (KeyCode.Space)) 
		{
			Shoot();
        }

        if (Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.C))
        {
            powers.FireMissiles();
        }

        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Z))
        {
            //select power up
        }
        //Combine both horizontal and vertical to create movement!
        GetComponent<Rigidbody>().velocity = speed*DirResultant;

		GetComponent<Rigidbody>().position = new Vector3 
			(Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
			 Mathf.Clamp(GetComponent<Rigidbody>().position.y, boundary.yMin, boundary.yMax), 
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
			firerate = cooldownRate;
		}

	}
}
