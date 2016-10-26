using UnityEngine;
using System.Collections;

public class DestroybyContact : MonoBehaviour {
    
	GameController gameController;

    void Start()
	{
        
		GameObject target = GameObject.FindWithTag("GameController");
        if(target.GetComponent<GameController>() != null)
		    gameController = target.GetComponent<GameController>();
    }

    void Update()
    {
    }

	//handles players inside enemies too
	void OnTriggerStay(Collider player)
    {
        if (player.CompareTag("PlayerShip") && !gameController.Invincible && 
            !gameController.playerDied && !gameController.getShieldStatus())
        {
            gameController.setPlayerDeathFlag(true);
            player.gameObject.SetActive(false);
            GameObject exp = gameController.GetComponent<SpecialFXPool>().playPlayerExplosion();
            exp.transform.position = player.transform.position;
            exp.SetActive(true);
            //Destroy(player.gameObject);
            if (gameController.playerLives == 0)
            {
                gameController.setGameOver();
            }
            else
            {
                gameController.ModifyLives(-1);
            }
        }
    }
}
