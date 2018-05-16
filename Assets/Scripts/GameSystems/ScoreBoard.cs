using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreBoard : MonoBehaviour {

    int numberDestroyed;
    int lastNumberDestroyed;
    int totalNumberDestroyed;
    float EnemyCount;
    float hitRatio;
    int bonusPoints;
    int numRazors;
    int numSwoopers;
    int numPowerShips;
    int numBlasters;
    int numHunters;

    void Awake()
    {
        totalNumberDestroyed = 0;
        lastNumberDestroyed = 0;
        numberDestroyed = 0;
        EnemyCount = 0;
        numRazors = 0;
        numSwoopers = 0;
        numPowerShips = 0;
        numBlasters = 0;
        numHunters = 0;
    }

    public void tallyScore(int bonusPts)
    {
        hitRatio = (numberDestroyed / EnemyCount) * 100;
        lastNumberDestroyed = numberDestroyed;
        int bonusPoints = CalculateBonus(hitRatio/100, bonusPts);
        StartCoroutine(GetComponent<GameUI>().displayHitRatio(hitRatio, numberDestroyed, EnemyCount, bonusPoints));
        numberDestroyed = 0;
    }

    int CalculateBonus(float hitRate, int bonus)
    {
        float temp = 0.0f;
        if (hitRate == 1.0f)
        {
            temp = bonus * 2;
        }
        else if (hitRate < 1.0f)
        {
            temp = bonus * hitRate;
        }
        int result = (int)temp;
        return result;
    }

    public void incrementHit()
    {
        numberDestroyed++;
    }

    public void incrementEnemyHit(GameObject e)
    {
        if (e.GetComponent<StraightMover>() != null)
            numRazors++;
        else if (e.GetComponent<CircularMover>() != null)
            numSwoopers++;
        else if (e.GetComponent<WavyMover>() != null)
            numPowerShips++;
        else if (e.GetComponent<RotatorMover>() != null)
            numBlasters++;
        else if (e.GetComponent<TrackingMover>() != null)
            numHunters++;
    }

    public void setEnemyCount(int e)
    {
        EnemyCount = e;
    }

    public void tallyEnemiesDestroyed()
    {
        GameUI gUI = GetComponent<GameUI>();
        if (numRazors > 0)
        {
            gUI.RazorIcon.GetComponentInChildren<Text>().text = "x " + numRazors;
            gUI.RazorIcon.gameObject.SetActive(true);
        }

        if (numPowerShips > 0)
        {
            gUI.PowerShipIcon.GetComponentInChildren<Text>().text = "x " + numPowerShips;
            gUI.PowerShipIcon.gameObject.SetActive(true);
        }

        if (numSwoopers > 0)
        {
            gUI.SwooperIcon.GetComponentInChildren<Text>().text = "x " + numSwoopers;
            gUI.SwooperIcon.gameObject.SetActive(true);
        }

        if (numBlasters > 0)
        {
            gUI.BlasterIcon.GetComponentInChildren<Text>().text = "x " + numBlasters;
            gUI.BlasterIcon.gameObject.SetActive(true);
        }

        if (numHunters > 0)
        {
            gUI.HunterIcon.GetComponentInChildren<Text>().text = "x " + numHunters;
            gUI.HunterIcon.gameObject.SetActive(true);
        }

        numRazors = 0;
        numPowerShips = 0;
        numSwoopers = 0;
        numBlasters = 0;
        numHunters = 0;
    }

    public int GetNumDestroyed()
    {
        return lastNumberDestroyed;
    }
}
