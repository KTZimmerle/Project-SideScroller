using UnityEngine;
using System.Collections;

public class SpawnWaves : MonoBehaviour {

    const int MAX_HAZARD_SIZE = 50;
    const int MAX_POWERSHIP_SIZE = 6;
    const int MAX_SQUAD_SIZE = 8;
    const int MAX_FORMATION_SIZE = 8;
    const int MAX_PLATFORM_SIZE = 25;
    const int MAX_HUNTER_SIZE = 5;
    int waveCount = 0;
    bool GameOverFlag;
    bool EnemiesRemaining;
    public bool bossAlive;
    public Vector3 spawnRange;
    public Vector3 spawnRangeTwo;
    public Vector3 spawnRangeThree;
    public int HazardLimit;
    public float startWait;
    public GameObject Hazard;
    public GameObject EnemyShipOne;
    public GameObject EnemyShipTwo;
    public GameObject EnemyShipThree;
    public GameObject EnemyShipPower;
    public GameObject BossOne;
    public GameObject BossOneHM;
    FirstBossRoutine BEOne;
    FirstBossRoutineHard BEOneHM;
    int numFormations = 2;
    int numPlatforms = 2;
    int numHunters = 2;
    int squadSize = 5;
    int extraSpawn = 1;
    public float spawnWait;
    public float waveBreak;
    public float bossWait;
    float subtractTime;

    bool BEOneisHM = false;
    GameUI gameUI;
    // Use this for initialization
    void Start ()
    {
        subtractTime = 0.0f;
        GameOverFlag = false;
        gameUI = GetComponent<GameUI>();
        BEOne = BossOne.GetComponent<FirstBossRoutine>();
        BEOneHM = BossOneHM.GetComponent<FirstBossRoutineHard>();
        bossAlive = false;
        EnemiesRemaining = false;
    }

    public IEnumerator spawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (!GameOverFlag)
        {

            waveCount += 1;
            if (waveCount >= 20 && !BEOneisHM)
            {
                BEOneisHM = true;
            }
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

            if (waveCount > 10)
            {
                Vector3 spawnPos = Vector3.zero;
                StartCoroutine(SpawnShipHunter(spawnPos));
            }

            for (int i = 0; i < HazardLimit; i++)
            {
                Vector3 spawnPos = new Vector3(spawnRange.x, Random.Range(-spawnRange.y, spawnRange.y), spawnRange.z);
                Quaternion spawnRotate = Quaternion.identity;
                Instantiate(Hazard, spawnPos, spawnRotate);
                yield return new WaitForSeconds(spawnWait);
            }
            
            yield return new WaitWhile(() => EnemiesRemaining);

            //Spawn a boss
            if (waveCount % 5 == 0)
            {
                GetComponent<GameController>().starIncomingBossMessage();
                yield return new WaitForSeconds(6.0f);
                setBossStatus(true);
                StartCoroutine(StartBossRoutine(Vector3.zero));
                yield return new WaitWhile(() => bossAlive);
                yield return new WaitForSeconds(11.0f);
            }
            
            StartCoroutine(SpawnPowerShips(Vector3.zero));
            if(!GameOverFlag)
                GetComponent<GameController>().startNextWaveMessage();
            yield return new WaitForSeconds(waveBreak);
            if(HazardLimit < MAX_HAZARD_SIZE)
                HazardLimit += 2;

            if (waveCount > 3)
                if (waveCount % 2 == 0 && numFormations < MAX_FORMATION_SIZE)
                {
                    subtractTime += 0.2f;
                    numFormations += 1;
                }

            if (waveCount > 7)
                if (waveCount % 2 == 0 && numPlatforms < MAX_PLATFORM_SIZE)
                    numPlatforms += 1;

            if (waveCount > 22)
                if (waveCount % 23 == 0 && numHunters < MAX_HUNTER_SIZE)
                    numHunters += 1;

            if (waveCount % 3 == 0 && squadSize < MAX_SQUAD_SIZE)
                squadSize += 1;
        }
    }

    //enemy formations
    IEnumerator SpawnFormationOne(Vector3 spawnPt)
    {
        yield return new WaitForSeconds(startWait + 2.0f);
        EnemiesRemaining = true;
        for (int j = 0; j < numFormations; j++)
        {
            spawnPt = new Vector3(spawnRange.x, Random.Range(-spawnRange.y, spawnRange.y), spawnRange.z);
            for (int i = 0; i < squadSize; i++)
            {
                Quaternion spawnRotate = Quaternion.identity;
                Instantiate(EnemyShipOne, spawnPt, spawnRotate);
                yield return new WaitForSeconds(0.25f);
            }
            yield return new WaitForSeconds(4.0f - subtractTime);
        }
        EnemiesRemaining = false;
    }

    IEnumerator SpawnFormationTwo(Vector3 spawnPt)
    {

        yield return new WaitForSeconds(startWait + 2.0f);
        for (int i = 0; i < numPlatforms; i++)
        {
            spawnRangeTwo.y *= CoinFlip(1, -1);
            yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
            spawnPt = new Vector3(Random.Range(-spawnRangeTwo.x, spawnRangeTwo.x + 7), spawnRangeTwo.y, spawnRangeTwo.z);
            Quaternion spawnRotate = Quaternion.identity;
            Instantiate(EnemyShipTwo, spawnPt, spawnRotate);
        }
    }

    IEnumerator SpawnPowerShips(Vector3 spawnPt)
    {
        if (0 == waveCount % 5 && extraSpawn < MAX_POWERSHIP_SIZE)
            extraSpawn += 1;
        
        int iter = 0;
        while (iter < extraSpawn)
        {
            spawnPt = new Vector3(spawnRange.x, Random.Range(-spawnRange.y/2, spawnRange.y/2), spawnRange.z);
            Quaternion spawnRotate = Quaternion.identity;
            Instantiate(EnemyShipPower, spawnPt, spawnRotate);
            iter++;
            yield return new WaitForSeconds(spawnWait);
        }
    }

    IEnumerator StartBossRoutine(Vector3 spawnPt)
    {
        yield return new WaitForSeconds(bossWait);
        GameObject boss;
        spawnPt = new Vector3(13.0f, 0.0f, 0.0f);
        Quaternion spawnRotate = Quaternion.identity;

        bossAlive = true;
        if(!BEOneisHM)
        {
            boss = Instantiate(BossOne, spawnPt, spawnRotate) as GameObject;
            yield return new WaitWhile(() => boss.GetComponent<FirstBossRoutine>().notDead);
        }
        else
        {
            boss = Instantiate(BossOneHM, spawnPt, spawnRotate) as GameObject;
            yield return new WaitWhile(() => boss.GetComponent<FirstBossRoutineHard>().notDead);
        }

        bossAlive = false;
    }

    IEnumerator SpawnShipHunter(Vector3 spawnPt)
    {
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < numHunters; i++)
        {
            spawnRangeThree.x *= CoinFlip(1, -1);
            spawnPt = new Vector3(spawnRangeThree.x, Random.Range(-spawnRangeThree.y, spawnRangeThree.y), spawnRangeThree.z);
            Quaternion spawnRotate = Quaternion.identity;
            Instantiate(EnemyShipThree, spawnPt, spawnRotate);
            yield return new WaitForSeconds(4.0f);
        }
    }

    int CoinFlip(int resultOne, int resultTwo)
    {
        return Random.Range(0, 2) > 0 ? resultOne : resultTwo;
    }

    public bool setGameOverFlag()
    {
        return GameOverFlag = true;
    }

    public void setBossStatus(bool status)
    {
        bossAlive = status;
    }
}
