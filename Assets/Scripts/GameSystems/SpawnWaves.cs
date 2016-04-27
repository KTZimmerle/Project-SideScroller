using UnityEngine;
using System.Collections;

public class SpawnWaves : MonoBehaviour {

    int waveCount = 0;
    bool GameOverFlag = false;
    public Vector3 spawnRange;
    public Vector3 spawnRangeTwo;
    public int HazardLimit;
    public float startWait;
    public GameObject Hazard;
    public GameObject EnemyShipOne;
    public GameObject EnemyShipTwo;
    int numFormations = 2;
    int numPlatforms = 2;
    int enemySqdSz = 5;
    int extraSpawn = 1;
    public float spawnWait;
    public float waveBreak;

    GameUI gameUI;
    // Use this for initialization
    void Start ()
    {
        gameUI = GetComponent<GameUI>();
    }

    public IEnumerator spawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (!GameOverFlag)
        {
            waveCount += 1;
            gameUI.UpdateWave(waveCount);
            if (waveCount > 2)
            {
                Vector3 spawnPos = Vector3.zero;
                StartCoroutine(SpawnFormationOne(spawnPos));
            }

            if (waveCount > 6)
            {
                Vector3 spawnPos = Vector3.zero;
                StartCoroutine(SpawnFormationTwo(spawnPos));
            }

            for (int i = 0; i < HazardLimit; i++)
            {
                Vector3 spawnPos = new Vector3(spawnRange.x, Random.Range(-spawnRange.y, spawnRange.y), spawnRange.z);
                Quaternion spawnRotate = Quaternion.identity;
                Instantiate(Hazard, spawnPos, spawnRotate);
                if (waveCount > 6)
                {
                    if (0 == waveCount % 6)
                        extraSpawn += 1;

                    int iter = 0;
                    while (iter < extraSpawn)
                    {
                        spawnPos = new Vector3(spawnRange.x, Random.Range(-spawnRange.y, spawnRange.y), spawnRange.z);
                        spawnRotate = Quaternion.identity;
                        Instantiate(Hazard, spawnPos, spawnRotate);
                        iter++;
                    }
                }
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveBreak);
            HazardLimit += waveCount;

            if (waveCount > 3)
                if (waveCount % 2 == 0)
                    numFormations += 1;

            if (waveCount > 7)
                if (waveCount % 2 == 0)
                    numPlatforms += 1;

            if (waveCount % 4 == 0)
                enemySqdSz += 1;
        }
    }

    //enemy formations
    IEnumerator SpawnFormationOne(Vector3 spawnPt)
    {
        yield return new WaitForSeconds(startWait + 2.0f);
        for (int j = 0; j < numFormations; j++)
        {
            spawnPt = new Vector3(spawnRange.x, Random.Range(-spawnRange.y, spawnRange.y), spawnRange.z);
            for (int i = 0; i < enemySqdSz; i++)
            {
                Quaternion spawnRotate = Quaternion.identity;
                Instantiate(EnemyShipOne, spawnPt, spawnRotate);
                yield return new WaitForSeconds(0.25f);
            }
            yield return new WaitForSeconds(4.0f);
        }
    }

    IEnumerator SpawnFormationTwo(Vector3 spawnPt)
    {

        yield return new WaitForSeconds(startWait + 2.0f);
        for (int i = 0; i < numPlatforms; i++)
        {
            spawnRangeTwo.y *= CoinFlip(1, -1);
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
            spawnPt = new Vector3(Random.Range(-spawnRangeTwo.x, spawnRangeTwo.x + 7), spawnRangeTwo.y, spawnRangeTwo.z);
            Quaternion spawnRotate = Quaternion.identity;
            Instantiate(EnemyShipTwo, spawnPt, spawnRotate);
        }
    }
    
    int CoinFlip(int resultOne, int resultTwo)
    {
        return Random.Range(0, 2) > 0 ? resultOne : resultTwo;
    }

}
