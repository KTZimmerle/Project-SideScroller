using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerProjectileList : MonoBehaviour {

    const int PROJECTILE_LIMIT = 2;
    public int MISSILE_LIMIT = 1;
    public int ALTFIRE_LIMIT = 4;
    public int LASER_LIMIT = 3;
    List<GameObject> bullets;
    List<GameObject> altfires;
    List<GameObject> missiles;
    List<GameObject> lasers;
    public GameObject bullet;
    public GameObject missile;
    public GameObject altfire;
    public GameObject laser;

    // Use this for initialization
    void Awake () {
        bullets = new List<GameObject>();
        altfires = new List<GameObject>();
        missiles = new List<GameObject>();
        lasers = new List<GameObject>();

        /*for (int i = 0; i < 4; i++)
        {
            if (bullets.Count < PROJECTILE_LIMIT)
                bullets.Add(Instantiate(bullet));

            if (missiles.Count < MISSILE_LIMIT)
                missiles.Add(Instantiate(missile));

            if (altfires.Count < ALTFIRE_LIMIT)
                altfires.Add(Instantiate(altfire));

            if (lasers.Count < LASER_LIMIT)
                lasers.Add(Instantiate(laser));
        }*/
    }

    public void addBullet(GameObject b)
    {
        bullets.Add(b);
    }

    public void addAltFire(GameObject b)
    {
        altfires.Add(b);
    }

    public void addMissile(GameObject b)
    {
        missiles.Add(b);
    }

    public void addLaser(GameObject b)
    {
        lasers.Add(b);
    }

    public void removeBullet(GameObject b)
    {
        bullets.Remove(b);
    }

    public void removeAltFire(GameObject b)
    {
        altfires.Remove(b);
    }

    public void removeMissile(GameObject b)
    {
        missiles.Remove(b);
    }

    public void removeLaser(GameObject b)
    {
        lasers.Remove(b);
    }

    public int getBullets()
    {
        return bullets.Count;
    }

    public int getAltFires()
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

}
