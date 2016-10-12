using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipPool : MonoBehaviour {

    public GameObject razer;
    public GameObject swooper;
    public GameObject blaster;
    public GameObject hunter;
    public GameObject powership;
    public GameObject powerup;
    public GameObject battleship;
    public GameObject battleshipHM;
    public GameObject starfighterP;
    /*public GameObject razerpool;
    public GameObject swooperpool;
    public GameObject blasterpool;
    public GameObject hunterpool;
    public GameObject powershippool;
    public GameObject poweruppool;*/

    const int MAX_HAZARD_SIZE = 25;
    const int MAX_POWERSHIP_SIZE = 6;
    const int MAX_SQUAD_SIZE = 16;
    const int MAX_PLATFORM_SIZE = 25;
    const int MAX_HUNTER_SIZE = 5;
    List<GameObject> razers;
    List<GameObject> swoopers;
    List<GameObject> blasters;
    List<GameObject> hunters;
    List<GameObject> powerships;
    List<GameObject> powerups;
    GameObject bShip;
    GameObject bShipHM;
    GameObject starFP;

    void Awake ()
    {
        razers = new List<GameObject>();
        swoopers = new List<GameObject>();
        blasters = new List<GameObject>();
        hunters = new List<GameObject>();
        powerships = new List<GameObject>();
        powerups = new List<GameObject>();
        bShip = Instantiate(battleship);
        bShipHM = Instantiate(battleshipHM);
        starFP = Instantiate(starfighterP);

        for (int i = 0; i < MAX_HAZARD_SIZE; i++)
        {
            razers.Add(Instantiate(razer));
            //razers[i].transform.SetParent(razerpool.transform, false);

            if(powerships.Count < MAX_POWERSHIP_SIZE)
            {
                powerships.Add(Instantiate(powership));
                powerups.Add(Instantiate(powerup));
                //powerships[i].transform.SetParent(powershippool.transform, false);
                //powerups[i].transform.SetParent(poweruppool.transform, false);
            }

            if (swoopers.Count < MAX_SQUAD_SIZE)
            {
                swoopers.Add(Instantiate(swooper));
                //swoopers[i].transform.SetParent(swooperpool.transform, false);
            }

            if (blasters.Count < MAX_PLATFORM_SIZE)
            {
                blasters.Add(Instantiate(blaster));
                //blasters[i].transform.SetParent(blasterpool.transform, false);
            }

            if (hunters.Count < MAX_HUNTER_SIZE)
            {
                hunters.Add(Instantiate(hunter));
                //hunters[i].transform.SetParent(hunterpool.transform, false);
            }
        }
	}
    
    /*public void addShip(GameObject b)
    {
        if (b.GetComponent<StraightMover>() != null)
            razers.Add(b);
        else if (b.GetComponent<CircularMover>() != null)
            swoopers.Add(b);
        else if (b.GetComponent<RotatorMover>() != null)
            blasters.Add(b);
        else if (b.GetComponent<TrackingMover>() != null)
            hunters.Add(b);
        else if (b.GetComponent<WavyMover>() != null)
            powerships.Add(b);
        else if (b.CompareTag("PickUp"))
            powerups.Add(b);
    }

    public void removeShip(GameObject b)
    {
        if (b.GetComponent<StraightMover>() != null)
            razers.Remove(b);
        else if (b.GetComponent<CircularMover>() != null)
            swoopers.Remove(b);
        else if (b.GetComponent<RotatorMover>() != null)
            blasters.Remove(b);
        else if (b.GetComponent<TrackingMover>() != null)
            hunters.Remove(b);
        else if (b.GetComponent<WavyMover>() != null)
            powerships.Remove(b);
        else if (b.CompareTag("PickUp"))
            powerups.Remove(b);
    }*/

    public int getMaxRazers()
    {
        return razers.Count;
    }

    public int getMaxSwoopers()
    {
        return swoopers.Count;
    }

    public int getMaxBlasters()
    {
        return blasters.Count;
    }

    public int getMaxHunters()
    {
        return hunters.Count;
    }

    public int getMaxPowerShips()
    {
        return powerships.Count;
    }

    public GameObject SpawnRazer()
    {
        for (int i = 0; i < getMaxRazers(); i++)
        {
            if (!razers[i].activeSelf)
                return razers[i];
        }
        return null;
    }

    public GameObject SpawnSwooper()
    {
        for (int i = 0; i < getMaxSwoopers(); i++)
        {
            if (!swoopers[i].activeSelf)
                return swoopers[i];
        }
        return null;
    }

    public GameObject SpawnBlaster()
    {
        for (int i = 0; i < getMaxBlasters(); i++)
        {
            if (!blasters[i].activeSelf)
                return blasters[i];
        }
        return null;
    }

    public GameObject SpawnHunter()
    {
        for (int i = 0; i < getMaxHunters(); i++)
        {
            if (!hunters[i].activeSelf)
                return hunters[i];
        }
        return null;
    }

    public GameObject SpawnPowerShip()
    {
        for (int i = 0; i < getMaxPowerShips(); i++)
        {
            if (!powerships[i].activeSelf)
                return powerships[i];
        }
        return null;
    }

    public GameObject SpawnPowerUp()
    {
        for (int i = 0; i < getMaxPowerShips(); i++)
        {
            if (!powerups[i].activeSelf)
                return powerups[i];
        }
        return null;
    }

    public GameObject SpawnBattleShip()
    {
        if (!bShip.activeSelf)
            return bShip;
        return null;
    }

    public GameObject SpawnBattleShipHM()
    {
        if (!bShipHM.activeSelf)
            return bShipHM;
        return null;
    }

    public GameObject SpawnStarFighter()
    {
        if (!starFP.activeSelf)
            return starFP;
        return null;
    }
}
