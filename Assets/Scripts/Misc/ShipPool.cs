using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipPool : MonoBehaviour {

    public GameObject razer;
    public GameObject swooper;
    public GameObject blaster;
    public GameObject hunter;
    public GameObject powership;
    const int MAX_HAZARD_SIZE = 50;
    const int MAX_POWERSHIP_SIZE = 6;
    const int MAX_SQUAD_SIZE = 8;
    const int MAX_FORMATION_SIZE = 8;
    const int MAX_PLATFORM_SIZE = 25;
    const int MAX_HUNTER_SIZE = 5;
    List<GameObject> razers;
    List<GameObject> swoopers;
    List<GameObject> blasters;
    List<GameObject> hunters;
    List<GameObject> powerships;
    
    void Awake ()
    {
        razers = new List<GameObject>();
        swoopers = new List<GameObject>();
        blasters = new List<GameObject>();
        hunters = new List<GameObject>();
        powerships = new List<GameObject>();

        for (int i = 0; i < 50; i++)
        {
            razers.Add(Instantiate(razer));

            if(powerships.Count < MAX_POWERSHIP_SIZE)
                powerships.Add(Instantiate(powership));

            if (swoopers.Count < MAX_SQUAD_SIZE)
                swoopers.Add(Instantiate(swooper));

            if (blasters.Count < MAX_PLATFORM_SIZE)
                blasters.Add(Instantiate(blaster));

            if (hunters.Count < MAX_HUNTER_SIZE)
                hunters.Add(Instantiate(hunter));
        }
	}
}
