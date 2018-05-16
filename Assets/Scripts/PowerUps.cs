using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour {

    GameUI gameUI;

    void Awake()
    {
        gameUI = (GameUI)FindObjectOfType(typeof(GameUI));
    }

    public void activatePower(PowerUpSystem p, int ID)
    {
        switch (ID)
        {
            case 1:
                p.changeSpeed();
                gameUI.Speedup.image.fillCenter = false;
                gameUI.UpdateSpeed();
                break;
            case 2:
                if (p.missilePowUp)
                    break;
                p.missilePowUp = true;
                gameUI.Missile.image.fillCenter = false;
                gameUI.ClearText(gameUI.Missile);
                break;
            case 3:
                if (p.altfirePowUp)
                    break;
                p.altfirePowUp = true;
                p.laserPowUp = false;
                gameUI.AltFire.image.fillCenter = false;
                gameUI.RestoreLaserText();
                gameUI.ClearText(gameUI.AltFire);
                break;
            case 4:
                if (p.laserPowUp)
                    break;
                p.laserPowUp = true;
                p.altfirePowUp = false;
                gameUI.Laser.image.fillCenter = false;
                gameUI.RestoreAltFireText();
                gameUI.ClearText(gameUI.Laser);
                break;
            case 5:
                if (p.isShielded)
                    break;
                p.ActivateShields();
                p.isShielded = true;
                gameUI.Shield.image.fillCenter = false;
                gameUI.ClearText(gameUI.Shield);
                break;
            case 6:
                p.ActivateSuperBomb();
                gameUI.SuperBomb.image.fillCenter = false;
                break;
            case 7:
                GetComponent<ActivateOrbiter>().RequestOrbiter();
                break;
            default:
                break;
        }
    }

    void HighlightPower(int ID)
    {
        gameUI.DeHighlightAll();
        switch (ID)
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
        if (pickup.GetComponent<PowerupID>() != null)
        {
            activatePower(GetComponent<PowerUpSystem>(), pickup.GetComponent<PowerupID>().getPowerupID());
            pickup.gameObject.SetActive(false);
        }

        if (pickup.CompareTag("PickUp"))
        {
            //activatePower();
            pickup.gameObject.SetActive(false);
            //Destroy(pickup.gameObject);
        }
    }
}
