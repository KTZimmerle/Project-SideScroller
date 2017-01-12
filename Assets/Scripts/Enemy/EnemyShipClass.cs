using UnityEngine;
using System.Collections;

public class EnemyShip : AbstractEnemy
{
    public EnemyShip(int hp = 1, int score = 1) { health = hp; scoreValue = score; }

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
}

