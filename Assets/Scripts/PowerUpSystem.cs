using UnityEngine;
using System.Collections;

public class PowerUpSystem : MonoBehaviour {

    public bool missilePowUp;
    public bool laserPowUp;
    public float missileRate;
    public float laserRate;

    public GameObject missile;

    //missiles
    public bool hasMissilePowerUp()
    {
        return missilePowUp;
    }

    public void FireMissiles()
    {
        if (missileRate < 0.0f && missilePowUp)
        {
            GameObject clone;
            clone = Instantiate(missile, transform.position, transform.rotation) as GameObject;
            missileRate = 1.5f;
        }

    }

    //lasers
    //alternate attacks
    //super bomb
    //speed up
    //shields

	// Use this for initialization
	void Start () {
        missilePowUp = false;
        missileRate = 1.5f;
	}
}
