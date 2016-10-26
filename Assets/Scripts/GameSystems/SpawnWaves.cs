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
    public bool GameOverFlag;
    public bool bossAlive;
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
    int bonusPoints;
    int totalEnemies;
    int enemiesRemaining;
    public float spawnWait;
    public float waveBreak;
    public float bossWait;
    float subtractTime;

    bool BEOneisHM = false;
    GameUI gameUI;
    ShipPool sPool;
    Camera c;

    void Start ()
    {
        HazardLimit = 10;
        c = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        subtractTime = 0.0f;
        GameOverFlag = false;
        gameUI = GetComponent<GameUI>();
        BEOne = BossOne.GetComponent<FirstBossRoutine>();
        BEOneHM = BossOneHM.GetComponent<FirstBossRoutineHard>();
        bossAlive = false;
    }

    void Spawner()
    {
        if (waveCount >= 15 && !BEOneisHM)
        {
            BEOneisHM = true;
        }
        gameUI.UpdateWave(waveCount);

        totalEnemies = HazardLimit + extraSpawn;


        if (waveCount > 2)
        {
            totalEnemies += numFormations * squadSize;
            StartCoroutine(SpawnFormationOne());
        }

        if (waveCount > 6)
        {
            totalEnemies += numPlatforms;
            StartCoroutine(SpawnFormationTwo());
        }

        if (waveCount > 10)
        {
            totalEnemies += numHunters;
            StartCoroutine(SpawnShipHunter());
        }
    }

    public IEnumerator spawnWaves()
    {
        bonusPoints = 100;
        yield return new WaitForSeconds(startWait);
        while (!GameOverFlag)
        {
            waveCount += 1;
            Spawner();
            enemiesRemaining = totalEnemies;


            int nextSpawn = 1;
            for (int i = 0; i < HazardLimit; i++)
            {
                GameObject Razer = GetComponent<ShipPool>().SpawnRazer();
                //Razer.transform.position = new Vector3(spawnRange.x, Random.Range(-spawnRange.y, spawnRange.y), spawnRange.z);
                Razer.transform.position = c.ScreenToWorldPoint(new Vector3(c.pixelWidth, 
                                           Random.Range(0.0f + c.pixelHeight * 0.1f, c.pixelHeight - c.pixelHeight * 0.1f), 
                                           c.nearClipPlane + 4.0f));
                Razer.transform.rotation = Quaternion.identity;
                Razer.SetActive(true);
                if (nextSpawn/(extraSpawn+1.0f) < (i / (float)HazardLimit))
                {
                    SpawnPowerShip();
                    nextSpawn++;
                }
                //Instantiate(Hazard, spawnPos, spawnRotate);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitWhile(() => (enemiesRemaining > 0));
            yield return new WaitForSeconds(2.0f);
            
            //Spawn a boss
            if (waveCount % 5 == 0)
            {
                GetComponent<GameController>().starIncomingBossMessage();
                yield return new WaitForSeconds(6.0f);
                setBossStatus(true);
                StartCoroutine(StartBossRoutine());
                yield return new WaitWhile(() => bossAlive);
                yield return new WaitForSeconds(8.0f);
            }
            
            //StartCoroutine(SpawnPowerShips());
            if (!GameOverFlag)
            {
                GetComponent<ScoreBoard>().setEnemyCount(totalEnemies);
                GetComponent<ScoreBoard>().tallyScore(bonusPoints);
                yield return new WaitForSeconds(waveBreak + 1.0f);
                if (0 == waveCount % 5 && extraSpawn < MAX_POWERSHIP_SIZE)
                    extraSpawn += 1;
                GetComponent<GameController>().startNextWaveMessage();
                yield return new WaitForSeconds(waveBreak);
            }

            if(HazardLimit < MAX_HAZARD_SIZE)
                HazardLimit += 2;

            if (waveCount > 5) // was 3
                if (waveCount % 3 == 0 && numFormations < MAX_FORMATION_SIZE)// was 2
                {
                    subtractTime += 0.2f;
                    numFormations += 1;
                }

            if (waveCount > 7)
                if (waveCount % 3 == 0 && numPlatforms < MAX_PLATFORM_SIZE)//was 2
                    numPlatforms += 1;

            if (waveCount > 22)
                if (waveCount % 23 == 0 && numHunters < MAX_HUNTER_SIZE)
                    numHunters += 1;

            if(waveCount > 5)
                if (waveCount % 3 == 0 && squadSize < MAX_SQUAD_SIZE)//was 3
                    squadSize += 1;

            //each successful round gives a bigger bonus
            bonusPoints += 100;
        }
    }

    /*void SpawnFormationOne(Vector3 spawnPt)
    {
        EnemiesRemaining = true;
        for (int j = 0; j < numFormations; j++)
        {
            spawnPt = new Vector3(spawnRange.x, Random.Range(-spawnRange.y, spawnRange.y), spawnRange.z);
            for (int i = 0; i < squadSize; i++)
            {
                GameObject Swooper = GetComponent<ShipPool>().SpawnSwooper();
                Swooper.transform.position = spawnPt;
                Swooper.transform.rotation = Quaternion.identity;
                Swooper.SetActive(true);
            }
        }
        EnemiesRemaining = false;
    }*/

    //enemy formations
    IEnumerator SpawnFormationOne()
    {
        yield return new WaitForSeconds(startWait + 2.0f);
        for (int j = 0; j < numFormations; j++)
        {
            //spawnPt = new Vector3(spawnRange.x, Random.Range(-spawnRange.y, spawnRange.y), spawnRange.z);
            Vector3 spawnPt = c.ScreenToWorldPoint(new Vector3(c.pixelWidth,
                                           Random.Range(0.0f + c.pixelHeight * 0.1f, c.pixelHeight - c.pixelHeight * 0.1f),
                                           c.nearClipPlane + 4.0f));/**/
            for (int i = 0; i < squadSize; i++)
            {
                GameObject Swooper = GetComponent<ShipPool>().SpawnSwooper();
                Swooper.transform.position = spawnPt;
                Swooper.transform.rotation = Quaternion.identity;
                Swooper.SetActive(true);
                //Instantiate(EnemyShipOne, spawnPt, spawnRotate);
                yield return new WaitForSeconds(0.25f);
            }
            yield return new WaitForSeconds(4.0f - subtractTime);
        }
        //enemiesRemaining -= squadSize * numFormations;
    }

    IEnumerator SpawnFormationTwo()
    {
        yield return new WaitForSeconds(startWait + 2.0f);
        for (int i = 0; i < numPlatforms; i++)
        {
            //spawnRangeTwo.y *= CoinFlip(1, -1);
            yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
            GameObject Blaster = GetComponent<ShipPool>().SpawnBlaster();
            //Blaster.transform.position = new Vector3(Random.Range(-spawnRangeTwo.x, spawnRangeTwo.x + 7), spawnRangeTwo.y, spawnRangeTwo.z);
            Blaster.transform.position = c.ScreenToWorldPoint(new Vector3(Random.Range(c.pixelWidth * 0.4f, c.pixelWidth * 0.9f),
                                         CoinFlip(-c.pixelHeight * 0.1f, c.pixelHeight * 1.1f),
                                         c.nearClipPlane + 4.0f));
            Blaster.transform.rotation = Quaternion.identity;
            Blaster.SetActive(true);
            //spawnPt = new Vector3(Random.Range(-spawnRangeTwo.x, spawnRangeTwo.x + 7), spawnRangeTwo.y, spawnRangeTwo.z);
            //Quaternion spawnRotate = Quaternion.identity;
            //Instantiate(EnemyShipTwo, spawnPt, spawnRotate);
        }
        //enemiesRemaining -= numPlatforms;
    }

    void SpawnPowerShip()
    {
        GameObject PowerShip = GetComponent<ShipPool>().SpawnPowerShip();
        PowerShip.transform.position = c.ScreenToWorldPoint(new Vector3(c.pixelWidth * 1.1f,
                                        Random.Range(0.0f + c.pixelHeight * 0.25f, c.pixelHeight - c.pixelHeight * 0.25f),
                                        c.nearClipPlane + 4.0f));
        PowerShip.transform.rotation = Quaternion.identity;
        PowerShip.SetActive(true);
    }

    /*IEnumerator SpawnPowerShips()
    {
        if (0 == waveCount % 5 && extraSpawn < MAX_POWERSHIP_SIZE)
            extraSpawn += 1;
        
        int iter = 0;
        while (iter < extraSpawn)
        {
            GameObject PowerShip = GetComponent<ShipPool>().SpawnPowerShip();
            PowerShip.transform.position = c.ScreenToWorldPoint(new Vector3(c.pixelWidth * 1.1f,
                                           Random.Range(0.0f + c.pixelHeight * 0.25f, c.pixelHeight - c.pixelHeight * 0.25f),
                                           c.nearClipPlane + 4.0f));
            //PowerShip.transform.position = new Vector3(spawnRange.x, Random.Range(-spawnRange.y / 2, spawnRange.y / 2), spawnRange.z);
            PowerShip.transform.rotation = Quaternion.identity;
            PowerShip.SetActive(true);
            //spawnPt = new Vector3(spawnRange.x, Random.Range(-spawnRange.y/2, spawnRange.y/2), spawnRange.z);
            //Quaternion spawnRotate = Quaternion.identity;
            //Instantiate(EnemyShipPower, spawnPt, spawnRotate);
            iter++;
            yield return new WaitForSeconds(spawnWait);
        }
    }*/

    IEnumerator StartBossRoutine()
    {
        yield return new WaitForSeconds(bossWait);
        GameObject boss;
        //Vector3 spawnPt = new Vector3(13.0f, 0.0f, 0.0f);
        Vector3 spawnPt = c.ScreenToWorldPoint(new Vector3(c.pixelWidth + c.pixelWidth * 0.15f,
                          c.pixelHeight * 0.5f,
                          c.nearClipPlane + 4.0f));

        bossAlive = true;
        if(!BEOneisHM)
        {
            //boss = Instantiate(BossOne, spawnPt, spawnRotate) as GameObject;
            boss = GetComponent<ShipPool>().SpawnBattleShip();
            boss.transform.position = spawnPt;
            boss.transform.rotation = Quaternion.identity;
            boss.SetActive(true);
            yield return new WaitWhile(() => boss.GetComponent<FirstBossRoutine>().notDead);
        }
        else
        {
            boss = GetComponent<ShipPool>().SpawnBattleShipHM();
            boss.transform.position = spawnPt;
            boss.transform.rotation = Quaternion.identity;
            boss.SetActive(true);
            //boss = Instantiate(BossOneHM, spawnPt, spawnRotate) as GameObject;
            yield return new WaitWhile(() => boss.GetComponent<FirstBossRoutineHard>().notDead);
        }

        bossAlive = false;
    }

    IEnumerator SpawnShipHunter()
    {
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < numHunters; i++)
        {
            GameObject Hunter = GetComponent<ShipPool>().SpawnHunter();
            //Hunter.transform.position = new Vector3(spawnRangeThree.x, Random.Range(-spawnRangeThree.y, spawnRangeThree.y), spawnRangeThree.z);
            Hunter.transform.position = c.ScreenToWorldPoint(new Vector3(CoinFlip(0, c.pixelWidth),
                                        Random.Range(0.0f + c.pixelHeight * 0.1f, c.pixelHeight - c.pixelHeight * 0.1f),
                                        c.nearClipPlane + 4.0f));
            Hunter.transform.rotation = Quaternion.identity;
            if (Hunter.transform.position.x < 0.0f)
            {
                gameUI.WarningIcon.GetComponent<RectTransform>().anchorMax = new Vector2(0.0f, 0.5f);
                gameUI.WarningIcon.GetComponent<RectTransform>().anchorMin = new Vector2(0.0f, 0.5f);
                gameUI.WarningIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(128.0f, 0.0f);
            }
            else
            {
                gameUI.WarningIcon.GetComponent<RectTransform>().anchorMax = new Vector2(1.0f, 0.5f);
                gameUI.WarningIcon.GetComponent<RectTransform>().anchorMin = new Vector2(1.0f, 0.5f);
                gameUI.WarningIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(-128.0f, 0.0f);
            }

            for (int j = 0; j < 5; j++)
            {
                gameUI.WarningIcon.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                gameUI.WarningIcon.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.1f);
            }
            //spawnRangeThree.x *= CoinFlip(1, -1);
            Hunter.SetActive(true);
            //Instantiate(EnemyShipThree, spawnPt, spawnRotate);
            yield return new WaitForSeconds(3.0f);
        }
        //enemiesRemaining -= numHunters;
    }

    public void decrementEnemyCount()
    {
        enemiesRemaining--;
    }

    float CoinFlip(float resultOne, float resultTwo)
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
