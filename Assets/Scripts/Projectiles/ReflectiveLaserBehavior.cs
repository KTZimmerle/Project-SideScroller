using UnityEngine;
using System.Collections;

public class ReflectiveLaserBehavior : LaserBehavior {

    public GameObject reflectHelper;
    bool laserReflected;

    // Use this for initialization
    void Awake ()
    {
        /*GetComponent<Rigidbody>().velocity = new Vector3(1.0f * Mathf.Cos(Mathf.Deg2Rad * angle),
                                                         1.0f * Mathf.Sin(Mathf.Deg2Rad * angle), 0.0f);*/
        base.Awake();
        tailX = 0.0f;
        headY = 0.0f;
        laserReflected = false;
    }

    void Update()
    {
        base.Update();
        //Physics.SphereCast(LasHit.midH, 0.1f, transform.up, out hit, 0.0f, 1 << 14, QueryTriggerInteraction.Collide);
        if (!laserReflected && (Physics.Linecast(LasHit.topT, LasHit.topH, out hit, 1 << 14, QueryTriggerInteraction.Collide) ||
            Physics.Linecast(LasHit.midT, LasHit.midH, out hit, 1 << 14, QueryTriggerInteraction.Collide) ||
            Physics.Linecast(LasHit.botT, LasHit.botH, out hit, 1 << 14, QueryTriggerInteraction.Collide)))
        {
            ReflectLaser(hit.collider);
        }

        if (laserReflected)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            veloc = 0.0f;
            stopGrowth = 0;
            LaserShrink();
        }

    }

    void ReflectLaser(Collider ignore)
    {
        if (laserReflected || totalGrowth < 0.5)
            return;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        int newAngle = (angle < 0.0f) ? -angle : 360 - angle;
        laserReflected = true;
        stopGrowth = 0;
        reflectHelper.GetComponent<ReflectHelper>().HelpReflectLaser(transform.position + points[1], newAngle);
    }
}
