using UnityEngine;
using System.Collections;

public class Missile : AbstractProjectile
{
    float blastRadius;
    public Missile(float rad = 2.0f, int dmg = 10, PlayerAttackType type = PlayerAttackType.proj)
    { blastRadius = rad; damage = dmg; hitType = type; }
    /*public Bullet(float accel = 2.0f, bool hit = false, int dmg = 1, PlayerAttackType type = PlayerAttackType.proj)
    { acceleration = accel; isHit = hit; damage = dmg; hitType = PlayerAttackType.proj; }*/

    public override bool isMissile()
    {
        return true;
    }
}
