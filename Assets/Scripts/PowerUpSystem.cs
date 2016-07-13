using UnityEngine;
using System.Collections;

public class PowerUpSystem : MonoBehaviour {

    public bool missilePowUp;
    public bool laserPowUp;
    public float missileRate;
    public int MISSILE_LIMIT = 1;
    public float laserRate;
    float speedModifier;

    public GameObject missile;

    //missiles
    public bool hasMissilePowerUp()
    {
        return missilePowUp;
    }

    public void FireMissiles()
    {
        if (missileRate < 0.0f && missilePowUp && GameObject.FindGameObjectsWithTag("MissileProjectile").Length + 1
            <= MISSILE_LIMIT)
        {
            GameObject clone;
            clone = Instantiate(missile, transform.position, transform.rotation) as GameObject;
            clone.GetComponent<ProjectileBehavior>().isFriendly = true;
            missileRate = 0.5f;
        }

    }

    //lasers
    //alternate attacks
    //super bomb
    //speed up
    //shields

	// Use this for initialization
	void Awake ()
    {
        speedModifier = 1.0f;
        missilePowUp = false;
        missileRate = 0.5f;
	}
}
