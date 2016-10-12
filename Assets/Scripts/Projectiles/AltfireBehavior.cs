using UnityEngine;
using System.Collections;

public class AltfireBehavior : ProjectileBehavior {

    public float rotation;
    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        //transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotation) * transform.rotation;
    }

    /*void Start()
    {
        if(isFriendly)
            gameController.GetComponent<PlayerProjectileList>().addWeapon(gameObject);
    }*/

    void OnEnable()
    {
        base.OnEnable();
        //transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotation) * Quaternion.Euler(0.0f, 0.0f, 0.0f);
        GetComponent<Rigidbody>().velocity = transform.right * speed;
    }

    void FixedUpdate ()
    {
        if (isFriendly)
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y, 0.0f) * acceleration;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!isFriendly || isHit)
            return;

        if (other.GetComponent<BossArmorBehavior>() != null)
        {
            //gameController.GetComponent<PlayerProjectileList>().addWeapon(gameObject);
            //Destroy(gameObject);
            gameObject.SetActive(false);
            return;
        }
        
        enemy = GetComponent<OnHitHandler>().OnHitHandle(other, gameController);

        if (enemy == null)
            return;

        if (enemy.getDeathStatus())
            return;

        isHit = true;
        if (enemy.takeDamage(bullet.damage) <= 0)
        {
            GetComponent<OnHitHandler>().OnHitLogic(other, gameController, enemy);
        }

        //gameController.GetComponent<PlayerProjectileList>().addWeapon(gameObject);
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
