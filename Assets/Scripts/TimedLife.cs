using UnityEngine;
using System.Collections;

public class TimedLife : MonoBehaviour {

    public float despawn = 15.0f;
	
	// Update is called once per frame
	void Update () {
        despawn -= Time.deltaTime;
        if (despawn < 0.0f)
            Destroy(gameObject);
	}
}
