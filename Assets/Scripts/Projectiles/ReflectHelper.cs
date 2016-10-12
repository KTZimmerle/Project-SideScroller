using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReflectHelper : MonoBehaviour {

    public GameObject BRlaser1;
    List<GameObject> bossRefLasers;
    const int BOSS_RLASER_CAP = 28;

    void Awake()
    {
        bossRefLasers = new List<GameObject>(BOSS_RLASER_CAP);

        for(int i = 0; i < BOSS_RLASER_CAP; i++)
            if (bossRefLasers.Count < BOSS_RLASER_CAP)
            {
                bossRefLasers.Add(Instantiate(BRlaser1));
                bossRefLasers[i].GetComponent<ReflectiveLaserBehavior>().isFriendly = false;
            }
    }

    // Use this for initialization
    public void HelpReflectLaser(Vector3 pos, int angle)
    {
        //GameObject clone = MonoBehaviour.Instantiate(BRlaser1, pos, transform.rotation) as GameObject;
        //clone.GetComponent < Physics.IgnoreCollision() > ();
        GameObject RLas = FireNextRLaser();
        //RLas.GetComponent<ReflectiveLaserBehavior>().isFriendly = false;
        RLas.GetComponent<ReflectiveLaserBehavior>().angle = angle;
        RLas.transform.position = pos;
        //RLas.GetComponent<Rigidbody>().velocity = Vector3.zero;
        RLas.SetActive(true);
    }
    
    GameObject FireNextRLaser()
    {
        for (int i = 0; i < bossRefLasers.Count; i++)
        {;
            if (!bossRefLasers[i].activeSelf)
                return bossRefLasers[i];
        }
        return null;
    }
}
