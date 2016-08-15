using UnityEngine;
using System.Collections;

public class BossShip : AbstractEnemy
{
    public BossShip(int hp = 20, int score = 1000, GameObject exp = null, GameObject d = null)
    {
        health = hp;
        scoreValue = score;
        explode = exp;
        drop = d;
    }

    public override int takeDamage(int dmg)
    {
        return health -= dmg;
    }

    public override int getScoreValue()
    {
        return scoreValue;
    }

    public void Shoot(GameObject proj, Vector3 pos, Quaternion rot, float speedChg = 0.0f, float angle = 0.0f)
    {
        GameObject clone;
        clone = MonoBehaviour.Instantiate(proj, pos, rot) as GameObject;
        clone.GetComponent<ProjectileBehavior>().isFriendly = false;
        clone.GetComponent<ProjectileBehavior>().angle = angle;
        clone.GetComponent<ProjectileBehavior>().speed += speedChg;
        clone.SetActive(true);
    }

    public void ShootLaser(GameObject laser, Vector3 pos, Quaternion rot, int angle = 0)
    {
        GameObject clone;
        clone = MonoBehaviour.Instantiate(laser, pos, rot) as GameObject;
        if (clone.GetComponent<ReflectiveLaserBehavior>() != null)
        {
            clone.GetComponent<ReflectiveLaserBehavior>().isFriendly = false;
            clone.GetComponent<ReflectiveLaserBehavior>().angle = angle;
        }
        else
        {
            clone.GetComponent<LaserBehavior>().isFriendly = false;
            clone.GetComponent<LaserBehavior>().angle = angle;
        }
        clone.SetActive(true);
    }

    public override void DropOnDeath(Vector3 pos, Quaternion rot)
    {
        if (drop == null)
            return;

        GameObject clone;
        clone = MonoBehaviour.Instantiate(drop, pos, rot) as GameObject;
    }

    public override bool isBoss()
    {
        return true;
    }

    public override void PlayExplosion(Vector3 pos, Quaternion rot)
    {
        GameObject clone;
        clone = MonoBehaviour.Instantiate(explode, pos, rot) as GameObject;
    }
}
