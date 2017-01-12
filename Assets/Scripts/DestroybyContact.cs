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

    //Check if a projectile hit an enemy
	/*void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.GetComponent<MissileBehavior>() != null)
        {
            contact = other.gameObject.GetComponent<MissileBehavior>();
            contact.proj = missile;
        }
        else if (other.gameObject.GetComponent<ProjectileBehavior>() != null)
        {
            contact = other.gameObject.GetComponent<ProjectileBehavior>();
            contact.proj = bullet;
        }
        else
            return;

        //necessary to make sure that player projectile only hits *one* enemy only
        /*if (contact.isHit == false && other.gameObject.GetComponent<ProjectileBehavior>().isFriendly)
        {
            contact.isHit = true;
            hitby = contact.proj.hitType;
            switch (hitby)
            {
                case PlayerAttackType.proj:
                case PlayerAttackType.missile:
                    Destroy(other.gameObject);
                    DamageOnHit(contact.proj.damage);
                    break;
            }

            if(hitPoints <= 0)
            {
                Destroy(gameObject);
                gameController.ModifyScore(scoreValue);
                /*if (gameController.livesGained < gameController.score / (gameController.extraLifeReq) &&
                    gameController.livesGained < gameController.GetExtraLivesLimit())
                {
                    gameController.livesGained += 1;
                    gameController.ModifyLives(1);
                } 
            }
            contact = null;
        }
		
	}*/

	//handles players inside enemies too
	void OnTriggerStay(Collider player)
	{
		if(player.tag == "PlayerShip" && !gameController.Invincible && !gameController.playerDied)
        {
            gameController.setPlayerDeathFlag(true);
            Destroy (player.gameObject);
			if(gameController.playerLives == 0)
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
