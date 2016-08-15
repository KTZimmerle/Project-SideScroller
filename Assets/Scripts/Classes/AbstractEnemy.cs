using UnityEngine;
using System.Collections;

public class AbstractEnemy {
    
    public AbstractEnemy() { health = 1; scoreValue = 0; isDead = false; explode = null; drop = null; }
    protected int health;
    protected int scoreValue;
    protected bool isDead;
    protected GameObject explode;
    protected GameObject drop;

    public virtual int takeDamage(int dmg)
    {
        if (dmg >= health)
            isDead = true;
        return health -= dmg;
    }

    public virtual int getScoreValue()
    {
        return scoreValue;
    }

    public virtual void DropOnDeath(Vector3 pos, Quaternion rot)
    {
        if (drop == null)
            return;

        GameObject clone;
        clone = MonoBehaviour.Instantiate(drop, pos, rot) as GameObject;
    }

    public virtual bool getDeathStatus()
    {
        return isDead;
    }

    public virtual bool isBoss()
    {
        return false;
    }

    public virtual void PlayExplosion(Vector3 pos, Quaternion rot)
    { }
}
