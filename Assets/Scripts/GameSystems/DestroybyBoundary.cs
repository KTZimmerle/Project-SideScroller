using UnityEngine;
using System.Collections;

public class DestroybyBoundary : MonoBehaviour {

	void OnTriggerExit(Collider other)
	{
        if (other.CompareTag("PlayerProjectile"))
            if(other.GetComponent<ProjectileBehavior>().isFriendly)
                GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerProjectileList>().removeBullet(other.gameObject);

        if (other.CompareTag("MissileProjectile"))
            if (other.GetComponent<MissileBehavior>().isFriendly)
                GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerProjectileList>().removeMissile(other.gameObject);

        if (other.CompareTag("AltfireProjectile"))
            if (other.GetComponent<AltfireBehavior>().isFriendly)
                GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerProjectileList>().removeAltFire(other.gameObject);

        if (other.CompareTag("PlayerLaser"))
            if (other.GetComponent<LaserBehavior>().isFriendly)
                GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerProjectileList>().removeLaser(other.gameObject);

        if (!other.CompareTag("BossLaser"))
		    Destroy (other.gameObject);
	}
}
