using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject PlayerShip;
	public GameObject RespawnPoint;
    
	public float playerSpawnWait;
	public float playerInvincibilityTimer;
	float invincibilityTimer;
	public int playerLives = 2;

    GameUI gameUI;
    SpawnWaves waves;
    GameObject StarFighter;
    public int score = 0;
	public int extraLifeReq = 2000;
    int reqScore;
	public bool Invincible = false;
    bool shielded = false;
    public bool playerDied = false;
	//Color transperency;
    
	// Use this for initialization
	void Start () 
	{
        reqScore = 0;
        score = 0;
		//transperency = PlayerShip.GetComponent<Renderer>().sharedMaterial.color;
		invincibilityTimer = playerInvincibilityTimer;

        gameUI = GetComponent<GameUI>();
        waves = GetComponent<SpawnWaves>();
        gameUI.Initialize(playerLives, score, 0);

        //Spawn Player for the first time
        StarFighter = GetComponent<ShipPool>().SpawnStarFighter();
        StarFighter.transform.position = new Vector3 (RespawnPoint.transform.position.x, 0.0f, 0.0f);
        StarFighter.transform.rotation = PlayerShip.transform.rotation;
        StarFighter.gameObject.SetActive(true);
        //Instantiate (PlayerShip, spawnPos, spawnRotate);
        StartCoroutine (waves.spawnWaves());
	}
	
	void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            //start the game
            //Application.LoadLevel("Title Screen");
            SceneManager.LoadScene(0);

        }

		if(Invincible)
		{
			if(invincibilityTimer <= 0.0f)
			{
				Invincible = false;
                //transperency.a = 1.0f;
            }
			invincibilityTimer -= Time.deltaTime;
		}
	}

	
	public void ModifyScore (int modifyScore)
    {
        score += modifyScore;
        gameUI.UpdateScore(score);
        if (score > extraLifeReq)
        {
            reqScore += 500;
            extraLifeReq += 2000 + reqScore;
            ModifyLives(1);
        }
    }

	public void ModifyLives (int modifyLives)
	{
		playerLives += modifyLives;
        gameUI.UpdateLives(playerLives);
		if(modifyLives < 0)
		{
            //gameUI.DeHighlightAll();
            PowerUpSystem ps = StarFighter.GetComponent<PowerUpSystem>();
            gameUI.RestoreButtonText(ps.missilePowUp, ps.altfirePowUp, ps.laserPowUp);
            gameUI.speedmultiplier = 1.0f;
            gameUI.SpeedupText.text = "Speed x 1.0";
            StartCoroutine (RespawnPlayer());
		}
	}

	IEnumerator RespawnPlayer()
	{
		yield return new WaitForSeconds (playerSpawnWait);
		/*Vector3 spawnPos = new Vector3 (RespawnPoint.transform.position.x, 0.0f, 0.0f);
		Quaternion spawnRotate = PlayerShip.transform.rotation;*/
        StarFighter = GetComponent<ShipPool>().SpawnStarFighter();
        StarFighter.transform.position = new Vector3(RespawnPoint.transform.position.x, 0.0f, 0.0f);
        StarFighter.transform.rotation = PlayerShip.transform.rotation;
        StarFighter.GetComponent<PlayerController>().revived = true;
        StarFighter.gameObject.SetActive(true);
        //clone = Instantiate (PlayerShip, spawnPos, spawnRotate) as GameObject;
        //clone.GetComponent<PlayerController>().revived = true;
        Invincible = true;
		//transperency.a = 0.5f;
		invincibilityTimer = playerInvincibilityTimer;
        setPlayerDeathFlag(false);
    }

    public void setGameOver()
    {
        waves.setGameOverFlag();
        gameUI.GameOver();
    }

    public void setPlayerDeathFlag(bool change)
    {
        playerDied = change;
    }

    public void setShieldStatus(bool status)
    {
        shielded = status;
    }

    public bool getShieldStatus()
    {
        return shielded;
    }

    public void startNextWaveMessage()
    {
        StartCoroutine(gameUI.nextWaveMessage());
    }

    public void starIncomingBossMessage()
    {
        StartCoroutine(gameUI.bossIncomingMessage());
    }
}
