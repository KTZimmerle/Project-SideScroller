using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOrbiter : MonoBehaviour {

    ShipPool sp;

    private void Start()
    {
        sp = GameObject.FindGameObjectWithTag("ShipPool").GetComponent<ShipPool>();
    }

    public void RequestOrbiter()
    {
        GameObject orb = sp.SpawnOrbiter();
        if(orb != null)
        {
            orb.SetActive(true);
        }
    }
}
