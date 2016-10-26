using UnityEngine;
using System.Collections;

public class LaserBehavior : MonoBehaviour {

    protected struct laserHit
    {
        public Vector3 topT;
        public Vector3 topH;
        public Vector3 midT;
        public Vector3 midH;
        public Vector3 botT;
        public Vector3 botH;
    };

    public float speed;
    protected laserHit LasHit;
    public Laser laser;
    protected LineRenderer laserBeam;
    protected Rect gameBounds;
    PlayerController player;
    public bool isFriendly;
    public bool isHit;
    public bool isManipulable = false;
    protected bool growthPeaked;
    protected bool ownerAlive;
    protected int stopGrowth;
    protected GameController gameController;
    protected SpawnWaves bossStatus;
    protected float tailX = -0.0f;
    protected float headX = 0.0f;
    protected float tailY = -0.0f;
    protected float headY = 0.0f;
    protected float veloc;
    public int length = 3;
    float lasSpeed;
    public float width;
    protected float extend;
    protected float totalGrowth = 0.0f;
    protected int mask;
    public int angle;
    protected RaycastHit hit;
    protected Vector3[] points;
    protected Vector3 direction;

    public void Init()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        tailX = -0.0f;
        headX = 0.0f;
        tailY = -0.0f;
        headY = 0.0f;
        totalGrowth = 0.0f;
        stopGrowth = 1;
        growthPeaked = false;
        ownerAlive = true;
        for (int i = 0; i < 2; i++)
        {
            points[i] = new Vector3(0.0f, 0.0f, 0.0f);
        }
        laserBeam.SetPositions(points);
    }

    // Use this for initialization
    protected void Awake()
    {
        points = new Vector3[2];
        laser = new Laser(length);
        laserBeam = GetComponent<LineRenderer>();
        laserBeam.SetWidth(width, width);
        GameObject target = GameObject.FindWithTag("GameController");
        if (target != null)
        {
            gameController = target.GetComponent<GameController>();
            bossStatus = target.GetComponent<SpawnWaves>();
        }

        if (GameObject.FindGameObjectWithTag("PlayerShip") != null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("PlayerShip");
            player = p.GetComponent<PlayerController>();
        }

        lasSpeed = speed / 100;

        if (isFriendly)
            mask = 1 << 8 | 1 << 10 | 1 << 12 | 1 << 15;
        else
            mask = 1 << 11;
    }

    protected void Start()
    {
        Camera c = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector3 bottomleft = c.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, c.nearClipPlane + 2.0f));
        Vector3 topright = c.ScreenToWorldPoint(new Vector3(c.pixelWidth, c.pixelHeight, c.nearClipPlane + 2.0f));
        /*Vector3 bottomleft = new Vector3(-9.5f, -4.5f, 0.0f);
        Vector3 topright = new Vector3(9.5f, 4.5f, 0.0f);*/
        gameBounds = new Rect(bottomleft.x, bottomleft.y, topright.x * 2, topright.y * 2);
    }

    protected void OnEnable()
    {
        stopGrowth = 1;
        growthPeaked = false;
        ownerAlive = true;
        direction = new Vector3(1.0f * Mathf.Cos(Mathf.Deg2Rad * angle),
                                1.0f * Mathf.Sin(Mathf.Deg2Rad * angle), 0.0f);

        veloc = speed;
        if (angle == 0)
            extend = lasSpeed;
        else if (angle == 180)
            extend = -lasSpeed;
        else
            extend = (Mathf.Abs(lasSpeed * Mathf.Cos(Mathf.Deg2Rad * angle)) + Mathf.Abs(lasSpeed * Mathf.Sin(Mathf.Deg2Rad * angle))) / 2;

        /*if (isFriendly)
            gameController.GetComponent<PlayerProjectileList>().removeWeapon(gameObject);*/
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

    protected void LaserGrowth()
    {
        if (totalGrowth <= laser.getLength() && ownerAlive)
        {
            headX += extend * direction.normalized.x * stopGrowth * Time.deltaTime * 100;
            headY += extend * direction.normalized.y * stopGrowth * Time.deltaTime * 100;
            totalGrowth += Mathf.Abs(extend * Time.deltaTime * 100);
        }
        else
        {
            growthPeaked = true;
        }
    }

    protected void LaserShrink()
    {
        if (!growthPeaked)
            return;
        tailX += extend * direction.normalized.x * Time.deltaTime * 100;
        tailY += extend * direction.normalized.y * Time.deltaTime * 100;
        if (Mathf.Abs(headX - tailX) < 0.11f && Mathf.Abs(headY - tailY) < 0.11f)
        {
            /*if (isFriendly)
            {
                gameController.GetComponent<PlayerProjectileList>().addWeapon(gameObject);
            }*/
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

	protected void Update ()
    {
        if (!growthPeaked)
            LaserGrowth();

        stopGrowth = 1;
        if (isFriendly && gameController != null)
        {
            if (gameController.playerDied)
            {
                ownerAlive = false;
            }
        }
        else if (!isFriendly && bossStatus != null && GetComponent<BoxCollider>() != null)
        {
            if (!bossStatus.bossAlive)
            {
                ownerAlive = false;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            float offsetX = (i < 1) ? tailX : headX;
            float offsetY = (i < 1) ? tailY : headY;
            points[i] = new Vector3(offsetX, offsetY, 0.0f);
        }

        LasHit.topT = transform.position + points[0] + new Vector3(0.0f, (width - 0.05f) / 2, 0.0f);
        LasHit.topH = transform.position + points[1] + new Vector3(0.0f, (width - 0.05f) / 2, 0.0f);
        LasHit.midT = transform.position + points[0] + new Vector3(0.0f, 0.0f, 0.0f);
        LasHit.midH = transform.position + points[1] + new Vector3(0.0f, 0.0f, 0.0f);
        LasHit.botT = transform.position + points[0] + new Vector3(0.0f, -(width - 0.05f) / 2, 0.0f);
        LasHit.botH = transform.position + points[1] + new Vector3(0.0f, -(width - 0.05f) / 2, 0.0f);
        if (Physics.Linecast(LasHit.topT, LasHit.topH, out hit, mask, QueryTriggerInteraction.Collide) ||
            Physics.Linecast(LasHit.midT, LasHit.midH, out hit, mask, QueryTriggerInteraction.Collide) ||
            Physics.Linecast(LasHit.botT, LasHit.botH, out hit, mask, QueryTriggerInteraction.Collide))
        {
            HandleHit(hit.collider);
            if (!isFriendly)
                GetComponent<DestroyPlayer>().HandlePlayerHit(hit.collider);
            else
                stopGrowth = 0;
        }

        laserBeam.SetPositions(points);
    }

    void FixedUpdate()
    {
        if(growthPeaked)
            GetComponent<Rigidbody>().velocity = 
                    new Vector3(1.0f * Mathf.Cos(Mathf.Deg2Rad * angle),
                                1.0f * Mathf.Sin(Mathf.Deg2Rad * angle), 0.0f) * veloc;
        MoveLaser();
        isHit = false;
    }

    public void MoveLaser()
    {
        if (!isManipulable)
            return;

        if (!ownerAlive)
            return;
        
        Vector3 DirResultant = Vector3.zero;
        DirResultant.y = Input.GetAxis("PlayerShipV") * Time.deltaTime;
        DirResultant.x = Input.GetAxis("PlayerShipH") * Time.deltaTime;
        if (growthPeaked)
            DirResultant.x /= 2;
        
        if (transform.position.x + DirResultant.x < gameBounds.xMin)
        {
            DirResultant.x = 0;
            transform.position = new Vector3(gameBounds.xMin, transform.position.y, 0.0f);
        }

        if (transform.position.y + DirResultant.y < gameBounds.yMin ||
           transform.position.y + DirResultant.y > gameBounds.yMax)
        {
            DirResultant.y = 0;
            transform.position = (transform.position.y > 0.0f) ?
                                 new Vector3(transform.position.x, gameBounds.yMax, 0.0f) :
                                 new Vector3(transform.position.x, gameBounds.yMin, 0.0f);
        }

        if (growthPeaked)
            GetComponent<Rigidbody>().velocity = player.speed * DirResultant + GetComponent<Rigidbody>().velocity;
        else
            GetComponent<Rigidbody>().velocity = player.speed * DirResultant;

        if (isHit)
            GetComponent<Rigidbody>().velocity -= transform.right * speed;
    }

    protected virtual void HandleHit(Collider other)
    {
        if (!isFriendly)
            return;

        if (other.GetComponent<BossArmorBehavior>() != null)
        {
            isHit = true;
            LaserShrink();
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

        LaserShrink();
    }
}
