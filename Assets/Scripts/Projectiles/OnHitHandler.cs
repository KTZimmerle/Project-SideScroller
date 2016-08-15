using UnityEngine;
using System.Collections;

public class OnHitHandler : MonoBehaviour {

    protected CircularMover E1;
    protected RotatorMover E2;
    protected StraightMover E3;
    protected TrackingMover E4;
    protected WavyMover EP;
    protected FirstBossRoutine BE;
    protected BossBarrierBehavior Barrier;

    public AbstractEnemy OnHitHandle(Collider other)
    {
        AbstractEnemy e;
        if (other.GetComponent<CircularMover>() != null)
        {
            E1 = other.GetComponent<CircularMover>();
            e = E1.ES;
        }
        else if (other.GetComponent<RotatorMover>() != null)
        {
            E2 = other.GetComponent<RotatorMover>();
            e = E2.ES;
        }
        else if (other.GetComponent<StraightMover>() != null)
        {
            E3 = other.GetComponent<StraightMover>();
            e = E3.ES;
        }
        else if (other.GetComponent<TrackingMover>() != null)
        {
            E4 = other.GetComponent<TrackingMover>();
            e = E4.ES;
        }
        else if (other.GetComponent<WavyMover>() != null)
        {
            EP = other.GetComponent<WavyMover>();
            e = EP.ES;
        }
        else if (other.GetComponent<FirstBossRoutine>() != null)
        {
            BE = other.GetComponent<FirstBossRoutine>();
            e = BE.BE;
        }
        else if (other.GetComponent<BossBarrierBehavior>() != null)
        {
            Barrier = other.GetComponent<BossBarrierBehavior>();
            e = Barrier.ES;
        }
        else
            e = null;

        return e;
    }
}
