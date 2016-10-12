using UnityEngine;
using System.Collections;

public class AbstractEnemy {
    
    public AbstractEnemy() { health = 1; scoreValue = 0; isDead = false; hasDrop = false; }
    protected int health;
    protected int scoreValue;
    protected bool isDead;
    protected bool hasDrop;

    public virtual void Init(int hp)
    {

    }

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
        /*if (drop == null)
            return;

        drop.transform.position = pos;
        drop.transform.rotation = rot;
        drop.SetActive(true);*/
        //clone = MonoBehaviour.Instantiate(drop, pos, rot) as GameObject;
    }

    public virtual bool getDeathStatus()
    {
        return isDead;
    }

    public virtual bool isBoss()
    {
        return false;
    }
}
