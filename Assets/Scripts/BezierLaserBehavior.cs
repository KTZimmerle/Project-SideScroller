using UnityEngine;
using System.Collections;

public class BezierLaserBehavior : MonoBehaviour
{
    public Laser laser;
    public bool isFriendly;
    public bool isHit;
    public float speed;
    public float time;
    public int damage;
    float delay;
    protected float veloc;
    protected Vector3 direction;
    public GameObject BL;
    public float sharpness;
    //Vector3 DirResultant;
    protected GameController gameController;
    //PlayerController player;
    protected SpawnWaves bossStatus;

    public void Init()
    {
        Vector3 DirResultant = Vector3.zero;
        delay = 0.0f;
        time = 0.0f;
        GetComponent<Rigidbody>().velocity = new Vector3(speed, 0.0f, 0.0f);
        transform.GetChild(0).rotation = Quaternion.AngleAxis(0.0f, transform.forward);

        BL.GetComponent<BezierLaser>().Reset();
    }

    // Use this for initialization
    protected void Awake()
    {
        BL = transform.GetChild(1).gameObject;
        laser = new Laser(3, damage);
        GameObject target = GameObject.FindWithTag("GameController");
        if (target != null)
        {
            gameController = target.GetComponent<GameController>();
            bossStatus = target.GetComponent<SpawnWaves>();
        }

        /*if (GameObject.FindGameObjectWithTag("PlayerShip") != null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("PlayerShip");
            player = p.GetComponent<PlayerController>();
        }*/
    }

    protected void Start()
    {
        Init();
        /*Vector3 bottomleft = new Vector3(-9.5f, -4.5f, 0.0f);
        Vector3 topright = new Vector3(9.5f, 4.5f, 0.0f);*/
    }

    protected void OnEnable()
    {
        //BL = GameObject.FindGameObjectWithTag("PlayerLaser").GetComponent<BezierLaser>();
        BL.GetComponent<BezierLaser>().Reset(sharpness);
    }

    protected void OnDisable()
    {
        Init();
    }

    protected void Update()
    {

        /*if (time < 1.0f)
        {
            if (delay <= 0.0f)
            {
                time += Time.smoothDeltaTime;
                time = Mathf.Clamp(time, 0.0f, 1.0f);
                if (time >= 1.0f)
                    time = 1.0f;*/

                /*if (time < 1.0f)
                    transform.right = ;
                    //transform.LookAt(transform.localPosition + BL.GetComponent<BezierLaser>().GetDirection(time));*/

            /*}
            else
                delay -= Time.deltaTime;
        }*/
    }

    void FixedUpdate()
    {
        if (time < 1.0f)
        {
            time += Time.smoothDeltaTime;
            time = Mathf.Clamp(time, 0.0f, 1.0f);
            BL.GetComponent<BezierLaser>().GetDirection(time);
            BL.GetComponent<BezierLaser>().GetPoint(time);
            Vector3 direction = BL.GetComponent<BezierLaser>().GetDirection(time);
            GetComponent<Rigidbody>().velocity = (direction) * Time.smoothDeltaTime * speed;
            float angle;
            angle = Mathf.Atan2(GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.x) * Mathf.Rad2Deg;
            transform.GetChild(0).rotation = Quaternion.AngleAxis(angle, transform.forward);
        }
        //MoveLaser();
    }

    /*public void MoveLaser()
    {
        if (DirResultant.y >= 0.1f || DirResultant.y <= -0.1f)
        {

            if (DirResultant.y <= 0.0f)
                sharpness -= Time.deltaTime;
            else
                sharpness += Time.deltaTime;

            if (sharpness > 1.0f)
            {
                sharpness = 1.0f;
            }
            else if (sharpness < -1.0f)
            {
                sharpness = -1.0f;
            }
        }
        else
        {
            sharpness = (sharpness > 0.0f) ? Mathf.Abs(sharpness - Time.deltaTime) : -Mathf.Abs(sharpness - Time.deltaTime);
        }
    }*/

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!isFriendly)
            return;

        if (other.GetComponent<BossArmorBehavior>() != null)
        {
            isHit = true;
            gameObject.SetActive(false);
            return;
        }

        AbstractEnemy enemy = GetComponent<OnHitHandler>().OnHitHandle(other, gameController);

        if (enemy == null)
            return;

        if (enemy.getDeathStatus())
            return;

        isHit = true;
        if (enemy.takeDamage(laser.damage) <= 0)
        {
            GetComponent<OnHitHandler>().OnHitLogic(other, gameController, enemy);
            isHit = false;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }/**/
}
