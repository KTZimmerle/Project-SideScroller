using UnityEngine;
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
	public int score = 0;
	public int extraLifeReq = 100;
	public bool Invincible = false;
    public int livesGained = 0;
	//Color transperency;
    const int EXTRA_LIFE_LIMIT = 50;
    
	// Use this for initialization
	void Start () 
	{
		score = 0;
		//transperency = PlayerShip.GetComponent<Renderer>().sharedMaterial.color;
		invincibilityTimer = playerInvincibilityTimer;

        gameUI = GetComponent<GameUI>();
        waves = GetComponent<SpawnWaves>();
        gameUI.Initialize(playerLives, score, 0);

        //Spawn Player for the first time
        Vector3 spawnPos = new Vector3 (RespawnPoint.transform.position.x, 0.0f, 0.0f);
		Quaternion spawnRotate = Quaternion.identity;
		Instantiate (PlayerShip, spawnPos, spawnRotate);
		StartCoroutine (waves.spawnWaves());
	}
	
	void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            //start the game
            Application.LoadLevel("Title Screen");
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
	}

	public void ModifyLives (int modifyLives)
	{
		playerLives += modifyLives;
        gameUI.UpdateLives(playerLives);
		if(modifyLives < 0)
		{
			StartCoroutine (RespawnPlayer());
		}
	}

	IEnumerator RespawnPlayer()
	{
		yield return new WaitForSeconds (playerSpawnWait);
		Vector3 spawnPos = new Vector3 (RespawnPoint.transform.position.x, 0.0f, 0.0f);
		Quaternion spawnRotate = Quaternion.identity;
		Instantiate (PlayerShip, spawnPos, spawnRotate);
		Invincible = true;
		//transperency.a = 0.5f;
		invincibilityTimer = playerInvincibilityTimer;
	}

    public int GetExtraLivesLimit()
    {
        return EXTRA_LIFE_LIMIT;
    }

    public void setGameOver()
    {
        gameUI.GameOver(true);
    }
}
