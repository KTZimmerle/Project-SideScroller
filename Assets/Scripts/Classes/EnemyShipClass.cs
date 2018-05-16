using UnityEngine;
using System.Collections;

public class EnemyShip : AbstractEnemy
{
    public EnemyShip(int hp = 1, int score = 1, bool d = false, float c = 1.0f)
    {
        health = hp;
        scoreValue = score;
        isDead = false;
        hasDrop = d;
        chance = c;
    }

    public override void Init(int hp)
    {
        health = hp;
        isDead = false;
    }

    public override int takeDamage(int dmg)
    {
        if (dmg >= health)
        {
            isDead = true;
        }
        return health -= dmg;
    }

    public override int getScoreValue()
    {
        return scoreValue;
    }

    public void Shoot(GameObject proj, Vector3 pos, Quaternion rot, bool destroyOrb = false)
    {
        /*GameObject clone;
        clone = MonoBehaviour.Instantiate(proj, pos, rot) as GameObject;*/
        proj.transform.position = pos;
        proj.transform.rotation = rot;
        proj.GetComponent<ProjectileBehavior>().isFriendly = false;
        proj.GetComponent<ProjectileBehavior>().destroyOrb = destroyOrb;
        proj.SetActive(true);
    }

    public override void DropOnDeath(Vector3 pos, Quaternion rot, GameObject drop = null)
    {
        if (!hasDrop || (Random.Range(0.0f, 1.0f) <= 1.0f - chance))
            return;

        GameObject powerUp;
        if (drop == null)
            powerUp = GameObject.FindGameObjectWithTag("ShipPool").GetComponent<ShipPool>().SpawnRandomPowerUp();
        else
            powerUp = drop;
        //Debug.Log(powerUp);
        powerUp.transform.position = pos;
        powerUp.transform.rotation = rot;
        powerUp.gameObject.SetActive(true);
        /*drop.transform.position = pos;
        drop.transform.rotation = rot;
        drop.SetActive(true);*/
        //clone = MonoBehaviour.Instantiate(drop, pos, rot) as GameObject;
    }

    public override bool getDeathStatus()
    {
        return isDead;
    }

    public override bool isBoss()
    {
        return false;
    }

    /*public override void PlayExplosion(Vector3 pos, Quaternion rot)
    {
        if (explode == null)
            return;

        explode.transform.position = pos;
        explode.transform.rotation = rot;
        explode.SetActive(true);
        GameObject clone;
        clone = MonoBehaviour.Instantiate(explode, pos, rot) as GameObject;
    }*/
}

