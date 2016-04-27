using UnityEngine;
using System.Collections;

public class DestroybyContact : MonoBehaviour {

    public int hitPoints;
	public int scoreValue;
    public bool isHit;
	GameController gameController;
    PlayerAttackType hitby;
    ProjectileBehavior proj;
    MissileBehavior missile;


    void Start()
	{
		GameObject target = GameObject.FindWithTag("GameController");
		gameController = target.GetComponent<GameController>();
        isHit = false;
    }

    //Check if a projectile hit an enemy
	void OnTriggerEnter(Collider other)
	{
        
		if(other.tag != "Boundary" && other.tag != gameObject.tag && 
           other.tag != "PlayerShip" && other.tag != "EnemyProj")
		{
            
            //necessary to make sure that player projectile only hits *one* enemy only
            proj = GameObject.FindWithTag("PlayerProjectile").GetComponent<ProjectileBehavior>();
            
            if (proj.isHit == false)
            {
                proj.isHit = true;
                hitby = proj.hitType;
                switch (hitby)
                {
                    case PlayerAttackType.proj:
                    case PlayerAttackType.missile:
                        Destroy(other.gameObject);
                        DamageOnHit(proj.damage);
                        break;
                }

                if(hitPoints <= 0)
                {
                    Destroy(gameObject);
                    gameController.ModifyScore(scoreValue);
                    if (gameController.livesGained < gameController.score / (gameController.extraLifeReq) &&
                        gameController.livesGained < gameController.GetExtraLivesLimit())
                    {
                        gameController.livesGained += 1;
                        gameController.ModifyLives(1);
                    } 
                }
            }
		}
	}

	//handles players inside enemies too
	void OnTriggerStay(Collider player)
	{
		if(player.tag == "PlayerShip" && !gameController.Invincible)
		{
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
