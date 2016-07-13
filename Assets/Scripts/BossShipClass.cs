using UnityEngine;
using System.Collections;

public class BossShip : AbstractEnemy
{
    public BossShip(int hp = 20, int score = 1000) { health = hp; scoreValue = score; }

    public override int takeDamage(int dmg)
    {
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

    public void ShootLaser(GameObject laser, Vector3 pos, Quaternion rot)
    {
        GameObject clone;
        clone = MonoBehaviour.Instantiate(laser, pos, rot) as GameObject;
        clone.GetComponent<LaserBehavior>().isFriendly = false;
    }

    public override void DropOnDeath(GameObject drop, Vector3 pos, Quaternion rot)
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
}
