using UnityEngine;
using System.Collections;

public class BossShip : AbstractEnemy
{
    public BossShip(int hp = 20, int score = 1000, bool d = false)
    {
        health = hp;
        scoreValue = score;
        hasDrop = d;
    }

    public override void Init(int hp)
    {
        health = hp;
        isDead = false;
    }

    public override int takeDamage(int dmg)
    {
        return health -= dmg;
    }

    public override int getScoreValue()
    {
        return scoreValue;
    }

    public void Shoot(GameObject proj, Vector3 pos, Quaternion rot, float speedChg = 0.0f, float angle = 0.0f)
    {
        /*GameObject clone;
        clone = MonoBehaviour.Instantiate(proj, pos, rot) as GameObject;*/
        proj.GetComponent<ProjectileBehavior>().isFriendly = false;
        proj.GetComponent<ProjectileBehavior>().angle = angle;
        proj.GetComponent<ProjectileBehavior>().veloc += speedChg;
        proj.transform.position = pos;
        proj.transform.rotation = rot;
        proj.SetActive(true);
    }

    public void ShootLaser(GameObject laser, Vector3 pos, Quaternion rot, int angle = 0)
    {
        /*GameObject clone;
        clone = MonoBehaviour.Instantiate(laser, pos, rot) as GameObject;*/
        /*if (laser.GetComponent<ReflectiveLaserBehavior>() != null)
        {
            laser.GetComponent<ReflectiveLaserBehavior>().isFriendly = false;
            laser.GetComponent<ReflectiveLaserBehavior>().angle = angle;
        }
        else
        {
            laser.GetComponent<LaserBehavior>().isFriendly = false;
            laser.GetComponent<LaserBehavior>().angle = angle;
        }*/
        laser.GetComponent<LaserBehavior>().isFriendly = false;
        laser.GetComponent<LaserBehavior>().angle = angle;
        laser.transform.position = pos;
        laser.transform.rotation = rot;
        laser.SetActive(true);
    }

    public void ShootRLaser(Vector3 pos, int angle = 0)
    {
        GameObject reflectHelper = GameObject.FindGameObjectWithTag("Helper");
        reflectHelper.GetComponent<ReflectHelper>().HelpReflectLaser(pos, angle);
    }

    public override void DropOnDeath(Vector3 pos, Quaternion rot)
    {
        /*if (drop == null)
            return;
        
        drop.transform.position = pos;
        drop.transform.rotation = rot;
        drop.SetActive(true);*/
        //clone = MonoBehaviour.Instantiate(drop, pos, rot) as GameObject;
    }

    public override bool isBoss()
    {
        return true;
    }

    /*public override void PlayExplosion(Vector3 pos, Quaternion rot)
    {
        GameObject clone;
        clone = MonoBehaviour.Instantiate(explode, pos, rot) as GameObject;
    }*/
}
