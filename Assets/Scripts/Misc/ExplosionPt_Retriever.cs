using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionPt_Retriever : MonoBehaviour {

    public List<GameObject> RetrievePoints()
    {
        List<GameObject> points = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (string.Compare(child.tag, "Explosion_FX") == 0)
            {
                points.Add(child.gameObject);
            }
        }
        //points = GameObject.FindGameObjectsWithTag("Explosion_FX");
        return points;
    }
}
