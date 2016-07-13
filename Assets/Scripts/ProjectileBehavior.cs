using UnityEngine;
using System.Collections;

public class ProjectileBehavior : MonoBehaviour {

    Bullet bullet;
    public float acceleration;
    public bool isFriendly;
    public bool isHit;
    protected GameController gameController;
    public AbstractProjectile proj;
    protected AbstractEnemy enemy;
    protected CircularMover E1;
    protected RotatorMover E2;
    protected StraightMover E3;
    protected WavyMover EP;
    //Ray r;

    protected virtual void Awake()
    {
        GameObject target = GameObject.FindWithTag("GameController");
        if (target.GetComponent<GameController>() != null)
            gameController = target.GetComponent<GameController>();
        bullet = new Bullet();
        isHit = false;
        //r = new Ray(transform.position, transform.right);
    }

    void FixedUpdate()
	{
        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x + acceleration, GetComponent<Rigidbody>().velocity.y, 0.0f);
	}

    protected virtual void OnTriggerEnter(Collider other)
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
        else if (other.GetComponent<WavyMover>() != null)
        {
            EP = other.GetComponent<WavyMover>();
            enemy = EP.ES;
        }
        else
            return;

        if (enemy.takeDamage(bullet.damage) <= 0)
        {
            gameController.ModifyScore(enemy.getScoreValue());
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }

    /*protected void ValidateEnemy(AbstractEnemy ae)
    {
        
    }*/
}
