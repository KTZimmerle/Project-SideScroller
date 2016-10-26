using UnityEngine;
using System.Collections;

public class DestroybyBoundary : MonoBehaviour {

    Camera c;
    SpawnWaves sw;

    void Awake()
    {
        sw = GameObject.FindGameObjectWithTag("GameController").GetComponent<SpawnWaves>();
        c = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (c.aspect >= 1.5f)
        {
            transform.localScale = new Vector3(20.0f, 12.0f, 1.0f);
            c.transform.localPosition = new Vector3(0.0f, 0.0f, -10.0f);
            c.nearClipPlane = 16;
            c.farClipPlane = 35;
        }
        else
        {
            transform.localScale = new Vector3(19.0f, 13.0f, 1.0f);
            c.transform.localPosition = new Vector3(0.0f, 0.0f, -15.0f);
            c.nearClipPlane = 21;
            c.farClipPlane = 50;
        }
    }

    void OnTriggerExit(Collider other)
    {
        /*if (other.CompareTag("PlayerWeapon"))
            GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerProjectileList>().addWeapon(other.gameObject);*/

        if (!other.CompareTag("BossLaser"))
            other.gameObject.SetActive(false);

        if(other.CompareTag("Hazard"))
            sw.decrementEnemyCount();
		    //Destroy (other.gameObject);
	}
}
