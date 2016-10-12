using UnityEngine;
using System.Collections;

public class PowerSelector : MonoBehaviour {

    const int MAXPOWERS = 6;
    private int collectedPowerUps;
    GameUI gameUI;

    void Awake()
    {
        gameUI = (GameUI) FindObjectOfType(typeof(GameUI));
        collectedPowerUps = 0;
    }

    /*void OnEnable()
    {
        collectedPowerUps = 0;
    }*/

    public int GetCollectedPowUps()
    {
        return collectedPowerUps;
    }

    public void IncrementCounter()
    {
        ++collectedPowerUps;
        if (collectedPowerUps > MAXPOWERS)
            collectedPowerUps = 1;
        HighlightPower();
    }

    public void selectPower(PowerUpSystem p)
    {
        if (collectedPowerUps == 0)
            return;

        switch (collectedPowerUps)
        {
            case 1:
                p.changeSpeed();
                gameUI.Speedup.image.fillCenter = false;
                gameUI.UpdateSpeed();
                collectedPowerUps = 0;
                break;
            case 2:
                if (p.missilePowUp)
                    break;
                p.missilePowUp = true;
                gameUI.Missile.image.fillCenter = false;
                gameUI.ClearText(gameUI.Missile);
                collectedPowerUps = 0;
                break;
            case 3:
                if (p.altfirePowUp)
                    break;
                p.altfirePowUp = true;
                p.laserPowUp = false;
                gameUI.AltFire.image.fillCenter = false;
                gameUI.RestoreLaserText();
                gameUI.ClearText(gameUI.AltFire);
                collectedPowerUps = 0;
                break;
            case 4:
                if (p.laserPowUp)
                    break;
                p.laserPowUp = true;
                p.altfirePowUp = false;
                gameUI.Laser.image.fillCenter = false;
                gameUI.RestoreAltFireText();
                gameUI.ClearText(gameUI.Laser);
                collectedPowerUps = 0;
                break;
            case 5:
                if (p.isShielded)
                    break;
                p.ActivateShields();
                p.isShielded = true;
                gameUI.Shield.image.fillCenter = false;
                gameUI.ClearText(gameUI.Shield);
                collectedPowerUps = 0;
                break;
            case 6:
                p.ActivateSuperBomb();
                gameUI.SuperBomb.image.fillCenter = false;
                collectedPowerUps = 0;
                break;
            default:
                break;
        }
    }

    void HighlightPower()
    {
        gameUI.DeHighlightAll();
        switch (collectedPowerUps)
        {
            case 1:
                gameUI.Speedup.image.fillCenter = true;
                break;
            case 2:
                gameUI.Missile.image.fillCenter = true;
                break;
            case 3:
                gameUI.AltFire.image.fillCenter = true;
                break;
            case 4:
                gameUI.Laser.image.fillCenter = true;
                break;
            case 5:
                gameUI.Shield.image.fillCenter = true;
                break;
            case 6:
                gameUI.SuperBomb.image.fillCenter = true;
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter(Collider pickup)
    {
        if (pickup.CompareTag("PickUp"))
        {
            IncrementCounter();
            pickup.gameObject.SetActive(false);
            //Destroy(pickup.gameObject);
        }
    }
}
