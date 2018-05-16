using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectilePool : MonoBehaviour {

    int laserTracker = 0;
    const int PROJECTILE_LIMIT = 5;
    const int MISSILE_LIMIT = 1;
    const int ALTFIRE_LIMIT = 8;
    const int LASER_LIMIT = 60;
    List<GameObject> bullets;
    List<GameObject> altfires;
    List<GameObject> missiles;
    List<GameObject> lasers;
    public GameObject bullet;
    public GameObject missile;
    public GameObject altfire;
    public GameObject laser;


    public GameObject bossProj;
    public GameObject bossProjTwo;
    public GameObject bossLaser;
    public GameObject bossTwoProj;
    const int PROJ_ONE_CAP = 135;
    const int PROJ_TWO_CAP = 40;
    const int BOSS_LASER_CAP = 2;
    List<GameObject> projOne;
    List<GameObject> projOneBossTwo;
    List<GameObject> projTwo;
    List<GameObject> bossLasers;
    List<GameObject> bossTwoLasers;


    void Awake () {
        bullets = new List<GameObject>();
        altfires = new List<GameObject>();
        missiles = new List<GameObject>();
        lasers = new List<GameObject>();
        projOne = new List<GameObject>();
        projTwo = new List<GameObject>();
        bossLasers = new List<GameObject>();
        projOneBossTwo = new List<GameObject>();

        for (int i = 0; i < 30; i++)
        {
            if (bullets.Count < PROJECTILE_LIMIT)
                bullets.Add(Instantiate(bullet));

            if (missiles.Count < MISSILE_LIMIT)
                missiles.Add(Instantiate(missile));

            if (altfires.Count < ALTFIRE_LIMIT)
                altfires.Add(Instantiate(altfire));

            if (lasers.Count < LASER_LIMIT)
                lasers.Add(Instantiate(laser));
        }/**/

        for (int i = 0; i < PROJ_ONE_CAP; i++)
        {
            projOne.Add(Instantiate(bossProj));
            projOne[i].GetComponent<ProjectileBehavior>().isFriendly = false;
            projOneBossTwo.Add(Instantiate(bossTwoProj));
            projOneBossTwo[i].GetComponent<ProjectileBehavior>().isFriendly = false;

            if (projTwo.Count < PROJ_TWO_CAP)
            {
                projTwo.Add(Instantiate(bossProjTwo));
                projTwo[i].GetComponent<AltfireBehavior>().isFriendly = false;
            }

            if (bossLasers.Count < BOSS_LASER_CAP)
            {
                bossLasers.Add(Instantiate(bossLaser));
                bossLasers[i].GetComponent<LaserBehavior>().isFriendly = false;
            }
        }
    }

    /*public void addWeapon(GameObject b)
    {
        if (b.GetComponent<ProjectileBehavior>() != b.GetComponent<AltfireBehavior>() && b.GetComponent<ProjectileBehavior>() != null)
            bullets.Add(b);
        else if (b.GetComponent<AltfireBehavior>() != null)
            altfires.Add(b);
        else if (b.GetComponent<ProjectileBehavior>() != b.GetComponent<MissileBehavior>() && b.GetComponent<MissileBehavior>() != null)
            missiles.Add(b);
        else if (b.GetComponent<LaserBehavior>() != null)
            lasers.Add(b);
    }

    public void removeWeapon(GameObject b)
    {
        if (b.GetComponent<ProjectileBehavior>() != b.GetComponent<AltfireBehavior>() && b.GetComponent<ProjectileBehavior>() != null)
            bullets.Remove(b);
        else if (b.GetComponent<AltfireBehavior>() != null)
            altfires.Remove(b);
        else if (b.GetComponent<ProjectileBehavior>() != b.GetComponent<MissileBehavior>() && b.GetComponent<MissileBehavior>() != null)
            missiles.Remove(b);
        else if (b.GetComponent<LaserBehavior>() != null)
            lasers.Remove(b);
    }*/

    public int getBullets()
    {
        return bullets.Count;
    }

    public int getAltfires()
    {
        return altfires.Count;
    }

    public int getMissiles()
    {
        return missiles.Count;
    }

    public int getLasers()
    {
        return lasers.Count;
    }

    public GameObject RequestBullet()
    {
        for (int i = 0; i < getBullets(); i++)
        {
            if (!bullets[i].activeSelf)
                return bullets[i];
        }
        return null;
    }

    public GameObject RequestMissile()
    {
        for (int i = 0; i < getMissiles(); i++)
        {
            if (!missiles[i].activeSelf)
                return missiles[i];
        }
        return null;
    }

    public GameObject RequestAltfire(int start = 0)
    {
        for (int i = 0; i < getAltfires(); i++)
        {
            if (!altfires[i].activeSelf)
                return altfires[i];
        }
        return null;
    }

    public GameObject RequestLaser()
    {
        int index = laserTracker % getLasers();
        /*for (int i = 0; i < getLasers(); i++)
        {
            if (!lasers[i].activeSelf)
                return lasers[i];
        }*/
        if (!lasers[index].activeSelf)
        {
            laserTracker++;
            return lasers[index];
        }
        else
            return null;
    }
    
    public GameObject FireNextBullet(GameObject boss)
    {
        if (!boss.CompareTag("Boss"))
            return null;

        for (int i = 0; i < PROJ_ONE_CAP; i++)
        {
            if (!projOne[i].activeSelf)
                return projOne[i];
        }
        return null;
    }

    public GameObject FireNextBulletTwo(GameObject boss)
    {
        if (!boss.CompareTag("Boss"))
            return null;
        
        for (int i = 0; i < PROJ_ONE_CAP; i++)
        {
            if (!projOneBossTwo[i].activeSelf)
                return projOneBossTwo[i];
        }
        return null;
    }

    public GameObject FireNextBolt(GameObject boss)
    {
        if (!boss.CompareTag("Boss"))
            return null;

        for (int i = 0; i < PROJ_TWO_CAP; i++)
        {
            if (!projTwo[i].activeSelf)
                return projTwo[i];
        }
        return null;
    }

    public GameObject FireNextLaser(GameObject boss)
    {
        if (!boss.CompareTag("Boss"))
            return null;

        for (int i = 0; i < BOSS_LASER_CAP; i++)
        {
            if (!bossLasers[i].activeSelf)
                return bossLasers[i];
        }
        return null;
    }
}
