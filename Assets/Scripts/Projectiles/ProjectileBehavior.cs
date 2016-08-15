using UnityEngine;
using System.Collections;

public class ProjectileBehavior : MonoBehaviour {

    public Bullet bullet;
    public float acceleration;
    public bool isFriendly;
    public bool isHit;
    protected GameController gameController;
    protected AbstractEnemy enemy;
    protected FirstBossRoutine BE;
    public float veloc;
    public Collider[] search;
    public float speed;
    public bool followPlayer = false;
    protected Vector3 heading;
    protected float distance;
    protected Vector3 direction;
    public float angle;

    protected virtual void Awake()
    {
        findTarget();
        GameObject target = GameObject.FindWithTag("GameController");
        if (target.GetComponent<GameController>() != null)
            gameController = target.GetComponent<GameController>();
        bullet = new Bullet();
        isHit = false;

        if (followPlayer)
        {
            GetComponent<Rigidbody>().velocity = direction.normalized * speed;
        }
        else
            GetComponent<Rigidbody>().velocity = transform.right * speed;
    }

    void Start()
    {
        if (isFriendly)
            gameController.GetComponent<PlayerProjectileList>().addBullet(gameObject);
    }

    void FixedUpdate()
    {
        if(isFriendly)
            GetComponent<Rigidbody>().velocity += new Vector3(GetComponent<Rigidbody>().velocity.x + acceleration, GetComponent<Rigidbody>().velocity.y, 0.0f) * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!isFriendly || isHit)
            return;
        
        if (other.GetComponent<BossArmorBehavior>() != null)
        {
            gameController.GetComponent<PlayerProjectileList>().removeBullet(gameObject);
            Destroy(gameObject);
            return;
        }

        enemy = GetComponent<OnHitHandler>().OnHitHandle(other);

        if(enemy == null)
            return;

        if(enemy.getDeathStatus())
            return;

        if (enemy.takeDamage(bullet.damage) <= 0)
        {
            enemy.DropOnDeath(other.transform.position, other.transform.rotation);
            isHit = true;
            if (!enemy.isBoss())
            {
                gameController.ModifyScore(enemy.getScoreValue());
                enemy.PlayExplosion(other.transform.position, other.transform.rotation);
                Destroy(other.gameObject);
            }
            else
            {
                BE = other.GetComponent<FirstBossRoutine>();
                BE.killBoss();
            }
        }

        gameController.GetComponent<PlayerProjectileList>().removeBullet(gameObject);
        Destroy(gameObject);
    }

    protected void findTarget()
    {
        if (!isFriendly && followPlayer)
            search = Physics.OverlapSphere(transform.position, 25.0f, 1 << 11, QueryTriggerInteraction.Collide);
        if (search.Length > 0 && search[0] != null)
        {
            if (angle >= 0.01f || angle <= -0.01f)
            {
                heading = (transform.position - search[0].transform.position);
                distance = Mathf.Sqrt(heading.x * heading.x + heading.y * heading.y);
                direction = (new Vector3(-heading.y, heading.x, 0.0f) / distance) * angle + search[0].transform.position.normalized;
            }
            else
            {
                heading = (search[0].transform.position - transform.position);
                distance = heading.magnitude;
                direction = (heading / distance);
            }
        }
        else
            direction = -transform.right;
    }
}
