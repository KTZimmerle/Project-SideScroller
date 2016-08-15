using UnityEngine;
using System.Collections;

public class WavyMover : MonoBehaviour {

    float theta;
    public int hitPoints;
    public int scoreValue;
    public EnemyShip ES;
    Rigidbody ESVelocity;
    public GameObject explosion;
    public float speed;
    public GameObject drop;

    // Use this for initialization
    void Awake()
    {
        speed *= 50;
        theta = 0.0f;
        ES = new EnemyShip(hitPoints, scoreValue, explosion, drop);
        ESVelocity = GetComponent<Rigidbody>();
    }
	
	void FixedUpdate () {
        theta += 0.025f;
        ESVelocity.velocity = new Vector3(1.0f, Mathf.Cos(theta), 0.0f) * speed * Time.deltaTime;
    }
}
