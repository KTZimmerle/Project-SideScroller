using UnityEngine;
using System.Collections;

public class AbstractEnemy {
    
    public AbstractEnemy() { health = 1; scoreValue = 0; }
    protected int health;
    protected int scoreValue;

    public virtual int takeDamage(int dmg)
    {
        return health -= dmg;
    }

    public virtual int getScoreValue()
    {
        return scoreValue;
    }

    public virtual void DropOnDeath(GameObject drop, Vector3 pos, Quaternion rot)
    {
        if (drop == null)
            return;

        GameObject clone;
        clone = MonoBehaviour.Instantiate(drop, pos, rot) as GameObject;
    }

    public virtual bool isBoss()
    {
        return false;
    }
}
