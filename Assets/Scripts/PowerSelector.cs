using UnityEngine;
using System.Collections;

public class PowerSelector : MonoBehaviour {

    private int collectedPowerUps;

    public int GetCollectedPowUps()
    {
        return collectedPowerUps;
    }

    public void IncrementCounter()
    {
        collectedPowerUps++;
    }

    public void selectPower(PowerUpSystem p)
    {
        switch (collectedPowerUps)
        {
            case 1:

                break;
            case 2:
                p.missilePowUp = true;
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            default:
                break;
        }
        collectedPowerUps = 0;
    }

    void Awake()
    {
        collectedPowerUps = 0;
    }

    void OnTriggerEnter(Collider pickup)
    {
        if (pickup.CompareTag("PickUp"))
        {
            IncrementCounter();
            Destroy(pickup.gameObject);
        }
    }
}
