using UnityEngine;
using System.Collections;

public class WavyMover : MonoBehaviour {

    float theta;
    public int hitPoints;
    public int scoreValue;
    public EnemyShip ES;
    Rigidbody ESVelocity;
    //public GameObject explosion;
    //GameObject exp;
    public float speed;
    public GameObject drop;

    // Use this for initialization
    void Awake()
    {
        //exp = Instantiate(explosion);
        speed *= 50;
        ES = new EnemyShip(hitPoints, scoreValue, true);
        ESVelocity = GetComponent<Rigidbody>();
        gameObject.SetActive(false);
    }
	
	void FixedUpdate ()
    {
        theta += 0.025f;
        ESVelocity.velocity = new Vector3(1.0f, Mathf.Cos(theta), 0.0f) * speed * Time.deltaTime;
    }

    void OnEnable()
    {
        theta = 0.0f;
    }

    void OnDisable()
    {
        ES.Init(hitPoints);
    }
}
