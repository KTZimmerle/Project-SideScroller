using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBossRoutine : MonoBehaviour, IBossKill {

    protected int firingOrder;
    public int hitPoints;
    public int scoreValue;
    public float veloc;
    public float speed;
    public float distance;
    public bool notDead;
    public bool isDying;
    public BossShip BE;
    //public GameObject projectile;
    public GameObject projectileTwo;
    public GameObject laser;
    public GameObject gunPointOne;
    public GameObject gunPointTwo;
    public GameObject sml_explosion;
    public GameObject med_explosion;
    protected bool isMovingStraight;
    protected bool isMovingOutBounds;
    protected bool isMovingRotation;
    protected bool isMakingEntrance;

    GameController gameController;
    SpecialFXPool gfx;
    //ProjectilePool projPool;
    protected IEnumerator BossAttacks;
    protected IEnumerator LaserPattern;
    protected bool stopLaser = false;
    protected Vector3 stopPoint;
    protected bool isMoving;
    Vector3 startPosition;
    float startX;
    float startY;
    float radius;
    float theta;
    float entranceTimer;
    float swapMovementTime;
    float rotatetoCenterTime;
    protected bool reverseRotation;
    protected bool flipX;
    protected List<GameObject> bosses;
    protected List<Vector3> OutofMapYCoordinates;

    protected virtual void Awake()
    {
        reverseRotation = false;
        isDying = false;
        bosses = new List<GameObject>(7);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            bosses.Add(transform.GetChild(i).gameObject);
        }
        OutofMapYCoordinates = new List<Vector3>(7);
        initYCoordinates();
        GameObject target = GameObject.FindWithTag("GameController");
        if (target.GetComponent<GameController>() != null)
            gameController = target.GetComponent<GameController>();
        BE = new BossShip(hitPoints, scoreValue);
        
        radius = 5.0f;
        gameObject.SetActive(false);
    }

    protected void OnEnable()
    {
        Init();
    }

    protected void OnDisable()
    {
        BE.Init(hitPoints);
        isDying = false;
        //CancelInvoke();
    }

    void Init()
    {
        //isMakingEntrance = true;
        firingOrder = 0;
        isDying = false;
        flipX = false;
        rotatetoCenterTime = 1.0f;
        swapMovementTime = 4.0f;
        entranceTimer = 5.0f;
        isMovingRotation = false;
        isMovingStraight = false;
        isMoving = true;
        BossAttacks = BossRoutine();
        notDead = true;
        veloc = 1.0f;
        transform.gameObject.SetActive(true);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            transform.GetChild(i).position = new Vector3(0.0f, -15.0f, 0.0f);
            transform.GetChild(i).rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
            transform.GetChild(i).RotateAround(Vector3.zero, Vector3.forward, 360 / 7 * i);
            //transform.GetChild(i).LookAt(new Vector3(transform.GetChild(i).position.x, transform.GetChild(i).position.y, 0.0f));
            transform.GetChild(i).transform.GetComponent<Rigidbody>().velocity = ((Vector3.zero - bosses[i].transform.position) / (Vector3.zero - bosses[i].transform.position).magnitude) * 5.0f;
        }
    }

    void initYCoordinates()
    {
        OutofMapYCoordinates.Add(new Vector3(-8.0f, 3.9f));
        OutofMapYCoordinates.Add(new Vector3(-8.0f, 2.6f));
        OutofMapYCoordinates.Add(new Vector3(-8.0f, 1.3f));
        OutofMapYCoordinates.Add(new Vector3(-8.0f, 0.0f));
        OutofMapYCoordinates.Add(new Vector3(-8.0f, -1.3f));
        OutofMapYCoordinates.Add(new Vector3(-8.0f, -2.6f));
        OutofMapYCoordinates.Add(new Vector3(-8.0f, -3.9f));
    }

    IEnumerator BossRoutine()
    {
        //yield return new WaitForSeconds(2.0f);
        while (notDead)
        {
            yield return new WaitForSeconds(1.0f);
            //rotate pattern
            isMovingRotation = true;
            for (int seconds = 0; seconds < 15; seconds++)
            {
                bosses[firingOrder].GetComponent<AIBossDrone>().FireBullet();
                if (seconds > 5)
                    bosses[(firingOrder+1) % 7].GetComponent<AIBossDrone>().FireBullet();
                if(seconds > 10)
                    bosses[(firingOrder + 2) % 7].GetComponent<AIBossDrone>().FireBullet();
                yield return new WaitForSeconds(1.0f);
                firingOrder = (firingOrder + 1) % 7;
            }
            //yield return new WaitForSeconds(15.0f);
            isMovingRotation = false;
            yield return new WaitForSeconds(2.0f);

            //first movement pattern
            isMovingStraight = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<AIBossDrone>().startMovementPatternOne();
                yield return new WaitForSeconds(1.5f);
            }

            yield return new WaitForSeconds(5.5f);
            isMovingStraight = false;
            yield return new WaitForSeconds(0.5f);

            //rotate pattern
            isMovingRotation = true;
            for (int seconds = 0; seconds < 15; seconds++)
            {
                bosses[firingOrder].GetComponent<AIBossDrone>().FireBullet();
                if (seconds > 5)
                    bosses[(firingOrder + 2) % 7].GetComponent<AIBossDrone>().FireBullet();
                if (seconds > 10)
                    bosses[(firingOrder + 4) % 7].GetComponent<AIBossDrone>().FireBullet();
                yield return new WaitForSeconds(1.0f);
                firingOrder = (firingOrder + 1) % 7;
            }
            //yield return new WaitForSeconds(15.0f);
            isMovingRotation = false;
            yield return new WaitForSeconds(1.0f);

            //second movement pattern
            for (int i = 0; i < transform.childCount; i++)
            {
                bosses[i].GetComponent<AIBossDrone>().startMovementPatternTwo();
                bosses[i].GetComponent<AIBossDrone>().SetOutofMapPosition(OutofMapYCoordinates[i], flipX);
                flipX = !flipX;
            }

            yield return new WaitUntil(() => transform.GetChild(0).GetComponent<AIBossDrone>().IsMovementTwoDone());

            for (int i = 0; i < bosses.Count; i++)
            {
                bosses[i].GetComponent<AIBossDrone>().RedoEntrance();
                bosses[i].transform.RotateAround(Vector3.zero, Vector3.forward, 360 / 7 * i);
                bosses[i].transform.LookAt(new Vector3(bosses[i].transform.position.x, bosses[i].transform.position.y, 0.0f));
                bosses[i].transform.GetComponent<Rigidbody>().velocity = ((Vector3.zero - bosses[i].transform.position) / (Vector3.zero - bosses[i].transform.position).magnitude) * 5.0f;
                //bosses[i].GetComponent<AIBossDrone>().RedoEntrance();
            }

            yield return new WaitUntil(() => transform.GetChild(0).GetComponent<AIBossDrone>().isEntranceDone());
            yield return new WaitForSeconds(1.0f);

            /*for (int i = 0; i < bosses.Count; i++)
            {
                bosses[i].GetComponent<AIBossDrone>().ReturnToStart();
            }*/
            //repeat from the top
        }
    }

    // Use this for initialization
    protected void Start () {
        GameObject target = GameObject.FindWithTag("GFXPool");
        if (target.GetComponent<SpecialFXPool>() != null)
            gfx = target.GetComponent<SpecialFXPool>();
    }

    // Update is called once per frame
    protected void Update ()
    {
        if (entranceTimer <= 0.0f && isMoving)
        {
            //if (GetComponent<FirstBossRoutineHard>() == null)
            StartCoroutine(BossAttacks);
            isMoving = false;
        }
        else if (entranceTimer > 0.0f && isMoving)
        {
            entranceTimer -= Time.deltaTime;
        }

        if (isMovingRotation && notDead)
        {
            theta = ((30.0f * Time.deltaTime));
            if (reverseRotation)
                theta *= -1;
            /*transform.GetChild(0).position = Vector3.zero + (transform.GetChild(0).GetComponent<AIBossDrone>().GetStartPosition()).normalized
                                 * radius;*/
            transform.GetChild(0).RotateAround(Vector3.zero, Vector3.forward, theta);
            transform.GetChild(1).RotateAround(Vector3.zero, Vector3.forward, theta);
            transform.GetChild(2).RotateAround(Vector3.zero, Vector3.forward, theta);
            transform.GetChild(3).RotateAround(Vector3.zero, Vector3.forward, theta);
            transform.GetChild(4).RotateAround(Vector3.zero, Vector3.forward, theta);
            transform.GetChild(5).RotateAround(Vector3.zero, Vector3.forward, theta);
            transform.GetChild(6).RotateAround(Vector3.zero, Vector3.forward, theta);
            //Debug.Log(theta);
            //Debug.Log(transform.GetChild(0).GetComponent<Rigidbody>().rotation);
        }
    }

    public void LoseHealth()
    {
        if (BE.takeDamage(500) <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        if (!notDead)
            return;

        StartCoroutine("startExploding");
        //GetComponent<Rigidbody>().velocity = Vector3.zero;
        StartCoroutine(destroyBoss());
        //InvokeRepeating("destroyBoss", 3.0f, 0.5f);
        notDead = false;
        stopLaser = true;
        gameController.ModifyScore(BE.getScoreValue());
        //GetComponent<BoxCollider>().enabled = false;
    }

    IEnumerator destroyBoss()
    {
        yield return new WaitUntil(() => !isMovingStraight);
        yield return new WaitUntil(() => transform.GetChild(6).GetComponent<AIBossDrone>().IsMovementTwoDone());
        yield return new WaitUntil(() => transform.GetChild(6).GetComponent<AIBossDrone>().isEntranceDone());
        isMovingRotation = false;
        StopCoroutine(BossAttacks);
        isDying = true;
        yield return new WaitForSeconds(4.0f);
        for (int i = 0; i < bosses.Count; i++)
        {
            med_explosion = gfx.playMediumExplosion();
            med_explosion.transform.position = bosses[i].transform.position;
            med_explosion.SetActive(true);
            bosses[i].SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        Invoke("DeactivateBoss", 1.0f);
    }

    void DeactivateBoss()
    {
        gameObject.SetActive(false);
    }

    protected IEnumerator startExploding()
    {

        while (transform.GetChild(6).gameObject.activeSelf)
        {
            for (int i = 0; i < 7; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf)
                {
                    foreach (GameObject exp in transform.GetChild(i).GetComponent<AIBossDrone>().GetExplosionPts())
                    {
                        sml_explosion = gfx.playSmallExplosion();
                        sml_explosion.transform.position = transform.GetChild(i).position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), -1.5f);
                        sml_explosion.SetActive(true);
                        yield return new WaitForSeconds(0.05f);
                    }
                    //smallExplosions(transform.GetChild(2).transform.position, explosionPtsT);
                }
            }
        
            yield return new WaitForSeconds(0.1f);
            
        }
        yield return new WaitForSeconds(3.0f);
    }

    public void ChangeRotationDirection()
    {
        reverseRotation = !reverseRotation;
    }
}
