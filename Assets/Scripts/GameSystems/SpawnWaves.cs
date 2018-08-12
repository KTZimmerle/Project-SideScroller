using System.Collections;
using UnityEngine;

public class SpawnWaves : MonoBehaviour {

    const int MAX_HAZARD_SIZE = 35;
    const int MAX_POWERSHIP_SIZE = 6;
    const int MAX_SQUAD_SIZE = 8;
    const int MAX_FORMATION_SIZE = 15;
    const int MAX_PLATFORM_SIZE = 35;
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
    public GameObject BossTwo;
    FirstBossRoutine BEOne;
    FirstBossRoutineHard BEOneHM;
    SecondBossRoutine BETwo;
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
    PowerUpSystem player;
    GameObject powerOne;
    GameObject powerTwo;

    bool BEOneisHM = false;
    bool BETwoisHM = false;
    GameUI gameUI;
    ShipPool sPool;
    Camera c;
    /*GameObject powerupOne;
    GameObject powerupTwo;*/

    void Start ()
    {
        HazardLimit = 10;
        c = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        subtractTime = 0.0f;
        GameOverFlag = false;
        gameUI = GetComponent<GameUI>();
        BEOne = BossOne.GetComponent<FirstBossRoutine>();
        BEOneHM = BossOneHM.GetComponent<FirstBossRoutineHard>();
        BETwo = BossTwo.GetComponent<SecondBossRoutine>();
        bossAlive = false;
        sPool = GameObject.FindGameObjectWithTag("ShipPool").GetComponent<ShipPool>();
    }

    private void OnEnable()
    {
    }

    void Spawner()
    {
        if (waveCount >= 15 && !BEOneisHM)
        {
            BEOneisHM = true;
        }
        gameUI.UpdateWave(waveCount);

        totalEnemies = HazardLimit + extraSpawn;


        if (waveCount > 1)
        {
            totalEnemies += numFormations * squadSize;
            StartCoroutine(SpawnFormationOne());
        }

        if (waveCount > 4)
        {
            totalEnemies += numPlatforms;
            StartCoroutine(SpawnFormationTwo());
        }
        
        if (waveCount > 8)
        {
            totalEnemies += numHunters;
            StartCoroutine(SpawnShipHunter());
        }
    }

