using UnityEngine;
using System.Collections;

public class Bullet : AbstractProjectile
{
    public Bullet( int dmg = 4, PlayerAttackType type = PlayerAttackType.proj)
    { damage = dmg; hitType = type; }
    /*public Bullet(float accel = 2.0f, bool hit = false, int dmg = 1, PlayerAttackType type = PlayerAttackType.proj)
    { acceleration = accel; isHit = hit; damage = dmg; hitType = PlayerAttackType.proj; }*/

    public override bool isMissile()
    {
        return false;
    }
}
