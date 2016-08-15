using UnityEngine;
using System.Collections;

public class ReflectHelper : MonoBehaviour {

    public GameObject BRlaser1;

    // Use this for initialization
    public void HelpReflectLaser(Vector3 pos, int angle)
    {
        GameObject clone = MonoBehaviour.Instantiate(BRlaser1, pos, transform.rotation) as GameObject;
        //clone.GetComponent < Physics.IgnoreCollision() > ();
        clone.GetComponent<ReflectiveLaserBehavior>().isFriendly = false;
        clone.GetComponent<ReflectiveLaserBehavior>().angle = angle;
        clone.GetComponent<Rigidbody>().velocity = Vector3.zero;
        clone.SetActive(true);
    }
}
