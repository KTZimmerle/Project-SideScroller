﻿using UnityEngine;
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
        if (player.CompareTag("PlayerShip") && !gameController.Invincible && !gameController.playerDied)
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
