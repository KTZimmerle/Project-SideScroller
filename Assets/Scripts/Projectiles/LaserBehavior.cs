using UnityEngine;
using System.Collections;

public class LaserBehavior : MonoBehaviour {

    Laser laser;
    LineRenderer laserBeam;
    BoxCollider laserHit;
    Rect gameBounds;
    int laserPoints;
    PlayerController player;
    public bool isFriendly;
    public bool isHit;
    protected GameController gameController;
    protected AbstractEnemy enemy;
    protected CircularMover E1;
    protected RotatorMover E2;
    protected StraightMover E3;
    protected TrackingMover E4;
    protected WavyMover EP;
    protected FirstBossRoutine BE;
    protected BossBarrierBehavior Barrier;
    float tailX = -0.25f;
    float headX = 0.25f;
    float tailY = -0.0f;
    float headY = 0.0f;
    float veloc;
    public int length = 3;
    public float Xspeed = 0.25f;
    public float Yspeed = 0.0f;
    float extend;
    float direction;
    float offsetX;
    float offsetY;
    float totalGrowth = 0.5f;
    public float slowDown = 10.0f;

    // Use this for initialization
    void Awake ()
    {
        GameObject target = GameObject.FindWithTag("GameController");
        if (target != null)
            gameController = target.GetComponent<GameController>();

        if (GameObject.FindGameObjectWithTag("PlayerShip") != null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("PlayerShip");
            player = p.GetComponent<PlayerController>();
        }

        veloc = 0.0f;
        extend += 0.05f;
        direction = GetComponent<Mover>().speed / Mathf.Abs(GetComponent<Mover>().speed);
        tailX *= direction;
        headX *= direction;
        laser = new Laser(length);
        laserBeam = GetComponent<LineRenderer>();
        laserHit = GetComponent<BoxCollider>();
        Vector3 bottomleft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 topright = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));
        gameBounds = new Rect(bottomleft.x, bottomleft.y, topright.x - bottomleft.x, topright.y - bottomleft.y);
        StartCoroutine(LaserGrowth());
    }

    IEnumerator LaserGrowth()
    {
        while (totalGrowth < laser.getLength())
        {
            headX += extend * 2 * direction;
            //tailX -= extend;
            laserHit.size = new Vector3(laserHit.size.x + extend * 2, laserHit.size.y, laserHit.size.z);
            totalGrowth += extend * 2;
            yield return new WaitForSeconds(0.001f);
        }
        veloc = GetComponent<Mover>().speed;
    }

    void LaserShrink()
    {
        headX -= extend * 2;
        laserHit.size = new Vector3(laserHit.size.x - extend * 2, laserHit.size.y, laserHit.size.z);
        if (headX - tailX < 0.1f)
            Destroy(gameObject);
    }

	void Update ()
    {
        Vector3[] points = new Vector3[2];
        for (int i = 0; i < 2; i++)
        {
            float offsetX = (i < 1) ? tailX : headX;
            float offsetY = (i < 1) ? tailY : headY;
            points[i] = new Vector3(Xspeed + offsetX, Yspeed + offsetY, 0.0f);
        }

        laserBeam.SetPositions(points);
        laserHit.center = new Vector3((points[0].x + points[1].x) / 2, (points[0].y + points[1].y) / 2, 0.0f);
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = transform.right * veloc;
        MoveLaser();
        isHit = false;
    }

    public void MoveLaser()
    {
        if (!isFriendly)
            return;

        Vector3 DirResultant = Vector3.zero;
        DirResultant.y = Input.GetAxis("PlayerShipV") * Time.deltaTime;
        DirResultant.x = Input.GetAxis("PlayerShipH") * Time.deltaTime;
        GetComponent<Rigidbody>().velocity = player.speed * DirResultant + transform.right * veloc;
        if (isHit)
            GetComponent<Rigidbody>().velocity -= transform.right * slowDown;
        Collider range = GetComponent<Collider>();
        GetComponent<Rigidbody>().position = new Vector3
            (GetComponent<Rigidbody>().position.x,
             Mathf.Clamp(GetComponent<Rigidbody>().position.y, gameBounds.yMin + range.bounds.size.y * 9.0f, gameBounds.yMax - range.bounds.size.y * 7.0f),
             0.0f);
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (!isFriendly)
            return;

        if (other.GetComponent<CircularMover>() != null)
        {
            E1 = other.GetComponent<CircularMover>();
            enemy = E1.ES;
        }
        else if (other.GetComponent<RotatorMover>() != null)
        {
            E2 = other.GetComponent<RotatorMover>();
            enemy = E2.ES;
        }
        else if (other.GetComponent<StraightMover>() != null)
        {
            E3 = other.GetComponent<StraightMover>();
            enemy = E3.ES;
        }
        else if (other.GetComponent<TrackingMover>() != null)
        {
            E4 = other.GetComponent<TrackingMover>();
            enemy = E4.ES;
        }
        else if (other.GetComponent<WavyMover>() != null)
        {
            EP = other.GetComponent<WavyMover>();
            enemy = EP.ES;
        }
        else if (other.GetComponent<FirstBossRoutine>() != null)
        {
            BE = other.GetComponent<FirstBossRoutine>();
            enemy = BE.BE;
        }
        else if (other.GetComponent<BossBarrierBehavior>() != null)
        {
            Barrier = other.GetComponent<BossBarrierBehavior>();
            enemy = Barrier.ES;
        }
        else if (other.GetComponent<BossArmorBehavior>() != null)
        {
            isHit = true;
            LaserShrink();
            return;
        }
        else
        {
            isHit = false;
            return;
        }

        isHit = true;
        if (enemy.takeDamage(laser.damage) <= 0)
        {
            if (other.GetComponent<Mover>() != null)
                enemy.DropOnDeath(other.GetComponent<Mover>().drop, other.transform.position, other.transform.rotation);
            if (!enemy.isBoss())
            {
                gameController.ModifyScore(enemy.getScoreValue());
                Destroy(other.gameObject);
            }
            else
                BE.killBoss();
            isHit = false;
        }

        LaserShrink();
    }
}
