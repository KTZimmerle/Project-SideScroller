using UnityEngine;
using System.Collections;

public class EnemyShipOne : MonoBehaviour
{
    ProjectileBehavior proj;
    MissileBehavior missile;

    public class EnemyFighter : AbstractEnemy
    {
        public EnemyFighter() { health = 1; scoreValue = 2; }

        protected override void takeDamage(int dmg)
        {
            health = -dmg;
        }

        public override int getScoreValue()
        {
            return scoreValue;
        }
    }

    EnemyFighter fighterOne = new EnemyFighter();

    void OnTriggerEnter(Collider other)
    {
    }
}
