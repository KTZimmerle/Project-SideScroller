using UnityEngine;
using System.Collections;

public class DestroybyEnemyFire : MonoBehaviour {

    GameController gameController;
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

    public void HandlePlayerHit(Collider player)
    {
        if (player.CompareTag("PlayerShip") && !gameController.Invincible && 
            !gameController.playerDied && !gameController.getShieldStatus())
        {
            gameController.setPlayerDeathFlag(true);
            Destroy(player.gameObject);
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
