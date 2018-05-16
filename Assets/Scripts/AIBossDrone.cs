using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossDrone : MonoBehaviour, IBossKill {

    public Material healthIndicator;
    public GameObject projectile;
    SpecialFXPool gfx;
    ProjectilePool projPool;
    public float startX;
    public float startY;
    public float startRotation;
    Vector3 StartPosition;
    Vector3 OutofMapPosition;
    float startUpTime;
    float timeToLerp;
    float timetoSlerp;
    float waitTime;
    float travelTime;
    bool isMoving;
    bool isMovingTwo;
    bool isMovingTwoDone;
    bool isOutofMap;
    bool isFlyingStraight;
    bool isMakingEntrance;
    bool notDead;
    public BossShip BE;
    Vector3 currentSide;
    Quaternion currentDirection;
    Vector3 oppositeSide;
    Quaternion oppositeDirection;
    List<GameObject> explosionPt;

    public int hitPoints;
    public int scoreValue;

    // Use this for initialization
    void Awake()
    {
        explosionPt = GetComponent<ExplosionPt_Retriever>().RetrievePoints();
        BE = new BossShip(hitPoints, scoreValue);
        Init();
        /*healthIndicator = GetComponent<Material>();
        healthIndicator.SetColor()*/
    }

    public void Init()
    {
        isMakingEntrance = true;
        notDead = true;
        isMoving = false;
        isMovingTwo = false;
        isOutofMap = false;
        isMovingTwoDone = true;
        isFlyingStraight = false;
        startUpTime = 1.0f;
        timeToLerp = 4.0f;
        timetoSlerp = 1.5f;
        waitTime = 1.0f;
        travelTime = 3.0f;
        GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.white);
    }


    private void OnEnable()
    {
        Init();
    }

    void OnDisable()
    {
        BE.Init(hitPoints);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        //CancelInvoke();
    }

    // Use this for initialization
    void Start ()
    {
        GameObject target = GameObject.FindWithTag("ProjectilePool");
        if (target.GetComponent<ProjectilePool>() != null)
            projPool = target.GetComponent<ProjectilePool>();
        StartPosition.Set(startX, startY, 0.0f);
        /*StartPosition = new Vector3(startX, startY, 0.0f);
        transform.position = StartPosition;*/
    }
	
	// Update is called once per frame
	void Update () {

        if (isMakingEntrance && (transform.position - Vector3.zero).magnitude < 5.0f)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            isMakingEntrance = false;
            transform.localPosition = StartPosition;
        }

        if (isMoving)
        {
            MovementPatternOne();
        }

        if (isMovingTwo)
        {
            MovementPatternTwo();
        }

    }

    public void SetPosition(float x, float y)
    {
        startX = x;
        startY = y;
    }

    public Vector3 GetStartPosition()
    {
        return StartPosition;
    }

    public Vector3 moveOppositeSide()
    {
        return Vector3.zero;
    }

    public void startMovementPatternOne()
    {
        isMoving = true;
        currentSide = transform.position;
        currentDirection = transform.rotation;
        oppositeSide = -transform.position;
        oppositeDirection = Quaternion.Euler(0.0f, 0.0f, transform.rotation.z + 180.0f) * transform.rotation;
        GetComponent<AutoRotate>().enabled = true;
        //start spinning
    }

    public void MovementPatternOne()
    {
        if (startUpTime > 0.0f)
        {
            startUpTime -= Time.deltaTime;
            return;
        }

        if (timeToLerp > 0.0f)
        {
            transform.position = Vector3.Lerp(currentSide, oppositeSide, (4.0f - timeToLerp) / 4);
            timeToLerp -= Time.deltaTime;
        }
        else
        {
            if (timetoSlerp > 0.0f)
            {
                GetComponent<AutoRotate>().enabled = false;
                transform.rotation = Quaternion.Slerp(currentDirection, oppositeDirection, 1.0f - timetoSlerp);
                timetoSlerp -= Time.deltaTime;
            }
            else
            {
                transform.rotation = oppositeDirection;
                isMoving = false;
                startUpTime = 1.0f;
                timeToLerp = 2.0f;
                timetoSlerp = 1.5f;
            }
        }
    }
    
    public void startMovementPatternTwo()
    {
        isMovingTwo = true;
        isMovingTwoDone = false;
        currentSide = transform.position;
        currentDirection = transform.rotation;
        oppositeSide = -transform.position;
        oppositeDirection = Quaternion.Euler(0.0f, 0.0f, transform.rotation.z + 180.0f) * transform.rotation;
        


        //start spinning
    }

    public void MovementPatternTwo()
    {
        //part 1 of phase 2
        MovementPatternTwoPartOne();

        //part 2 of phase 2
        MovementPatternTwoPartTwo();

        //part 3 of phase 2

    }

    void MovementPatternTwoPartOne()
    {
        if (timetoSlerp > 0.0f)
        {
            transform.rotation = Quaternion.Slerp(currentDirection, oppositeDirection, 1.0f - timetoSlerp);
            timetoSlerp -= Time.deltaTime;
        }
        else
        {
            if (timeToLerp > 0.0f && !isOutofMap)
            {
                //transform.position = Vector3.Lerp(currentSide, oppositeSide, (2.0f - timeToLerp) / 2);
                GetComponent<Rigidbody>().velocity = -((Vector3.zero - transform.position) / (Vector3.zero - transform.position).magnitude) * 5.0f;
                timeToLerp -= Time.deltaTime;
            }
            else if (timeToLerp <= 0.0f && !isOutofMap)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.rotation = oppositeDirection;
                isFlyingStraight = true;
                isOutofMap = true;
            }
        }
    }

    void MovementPatternTwoPartTwo()
    {
        if (waitTime < 0.0f && isFlyingStraight)
        {
            transform.position = OutofMapPosition + Vector3.right * (OutofMapPosition.x / 2);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, OutofMapPosition.x > 0.0f ? 270.0f : 90.0f);
            GetComponent<Rigidbody>().velocity = ((OutofMapPosition - transform.position) / (OutofMapPosition - transform.position).magnitude) * 10.0f;
            isFlyingStraight = false;
            GetComponent<AutoRotate>().enabled = true;
        }
        else if (waitTime >= 0.0f && isFlyingStraight)
        {
            waitTime -= Time.deltaTime;
        }


        if (travelTime > 0.0f && (waitTime <= 0.0f && !isFlyingStraight))
        {
            travelTime -= Time.deltaTime;
            if (travelTime <= 0.0f)
            {
                startUpTime = 1.0f;
                timeToLerp = 4.0f;
                timetoSlerp = 1.5f;
                isMovingTwo = false;
                waitTime = 1.0f;
                travelTime = 3.0f;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                isOutofMap = false;
                transform.position = new Vector3(0.0f, -15.0f, 0.0f);
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                isMovingTwoDone = true;
                RedoEntrance();
                GetComponent<AutoRotate>().enabled = false;
            }
        }
    }

    public void SetOutofMapPosition(Vector3 OoMPos, bool flipX)
    {
        
        OutofMapPosition = OoMPos;
        if (flipX)
            OutofMapPosition = new Vector3(-OutofMapPosition.x, OutofMapPosition.y);
    }

    public void FireBullet()
    {
        GameObject bullet = GetBullet();
        BE.Shoot(bullet, transform.position, transform.rotation);
        //projPool.FireNextBulletTwo(this.gameObject);
    }

    public GameObject GetBullet()
    {
        return projPool.FireNextBulletTwo(this.gameObject);
    }

    /*protected GameObject FireBolt()
    {
        return projPool.FireNextBolt(this.gameObject);
    }

    protected GameObject FireLaser()
    {
        return projPool.FireNextLaser(this.gameObject);
    }*/

    public bool IsDoneMoving()
    {
        return !isMoving;
    }/**/

    public bool IsMovementTwoDone()
    {
        return isMovingTwoDone;
    }

    public void RedoEntrance()
    {
        isMakingEntrance = true;
    }

    public bool isEntranceDone()
    {
        return (isMakingEntrance == false);
    }

    public List<GameObject> GetExplosionPts()
    {
        return explosionPt;
    }

    public void Kill()
    {
        if (!notDead)
            return;

        transform.parent.GetComponent<SecondBossRoutine>().LoseHealth();
        notDead = false;
        GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.red);
        //GetComponent<BoxCollider>().enabled = false;
    }
}
