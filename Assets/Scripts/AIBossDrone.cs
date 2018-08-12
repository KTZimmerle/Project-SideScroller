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
    Vector3 BGMapPosition;
    float startUpTime;
    float timeToLerp;
    float timetoSlerp;
    float waitTime;
    float travelTime;
    float delay;
    float TPSlerp;
    float TPDelay;
    float SecondTPDelay;
    float TPLerp;
    bool isMoving;
    bool isMovingTwo;
    bool isMovingTwoDone;
    bool isOutofMap;
    bool isFlyingStraight;
    bool isMakingEntrance;
    bool notDead;
    bool isHardMode;
    bool isMovingThree;
    bool isMovingThreeDone;
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
        isHardMode = false;
        Init();
        /*healthIndicator = GetComponent<Material>();
        healthIndicator.SetColor()*/
    }

    public void Init()
    {
        TPSlerp = 5.0f;
        TPDelay = 1.0f;
        SecondTPDelay = 0.5f;
        TPLerp = 1.5f;
        delay = 0.0f;
        isMakingEntrance = true;
        notDead = true;
        isMoving = false;
        isMovingTwo = false;
        isMovingThree = false;
        isOutofMap = false;
        isMovingTwoDone = true;
        isMovingThreeDone = true;
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
        //oppositeDirection = Quaternion.Inverse(currentDirection);
        GetComponent<AutoRotate>().enabled = true;
        if (isHardMode)
        {
            timeToLerp -= 1.0f;
            timetoSlerp -= 0.5f;
            startUpTime -= 0.5f;
        }
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
            if(isHardMode)
                transform.position = Vector3.Lerp(currentSide, oppositeSide, (3.0f - timeToLerp) / 3);
            else
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
                //transform.LookAt(Vector3.zero, transform.forward);
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

    public void startMovementPatternThree()
    {
        isMovingThree = true;
        isMovingThreeDone = false;
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
        if (isHardMode)
        {
            //randomly select a pattern
            if(isOutofMap)
                MovementPatternTwoPartTwoHM();

            if(isMovingThree)
                MovementPatternThree();
        }
        else
        {
            MovementPatternTwoPartTwo();
        }
        

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

    void MovementPatternTwoPartTwoHM()
    {
        if (delay < 0.0f)
            MovementPatternTwoPartTwo();
        else
            delay -= Time.deltaTime;
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

        ReturnToStart();
    }

    void MovementPatternThree()
    {
        if (TPDelay > 0.0f)
        {
            TPDelay -= Time.deltaTime;
            if (TPDelay <= 0.0f)
            {
                if (TPSlerp >= 0.0f)
                {
                    //transform.rotation = Vector3.Slerp(currentSide, oppositeSide, (5.0f - timeToLerp) / 4);
                    transform.rotation = Quaternion.Slerp(currentDirection, oppositeDirection, (5.0f - TPSlerp) / 5);
                    TPSlerp -= Time.deltaTime;
                }
                else
                {
                    if (TPLerp >= 0.0f)
                    {
                        //change target vectors
                        transform.position = Vector3.Lerp(currentSide, oppositeSide, (1.5f - timeToLerp) / 1.5f);
                        TPLerp -= Time.deltaTime;
                    }
                    else
                    {
                        TPSlerp = 5.0f;
                        TPDelay = 1.0f;
                        SecondTPDelay = 0.5f;
                        TPLerp = 1.5f;
                        isMovingThree = false;
                    }
                }
            }
        }
    }

    public void ReturnToStart()
    {
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
                //RedoEntrance();
                GetComponent<AutoRotate>().enabled = false;
            }
        }
    }

    public void DelayMovementPatternTwo(float wait)
    {
        waitTime = wait;
    }

    public void SetOutofMapPosition(Vector3 OoMPos, bool flipX)
    {
        
        OutofMapPosition = OoMPos;
        if (flipX)
            OutofMapPosition = new Vector3(-OutofMapPosition.x, OutofMapPosition.y);
    }

    public void SetBGMapPosition(Vector3 BGMPos)
    {
        OutofMapPosition = BGMPos;
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
    }*/

    public void FireLaserBeam()
    {
        transform.GetChild(7).gameObject.SetActive(true);
    }

    public void StopLaserBeam()
    {
        transform.GetChild(7).gameObject.SetActive(false);
    }

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

    public void SetHardModeOn()
    {
        isHardMode = true;
    }

    public void SetDelay(float d)
    {
        delay = d;
    }

    public void PlayLasFX()
    {
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            if (ps.CompareTag("Laser_FX"))
                ps.Play();
        }
    }
}
