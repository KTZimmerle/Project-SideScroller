using UnityEngine;
using System.Collections;

public abstract class AbstractEnemy {
    
    public AbstractEnemy() { health = 1; scoreValue = 0; }
    protected int health;
    protected int scoreValue;

    protected virtual void takeDamage(int dmg)
    {
        health =- dmg;
    }

    public virtual int getScoreValue()
    {
        return scoreValue;
    }   
}
