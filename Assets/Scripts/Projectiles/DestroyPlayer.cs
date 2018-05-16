using UnityEngine;
using System.Collections;

public class DestroyPlayer : MonoBehaviour {

    SpecialFXPool specialFX;
    GameController gc;
    GameObject orbOne;
    GameObject orbTwo;
    GameObject orbSpawn;

    //Useful if an attack is more complicated than what a collider can handle accurately
    public bool canKill = true;

    void Start()
    {
        orbOne = null;
        orbTwo = null;
        orbSpawn = null;
        GameObject target = GameObject.FindWithTag("GFXPool");
        if(target != null)
            specialFX = target.GetComponent<SpecialFXPool>();
        target = GameObject.FindWithTag("GameController");
        if (target != null)
            gc = target.GetComponent<GameController>();
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
        if (player.CompareTag("PlayerShip") && !gc.Invincible &&
            !gc.playerDied && !gc.getShieldStatus())
        {
            gc.setPlayerDeathFlag(true);
            player.gameObject.SetActive(false);

            GameObject exp = specialFX.GetComponent<SpecialFXPool>().playPlayerExplosion();
            exp.transform.position = player.transform.position;
            exp.SetActive(true);
            //Destroy(player.gameObject);
            if (gc.playerLives == 0)
            {
                gc.setGameOver();
            }
            else
            {
                gc.ModifyLives(-1);
            }
        }
    }
}
