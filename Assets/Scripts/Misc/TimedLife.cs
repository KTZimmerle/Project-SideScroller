using UnityEngine;
using System.Collections;

public class TimedLife : MonoBehaviour {

    public float despawn = 5.0f;
    float timeLeft;

    void OnEnable()
    {
        timeLeft = despawn;
    }
	// Update is called once per frame
	void Update () {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0.0f)
        {
            //GameObject.FindGameObjectWithTag("Helper").GetComponent<ReflectHelper>().AddBossRLaser(gameObject);
            gameObject.SetActive(false);
        }
            //Destroy(gameObject);
	}
}