    public IEnumerator spawnWaves()
    {
        bonusPoints = 100;
        yield return new WaitForSeconds(startWait);
        player = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<PowerUpSystem>();
        while (!GameOverFlag)
        {
            waveCount += 1;
            Spawner();
            enemiesRemaining = totalEnemies;


            int nextSpawn = 1;
            for (int i = 0; i < HazardLimit; i++)
            {
                GameObject Razer = sPool.SpawnRazer();
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
            if (waveCount % 3 == 0)
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
                powerOne = SpawnPowerUp();
                if (GetComponent<ScoreBoard>().GetNumDestroyed()/totalEnemies == 1)
                {
                    powerOne.transform.position = new Vector3(3.0f, 3.0f);
                    powerOne.SetActive(true);
                    powerTwo = sPool.SpawnRandomPowerUp();
                    powerTwo.transform.position = new Vector3(3.0f, -3.0f);
                    powerTwo.SetActive(true);
                }
                else
                {
                    powerOne.transform.position = new Vector3(3.0f, 0.0f);
                    powerOne.SetActive(true);
                }
                yield return new WaitForSeconds(waveBreak + 1.0f);
                if (0 == waveCount % 5 && extraSpawn < MAX_POWERSHIP_SIZE)
                    extraSpawn += 1;
                GetComponent<GameController>().startNextWaveMessage();
                yield return new WaitForSeconds(waveBreak);
            }

            if(HazardLimit < MAX_HAZARD_SIZE)
                HazardLimit += 2;

            if (waveCount > 5) // was 3
                if (waveCount % 2 == 0 && numFormations < MAX_FORMATION_SIZE)// was 2
                {
                    subtractTime += 0.2f;
                    numFormations += 1;
                }

            if (waveCount > 7)
                if (waveCount % 2 == 0 && numPlatforms < MAX_PLATFORM_SIZE)//was 2
                    numPlatforms += 1;

            //change back to 22
            if (waveCount > 12)
                if (waveCount % 13 == 0 && numHunters < MAX_HUNTER_SIZE)
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
            //sets the spawn point for swoopers to be off-screen to the right side.
            //randomly chooses a y axis position
            Vector3 spawnPt = c.ScreenToWorldPoint(new Vector3(c.pixelWidth,
                                           Random.Range(0.0f + c.pixelHeight * 0.1f, c.pixelHeight - c.pixelHeight * 0.1f),
                                           c.nearClipPlane + 4.0f));/**/
            for (int i = 0; i < squadSize; i++)
            {
                GameObject Swooper = sPool.SpawnSwooper();
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
            //spawns the blaster either above or below off-screen and picking a random x axis position primarily on the right side
            yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
            GameObject Blaster = sPool.SpawnBlaster();
            //Blaster.transform.position = new Vector3(Random.Range(-spawnRangeTwo.x, spawnRangeTwo.x + 7), spawnRangeTwo.y, spawnRangeTwo.z);
            Blaster.transform.position = c.ScreenToWorldPoint(new Vector3(Random.Range(c.pixelWidth * 0.4f, c.pixelWidth * 0.9f),
                                         CoinFlip(-c.pixelHeight * 0.1f, c.pixelHeight * 1.1f),
                                         c.nearClipPlane + 4.0f));
            Blaster.transform.rotation = Quaternion.identity;
            Blaster.SetActive(true);
        }
    }

    void SpawnPowerShip()
    {
        GameObject PowerShip = sPool.SpawnPowerShip();
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
        //boss one point only
        Vector3 spawnPt = c.ScreenToWorldPoint(new Vector3(c.pixelWidth + c.pixelWidth * 0.15f,
                          c.pixelHeight * 0.5f,
                          c.nearClipPlane + 4.0f));

        bossAlive = true;

        if (waveCount >= 6 && waveCount % 6 == 0)
        {
            boss = sPool.SpawnAIDrones();
            boss.SetActive(true);
            yield return new WaitWhile(() => !boss.GetComponent<SecondBossRoutine>().isDying);
        }
        else
        {
            if (!BEOneisHM)
            {
                //boss = Instantiate(BossOne, spawnPt, spawnRotate) as GameObject;
                boss = sPool.SpawnBattleShip();
                boss.transform.position = spawnPt;
                boss.transform.rotation = Quaternion.identity;
                boss.SetActive(true);
                yield return new WaitWhile(() => boss.GetComponent<FirstBossRoutine>().notDead);
            }
            else
            {
                boss = sPool.SpawnBattleShipHM();
                boss.transform.position = spawnPt;
                boss.transform.rotation = Quaternion.identity;
                boss.SetActive(true);
                //boss = Instantiate(BossOneHM, spawnPt, spawnRotate) as GameObject;
                yield return new WaitWhile(() => boss.GetComponent<FirstBossRoutineHard>().notDead);
            }
        }

        bossAlive = false;
    }

    IEnumerator SpawnShipHunter()
    {
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < numHunters; i++)
        {
            //Spawns the hunter either to the left or right off screen and choosing a random y axis position.
            GameObject Hunter = sPool.SpawnHunter();
            Hunter.transform.position = c.ScreenToWorldPoint(new Vector3(CoinFlip(0, c.pixelWidth),
                                        Random.Range(0.0f + c.pixelHeight * 0.1f, c.pixelHeight - c.pixelHeight * 0.1f),
                                        c.nearClipPlane + 4.0f));
            Hunter.transform.rotation = Quaternion.identity;

            gameUI.WarningIcon.gameObject.SetActive(true);
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

    public GameObject SpawnPowerUp()
    {
        if (!player.CheckMissilePowUp())
            return sPool.SpawnMissilePowerUp();
        else if (!player.CheckCrossFirePowUp())
            return sPool.SpawnCrossFirePowerUp();
        else if (!player.CheckLaserPowUp())
            return sPool.SpawnLaserPowerUp();
        else
            return null;
    }
}
