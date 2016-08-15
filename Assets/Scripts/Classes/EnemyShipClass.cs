using UnityEngine;
using System.Collections;

public class EnemyShip : AbstractEnemy
{
    public EnemyShip(int hp = 1, int score = 1, GameObject exp = null, GameObject d = null)
    {
        health = hp;
        scoreValue = score;
        isDead = false;
        explode = exp;
        drop = d;
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

    public void Shoot(GameObject proj, Vector3 pos, Quaternion rot)
    {
        GameObject clone;
        clone = MonoBehaviour.Instantiate(proj, pos, rot) as GameObject;
        clone.GetComponent<ProjectileBehavior>().isFriendly = false;
    }

    public override void DropOnDeath(Vector3 pos, Quaternion rot)
    {
        if (drop == null)
            return;
        
        GameObject clone;
        clone = MonoBehaviour.Instantiate(drop, pos, rot) as GameObject;
    }

    public override bool getDeathStatus()
    {
        return isDead;
    }

    public override bool isBoss()
    {
        return false;
    }

    public override void PlayExplosion(Vector3 pos, Quaternion rot)
    {
        if (explode == null)
            return;

        GameObject clone;
        clone = MonoBehaviour.Instantiate(explode, pos, rot) as GameObject;
    }
}

