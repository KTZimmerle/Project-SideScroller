using UnityEngine;
using System.Collections;

public class BezierLaserBehavior : MonoBehaviour
{
    public Laser laser;
    public bool isFriendly;
    public bool isHit;
    public float speed;
    public float time;
    float delay;
    protected float veloc;
    public int length = 3;
    public float width;
    protected float extend;
    protected float totalGrowth = 0.0f;
    protected Vector3[] points;
    protected Vector3 direction;
    public BezierLaser BL;
    public float sharpness = 0.5f;
    Vector3 DirResultant;
    protected GameController gameController;
    PlayerController player;
    protected SpawnWaves bossStatus;

    public void Init()
    {
        Vector3 DirResultant = Vector3.zero;
        delay = 0.0f;
        time = 0.0f;
        GetComponent<Rigidbody>().velocity = new Vector3(speed, 0.0f, 0.0f);
        
    }

    // Use this for initialization
    protected void Awake()
    {
        /*GameObject target = GameObject.FindWithTag("GameController");
        if (target != null)
        {
            gameController = target.GetComponent<GameController>();
            bossStatus = target.GetComponent<SpawnWaves>();
        }

        if (GameObject.FindGameObjectWithTag("PlayerShip") != null)
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
        BL.Reset(sharpness);
    }

    protected void OnDisable()
    {
        Init();
    }

    /*protected IEnumerator LaserGrowth()
    {
        while (totalGrowth <= laser.getLength() && ownerAlive)
        {
            headX += extend * direction.normalized.x * stopGrowth;
            headY += extend * direction.normalized.y * stopGrowth;
            totalGrowth += Mathf.Abs(extend);
            yield return new WaitForSeconds(0.001f);
        }
        isDone = true;
        veloc = speed;
    }*/

    /*protected void LaserGrowth()
    {
        
    }

    protected void LaserShrink()
    {
        
    }*/

    protected void Update()
    {

        if(time < 1.0f)
        {
            if (delay <= 0.0f)
            {
                time += Time.deltaTime;
                if (time >= 1.0f)
                    time = 1.0f;
            }
            else
                delay -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        DirResultant.y = Input.GetAxis("PlayerShipV") * Time.deltaTime;
        BL.GetDirection(time);
        BL.GetPoint(time);
        //Debug.Log(BL.GetDirection(time));
        GetComponent<Rigidbody>().velocity = (BL.GetDirection(time) ) * Time.deltaTime * speed;
        //MoveLaser();
    }

    public void MoveLaser()
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
    }

    /*protected virtual void HandleHit(Collider other)
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
    }*/
}
