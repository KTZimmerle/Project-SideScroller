using UnityEngine;
using System.Collections;

public class RotatorMover : MonoBehaviour {

    public int hitPoints;
    public int scoreValue;
    public EnemyShip ES;
    public GameObject projectile;
    GameObject proj;
    Collider player;
    public GameObject explosion;
    float seconds;
    float secondsToShoot;
    float secondsToScan;
    float rotSpeed;
    int minSpeed;
    int maxSpeed;
    float chance;

    void Awake()
    {
        chance = 0.05f;
        player = null;
        proj = Instantiate(projectile);
        ES = new EnemyShip(hitPoints, scoreValue, true, chance);
        minSpeed = 1;
        maxSpeed = 4;
        rotSpeed = 100.0f;
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        seconds = 3.0f;
        secondsToShoot = 2.5f;
        secondsToScan = 1.0f;
        float speed = Random.Range(minSpeed, maxSpeed);
        if (GetComponent<Rigidbody>().transform.position.y < 0.0f)
            GetComponent<Rigidbody>().velocity = transform.up * speed;
        else
            GetComponent<Rigidbody>().velocity = -transform.up * speed;
    }

    void OnDisable()
    {
        ES.Init(hitPoints);
        player = null;
    }

    void Update () 
    {
        if (seconds > 0.0f)
        {
            transform.Rotate(0.0f, 0.0f, Time.deltaTime * 1000, Space.Self);
            seconds -= Time.deltaTime;
        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            secondsToScan -= Time.deltaTime;
        }

        //find a player ship to target and lock on to
        if (player != null)
        {

            Vector3 targetPos = player.transform.position;
            targetPos.z = 0.0f;
            transform.right = targetPos - transform.position;


            if (secondsToShoot < 0.0f)
            {
                foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
                {
                    if(ps.CompareTag("Muzzle_FX"))
                        ps.Play(false);
                }
                ES.Shoot(proj, transform.position, Quaternion.Euler(0.0f, 0.0f, 180.0f) * transform.rotation, true);
                //ES.Shoot(projectile, transform.position, Quaternion.Euler(0.0f, 0.0f, 180.0f) * transform.rotation);
                secondsToShoot = 2.5f;
            }
            else
                secondsToShoot -= Time.deltaTime;

            if (secondsToShoot < 2.0f)
                PlayEffects();
            if (!player.gameObject.activeSelf)
                player = null;
        }
        else //keep searching for a new player ship
        {
            if (GetComponent<ParticleSystem>().isPlaying)
                PauseEffects();
            if(secondsToScan < 0.0f)
            {
                Collider[] search;
                secondsToScan = 1.0f;
                search = Physics.OverlapSphere(transform.position, 25.0f, 1 << 11, QueryTriggerInteraction.Collide);
                if(search.Length > 0)
                    player = search[0];
            }
        }
    }

    void PlayEffects()
    {
        if (GetComponentsInChildren<ParticleSystem>()[2].isPlaying &&
            GetComponent<ParticleSystem>().isPlaying)
            return;

        //GetComponent<ParticleSystem>().Play(false);
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            if (!ps.CompareTag("Muzzle_FX"))
                ps.Play(false);
        }
    }

    void PauseEffects()
    {
        if (!GetComponentsInChildren<ParticleSystem>()[2].isPlaying &&
            !GetComponent<ParticleSystem>().isPlaying)
            return;

        //GetComponent<ParticleSystem>().Pause(false);
        GetComponent<ParticleSystem>().Clear(false);
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            if (!ps.CompareTag("Muzzle_FX"))
                ps.Pause(false);
        }
    }
}
