using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpecialFXPool : MonoBehaviour {

    public GameObject small_Explosion;
    public GameObject medium_Explosion;
    public GameObject large_Explosion;
    public GameObject missile_Explosion;
    public GameObject player_Explosion;
    const int SMALL_EXPLOSION_CAP = 20;
    const int MEDIUM_EXPLOSION_CAP = 25;
    const int LARGE_EXPLOSION_CAP = 10;
    const int MISSILE_EXPLOSION_CAP = 3;
    const int PLAYER_EXPLOSION_CAP = 1;
    List<GameObject> smallXPlosPool;
    List<GameObject> mediumXPlosPool;
    List<GameObject> largeXPlosPool;
    List<GameObject> missileXPlosPool;
    List<GameObject> playerXPlosPool;

    void Awake()
    {
        smallXPlosPool = new List<GameObject>();
        mediumXPlosPool = new List<GameObject>();
        largeXPlosPool = new List<GameObject>();
        missileXPlosPool = new List<GameObject>();
        playerXPlosPool = new List<GameObject>();

        for (int i = 0; i < MEDIUM_EXPLOSION_CAP; i++)
        {
            mediumXPlosPool.Add(Instantiate(medium_Explosion));

            if (smallXPlosPool.Count < SMALL_EXPLOSION_CAP)
            {
                smallXPlosPool.Add(Instantiate(small_Explosion));
            }
            
            if (missileXPlosPool.Count < MISSILE_EXPLOSION_CAP)
            {
                missileXPlosPool.Add(Instantiate(missile_Explosion));
            }

            if (playerXPlosPool.Count < PLAYER_EXPLOSION_CAP)
            {
                playerXPlosPool.Add(Instantiate(player_Explosion));
            }
            
            /*if (largeXPlosPool.Count < LARGE_EXPLOSION_CAP)
            {
                largeXPlosPool.Add(Instantiate(large_Explosion));
                //swoopers[i].transform.SetParent(swooperpool.transform, false);
            }*/
        }
    }

    public GameObject playSmallExplosion()
    {
        for (int i = 0; i < SMALL_EXPLOSION_CAP; i++)
        {
            if (!smallXPlosPool[i].activeSelf)
                return smallXPlosPool[i];
        }
        return null;
    }

    public GameObject playMediumExplosion()
    {
        for (int i = 0; i < MEDIUM_EXPLOSION_CAP; i++)
        {
            if (!mediumXPlosPool[i].activeSelf)
                return mediumXPlosPool[i];
        }
        return null;
    }

    public GameObject playLargeExplosion()
    {
        for (int i = 0; i < LARGE_EXPLOSION_CAP; i++)
        {
            if (!largeXPlosPool[i].activeSelf)
                return largeXPlosPool[i];
        }
        return null;
    }

    public GameObject playMissileExplosion()
    {
        for (int i = 0; i < MISSILE_EXPLOSION_CAP; i++)
        {
            if (!missileXPlosPool[i].activeSelf)
                return missileXPlosPool[i];
        }
        return null;
    }

    public GameObject playPlayerExplosion()
    {
        for (int i = 0; i < PLAYER_EXPLOSION_CAP; i++)
        {
            if (!playerXPlosPool[i].activeSelf)
                return playerXPlosPool[i];
        }
        return null;
    }

}
