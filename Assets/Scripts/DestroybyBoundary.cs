using UnityEngine;
using System.Collections;

public class DestroybyBoundary : MonoBehaviour {

	void OnTriggerExit(Collider other)
	{
        if(!other.CompareTag("BossLaser"))
		    Destroy (other.gameObject);
	}
}
