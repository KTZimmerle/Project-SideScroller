using UnityEngine;
using System.Collections;

public class DestroybyContact : MonoBehaviour {

    public int hitPoints;
	public int scoreValue;
	GameController gameController;
    PlayerAttackType hitby;
    ProjectileBehavior contact;
    Bullet bullet;
    Missile missile;
    //AbstractProjectile proj;

    void Start()
	{
        
		GameObject target = GameObject.FindWithTag("GameController");
        if(target.GetComponent<GameController>() != null)
		    gameController = target.GetComponent<GameController>();
        bullet = new Bullet();
        missile = new Missile();
        contact = null;
    }

	//handles players inside enemies too
	void OnTriggerStay(Collider player)
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

    void DamageOnHit(int dmg)
    {
        hitPoints -= dmg;
    }
}
