using UnityEngine;
using System.Collections;

public class StraightMover : MonoBehaviour {

    public float speedMin;
    public float speedMax;
    public int hitPoints;
    public int scoreValue;
    public EnemyShip ES;
    public GameObject projectile;
    GameObject proj;
    public GameObject explosion;
    GameObject player;
    float seconds;
    bool hasShot;
    public float speed;

    // Use this for initialization
    void Awake()
    {
        //exp = Instantiate(explosion);
        proj = Instantiate(projectile);
        ES = new EnemyShip(hitPoints, scoreValue, false);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        GetComponent<Rigidbody>().velocity = transform.right * speed;
        GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(GetComponent<Rigidbody>().velocity.x * speedMin,
                                                         GetComponent<Rigidbody>().velocity.x * speedMax), 0.0f, 0.0f);
        player = GameObject.FindGameObjectWithTag("PlayerShip");
        seconds = 2.0f;
        hasShot = false;
    }

    void OnDisable()
    {
        ES.Init(hitPoints);
    }

    void Update()
    {
        seconds -= Time.deltaTime;
        if ((seconds) <= 0.0f && !hasShot && player != null)
        {
            hasShot = true;
            //ES.Shoot(projectile, transform.position, transform.rotation);
            ES.Shoot(proj, transform.position, transform.rotation);
        }
    }
}
