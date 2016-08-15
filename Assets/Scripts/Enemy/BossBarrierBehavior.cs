using UnityEngine;
using System.Collections;

public class BossBarrierBehavior : MonoBehaviour
{
    public int hitPoints;
    public int scoreValue;
    public EnemyShip ES;
    public GameObject explosion;

    void Awake()
    {
        ES = new EnemyShip(hitPoints, scoreValue, explosion);
    }
}

