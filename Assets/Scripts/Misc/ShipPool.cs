using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipPool : MonoBehaviour {

    int hunterTracker = 0;
    public GameObject razer;
    public GameObject swooper;
    public GameObject blaster;
    public GameObject hunter;
    public GameObject powership;
    public GameObject powerup;
    public GameObject missilepowerup;
    public GameObject speedpowerup;
    public GameObject crossfirepowerup;
    public GameObject laserpowerup;
    public GameObject shieldpowerup;
    public GameObject orbiterPickup;
    public GameObject battleship;
    public GameObject battleshipHM;
    public GameObject AIDrones;
    //public GameObject AIBossDrones;
    public GameObject starfighterP;
    public GameObject orbiterOne;
    public GameObject orbiterTwo;

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
    List<GameObject> missilepowerups;
    List<GameObject> speedpowerups;
    List<GameObject> crossfirepowerups;
    List<GameObject> laserpowerups;
    List<GameObject> shieldpowerups;
    List<GameObject> orbiters;
    List<GameObject> orbiterspickups;
    //List<GameObject> AIDrones;
    List<float> xPositions;
    List<float> yPositions;
    GameObject bShip;
    GameObject bShipHM;
    GameObject AIShips;
    //GameObject AIShips;
    GameObject starFP;

    void Awake ()
    {
        /*xPositions = new List<float>(new float[]{ -2.5f, 2.5f, 0.0f, -4.5f, 4.5f, -4.0f, 4.0f});
        yPositions = new List<float>(new float[] { 4.0f, 4.0f, -4.5f, 1.0f, 1.0f, -2.75f, -2.75f });*/
        //AIDrones = new List<GameObject>();
        razers = new List<GameObject>();
        swoopers = new List<GameObject>();
        blasters = new List<GameObject>();
        hunters = new List<GameObject>();
        powerships = new List<GameObject>();
        powerups = new List<GameObject>();
        orbiters = new List<GameObject>();
        orbiterspickups = new List<GameObject>();
        speedpowerups = new List<GameObject>();
        missilepowerups = new List<GameObject>();
        crossfirepowerups = new List<GameObject>();
        laserpowerups = new List<GameObject>();
        shieldpowerups = new List<GameObject>();

        bShip = Instantiate(battleship);
        bShipHM = Instantiate(battleshipHM);
        AIShips = Instantiate(AIDrones);
        starFP = Instantiate(starfighterP);
        orbiters.Add(Instantiate(orbiterOne));
        orbiters.Add(Instantiate(orbiterTwo));
        orbiters[0].GetComponent<Orbiter>().SetStartPosition();
        orbiters[1].GetComponent<Orbiter>().SetStartPosition(0.0f, -1.0f);


        for (int i = 0; i < MAX_HAZARD_SIZE; i++)
        {
            razers.Add(Instantiate(razer));

            /*if (AIDrones.Count < 7)
            {
                AIDrones.Add(Instantiate(AIBossDrones));
            }*/
            //razers[i].transform.SetParent(razerpool.transform, false);

            if(powerships.Count < MAX_POWERSHIP_SIZE)
            {
                powerships.Add(Instantiate(powership));
                orbiterspickups.Add(Instantiate(orbiterPickup));
                missilepowerups.Add(Instantiate(missilepowerup));
                speedpowerups.Add(Instantiate(speedpowerup));
                crossfirepowerups.Add(Instantiate(crossfirepowerup));
                laserpowerups.Add(Instantiate(laserpowerup));
                shieldpowerups.Add(Instantiate(shieldpowerup));
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

    private void Start()
    {
        orbiters[0].GetComponent<Orbiter>().SetOppositeOrb(orbiters[1]);
        orbiters[1].GetComponent<Orbiter>().SetOppositeOrb(orbiters[0]);
        starFP.GetComponent<PlayerController>().SetOrbiterOneRef(orbiters[0]);
        starFP.GetComponent<PlayerController>().SetOrbiterTwoRef(orbiters[1]);
        starFP.GetComponent<PowerUpSystem>().SetOrbiterOneRef(orbiters[0]);
        starFP.GetComponent<PowerUpSystem>().SetOrbiterTwoRef(orbiters[1]);
    }


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
        int index = hunterTracker % getMaxHunters();

        if (!hunters[index].activeSelf)
        {
            hunterTracker++;
            return hunters[index];
        }
        else
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

    public GameObject SpawnRandomPowerUp()
    {
        int rand = Random.Range(1, 61);
        if (rand >= 1 && rand < 10)
            return SpawnSpeedPowerUp();
        else if (rand >= 10 && rand < 20)
            return SpawnMissilePowerUp();
        else if (rand >= 20 && rand < 30)
            return SpawnCrossFirePowerUp();
        else if (rand >= 30 && rand < 40)
            return SpawnLaserPowerUp();
        else if (rand >= 40 && rand < 50)
            return SpawnShieldPowerUp();
        else if (rand >= 50 && rand <= 60)
            return SpawnOrbiterPowerUp();
        else
            return null;
    }

    public GameObject SpawnOrbiterPowerUp()
    {
        for (int i = 0; i < getMaxPowerShips(); i++)
        {
            if (!orbiterspickups[i].activeSelf)
                return orbiterspickups[i];
        }
        return null;
    }

    public GameObject SpawnSpeedPowerUp()
    {
        for (int i = 0; i < getMaxPowerShips(); i++)
        {
            if (!speedpowerups[i].activeSelf)
                return speedpowerups[i];
        }
        return null;
    }

    public GameObject SpawnMissilePowerUp()
    {
        for (int i = 0; i < getMaxPowerShips(); i++)
        {
            if (!missilepowerups[i].activeSelf)
                return missilepowerups[i];
        }
        return null;
    }

    public GameObject SpawnCrossFirePowerUp()
    {
        for (int i = 0; i < getMaxPowerShips(); i++)
        {
            if (!crossfirepowerups[i].activeSelf)
                return crossfirepowerups[i];
        }
        return null;
    }

    public GameObject SpawnLaserPowerUp()
    {
        for (int i = 0; i < getMaxPowerShips(); i++)
        {
            if (!laserpowerups[i].activeSelf)
                return laserpowerups[i];
        }
        return null;
    }

    public GameObject SpawnShieldPowerUp()
    {
        for (int i = 0; i < getMaxPowerShips(); i++)
        {
            if (!shieldpowerups[i].activeSelf)
                return shieldpowerups[i];
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

    public GameObject SpawnAIDrones()
    {
        if (!AIShips.activeSelf)
            return AIShips;
        return null;
    }

    public GameObject SpawnStarFighter()
    {
        if (!starFP.activeSelf)
            return starFP;
        return null;
    }

    public GameObject SpawnOrbiter()
    {
        if (!orbiters[0].activeSelf)
            return orbiters[0];
        if (!orbiters[1].activeSelf)
            return orbiters[1];
        return null;
    }
    
}
