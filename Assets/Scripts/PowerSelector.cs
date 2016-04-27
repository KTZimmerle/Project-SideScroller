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

    void selectPower(PowerUpSystem p)
    {
        switch (collectedPowerUps)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                p.missilePowUp = true;
                break;
            case 4:
                break;
        }
        collectedPowerUps = 0;
    }

    void Start()
    {
        collectedPowerUps = 0;
    }
}
