using UnityEngine;
using System.Collections;

public class DestroybyBoundary : MonoBehaviour {

	void OnTriggerExit(Collider other)
    {
        /*if (other.CompareTag("PlayerWeapon"))
            GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerProjectileList>().addWeapon(other.gameObject);*/

        if (!other.CompareTag("BossLaser"))
            other.gameObject.SetActive(false);
		    //Destroy (other.gameObject);
	}
}
