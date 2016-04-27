using UnityEngine;
using System.Collections;

public class ProjectileBehavior : MonoBehaviour {

	public float acceleration;
    public bool isHit = false;
    public int damage;
    public PlayerAttackType hitType; //From AttackType.cs - KTZ

    void Start()
    {
        hitType = PlayerAttackType.proj;
    }

	void FixedUpdate()
	{
        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x + acceleration, GetComponent<Rigidbody>().velocity.y, 0.0f);
	}

    void OnTriggerEnter(Collider other)
    {

    }
}
