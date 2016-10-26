using UnityEngine;
using System.Collections;

public class DestroyPlayer : MonoBehaviour {

    GameController gameController;

    //Useful if an attack is more complicated than what a collider can handle accurately
    public bool canKill = true;

    void Start()
    {
        GameObject target = GameObject.FindWithTag("GameController");
        if(target != null)
            gameController = target.GetComponent<GameController>();
    }

    void OnTriggerStay(Collider player)
    {
        if (!canKill)
            return;
        
        HandlePlayerHit(player);
    }


    // Useful for handling game logic using more than just collider-based detection
    public void HandlePlayerHit(Collider player)
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
