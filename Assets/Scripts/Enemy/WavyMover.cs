using UnityEngine;
using System.Collections;

public class WavyMover : Mover {

    float theta;
    public int hitPoints;
    public int scoreValue;
    public EnemyShip ES;
    Rigidbody ESVelocity;

    // Use this for initialization
    protected override void Awake()
    {
        theta = 0.0f;
        ES = new EnemyShip(hitPoints, scoreValue);
        ESVelocity = GetComponent<Rigidbody>();
    }
	
	void FixedUpdate () {
        theta += 0.025f;
        ESVelocity.velocity = new Vector3(1.0f, Mathf.Cos(theta), 0.0f) * speed;
    }
}
