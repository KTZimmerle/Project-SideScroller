using UnityEngine;
using System.Collections;

public class MissileBehavior : ProjectileBehavior {

    GameObject[] targets;
    GameObject closestTarget;
    public float veloc;

    void Start()
    {
        closestTarget = null;
        Rigidbody rb = GetComponent<Rigidbody>();
        veloc = GetComponent<Mover>().speed;
        hitType = PlayerAttackType.missile;
    }

    void FixedUpdate()
    {
        //find the closest enemy
        if (closestTarget != null)
        {
            Vector3 targetPos = closestTarget.transform.position;
            targetPos.z = 0.0f;
            transform.right = targetPos - transform.position;
        }
        else //keep searching the closest enemy available
        {
            /*rb.rotation =
                   Quaternion.Euler(GetComponent<Rigidbody>().rotation.eulerAngles + new Vector3(0.0f, 0.0f, rotaSpeed));*/
            targets = GameObject.FindGameObjectsWithTag("Hazard");
            float dist = Mathf.Infinity;
            Vector3 pos = transform.position;
            foreach (GameObject potenTarget in targets)
            {
                Vector3 difference = potenTarget.transform.position - pos;
                float currentDist = difference.sqrMagnitude;
                if (currentDist < dist)
                {
                    closestTarget = potenTarget;
                    dist = currentDist;
                }
            }
        }

        //GetComponent<Rigidbody>().velocity = new Vector3(transform.right, GetComponent<Rigidbody>().velocity.y, 0.0f);
        GetComponent<Rigidbody>().velocity = transform.right * veloc;
	}
}
