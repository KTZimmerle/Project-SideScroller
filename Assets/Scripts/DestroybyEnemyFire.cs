using UnityEngine;
using System.Collections;

public class DestroybyEnemyFire : MonoBehaviour {

    GameController gameController;

    void Start()
    {
        GameObject target = GameObject.FindWithTag("GameController");
        gameController = target.GetComponent<GameController>();
    }

    void OnTriggerStay(Collider player)
    {
        if (player.tag == "PlayerShip" && !gameController.Invincible)
        {
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
