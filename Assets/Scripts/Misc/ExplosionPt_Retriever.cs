using UnityEngine;
using System.Collections;

public class ExplosionPt_Retriever : MonoBehaviour {

    public GameObject[] RetrievePoints()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("Explosion_FX");
        return points;
    }
}
