using UnityEngine;
using System.Collections;

public class HazardSpeed : MonoBehaviour {

	public float speedMin;
	public float speedMax;
	// Use this for initialization
	void Start () 
	{
		GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(GetComponent<Rigidbody>().velocity.x*speedMin, 
		                                              GetComponent<Rigidbody>().velocity.x*speedMax), 
		                                 0.0f, 0.0f);
	}
}
