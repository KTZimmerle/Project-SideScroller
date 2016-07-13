using UnityEngine;
using System.Collections;

public class PowerDrop : MonoBehaviour {

    public GameObject PowerUpDrop;

    void OnDestroy()
    {
        GameObject clone;
        clone = Instantiate(PowerUpDrop, transform.position, transform.rotation) as GameObject;
    }
}
