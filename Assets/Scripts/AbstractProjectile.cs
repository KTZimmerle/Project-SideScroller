using UnityEngine;
using System.Collections;

public class AbstractProjectile
{
    public int damage;
    public PlayerAttackType hitType; //From AttackType.cs - KTZ

    public virtual bool isMissile()
    {
        return false;
    }

}
