using UnityEngine;
using System.Collections;

public class Laser : AbstractProjectile
{
    int length;
    public Laser(int len = 3, int dmg = 1, PlayerAttackType type = PlayerAttackType.laser)
    { length = len; damage = dmg; hitType = type; }
    /*public Bullet(float accel = 2.0f, bool hit = false, int dmg = 1, PlayerAttackType type = PlayerAttackType.proj)
    { acceleration = accel; isHit = hit; damage = dmg; hitType = PlayerAttackType.proj; }*/

    public int getLength()
    {
        return length;
    }

    public override bool isMissile()
    {
        return false;
    }
}
