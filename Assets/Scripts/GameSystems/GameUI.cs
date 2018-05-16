using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour {

    public Text totalScore;
    public Text gameOverText;
    public Text playerLivesText;
    public Text waveText;
    public Text SpeedupText;
    public Text NextWave;
    public Text BossIncoming;
    public Text HitRatio;
    public Button Speedup;
    public Button Missile;
    public Button AltFire;
    public Button Laser;
    public Button Shield;
    public Button SuperBomb;
    public Image RazorIcon;
    public Image SwooperIcon;
    public Image PowerShipIcon;
    public Image BlasterIcon;
    public Image HunterIcon;
    public Text HighScoresTitle;
    public Text[] HighScores;
    public Image WarningIcon;
    public InputField InitialsField;
    public double speedmultiplier;
    //public int numHighScores;

    public void ClearText(Button b)
    {
        b.GetComponentInChildren<Text>().text = "";
    }

    public void UpdateScore(int score)
    {
        totalScore.text = "Score: " + score;
    }

    public void UpdateWave(int waveCount)
    {
        waveText.text = "Wave: " + waveCount;
    }

    public void UpdateLives(int playerLives)
    {
        playerLivesText.text = "Lives: " + (playerLives);
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over\nPress Esc to exit";
    }

    public void Initialize(int playerLives, int score, int waveCount)
    {
        speedmultiplier = 1.0;
        gameOverText.text = "";
        HitRatio.text = "";
        playerLivesText.text = "Lives: " + (playerLives);
        totalScore.text = "Score: " + score;
        waveText.text = "Wave: " + waveCount;
        SpeedupText.text = "Speed x 1.0";
    }

    public void DeHighlightAll()
    {
        Speedup.image.fillCenter = false;
        Missile.image.fillCenter = false;
        AltFire.image.fillCenter = false;
        Laser.image.fillCenter = false;
        Shield.image.fillCenter = false;
        SuperBomb.image.fillCenter = false;
    }

    public void RestoreButtonText(bool hasAltfire, bool hasLaser)
    {
        if (hasAltfire || hasLaser)
        {
            RestoreAltFireText();
            RestoreLaserText();
        }
        else
        {
            RestoreMissileText();
        }
    }

    public void RestoreShieldText()
    {
        Shield.GetComponentInChildren<Text>().text = "Shield";
    }

    public void RestoreMissileText()
    {
        Missile.GetComponentInChildren<Text>().text = "Missile";
    }

    public void RestoreAltFireText()
    {
        AltFire.GetComponentInChildren<Text>().text = "Alt Fire";
    }

    public void RestoreLaserText()
    {
        Laser.GetComponentInChildren<Text>().text = "Laser";
    }

    public void UpdateSpeed()
    {
        if (speedmultiplier + 0.2 <= 2.0)
            speedmultiplier += 0.2;
        else
            speedmultiplier = 1.0;
        SpeedupText.text = "Speed x " + speedmultiplier;
    }

    public IEnumerator nextWaveMessage()
    {
        NextWave.text = "Next Wave Incoming";
        yield return new WaitForSeconds(3.0f);
        NextWave.text = "";
    }

    public IEnumerator bossIncomingMessage()
    {
        BossIncoming.text = "Warning!";
        for (int i = 0; i < 2; i++)
        {
            BossIncoming.enabled = true;
            yield return new WaitForSeconds(1.5f);
            BossIncoming.enabled = false;
            yield return new WaitForSeconds(0.25f);
        }
        BossIncoming.text = "Battleship Incoming!";
        BossIncoming.enabled = true;
        yield return new WaitForSeconds(2.0f);
        BossIncoming.text = "";
    }

    public IEnumerator displayHitRatio(float percentage, int hits, float enemies, int bonusPoints)
    {
        HitRatio.text = "Destroyed: " + hits + "/" + enemies + "\n";
        yield return new WaitForSeconds(1.0f);
        HitRatio.text += percentage.ToString("F2") + "%\n";
        GetComponent<ScoreBoard>().tallyEnemiesDestroyed();
        yield return new WaitForSeconds(1.0f);
        if (hits/enemies == 1.0f)
        {
            HitRatio.text += "Perfect!\n";
        }
        HitRatio.text += "Bonus: " + bonusPoints;
        GetComponent<GameController>().ModifyScore(bonusPoints);
        yield return new WaitForSeconds(2.0f);
        HitRatio.text = "";
        RazorIcon.gameObject.SetActive(false);
        SwooperIcon.gameObject.SetActive(false);
        PowerShipIcon.gameObject.SetActive(false);
        BlasterIcon.gameObject.SetActive(false);
        HunterIcon.gameObject.SetActive(false);
    }

    public void DisplayHighScores()
    {
        if (HighScores == null)
            return;

        GetComponent<HighScore>().loadScores(HighScoresTitle, HighScores);
        HighScoresTitle.gameObject.SetActive(true);/**/
        for (int i = 0; i < 10; i++)
        {
            HighScores[i].gameObject.SetActive(true);
        }
        gameOverText.text = "";
    }

    public Text RequestPlacementText(int pos)
    {
        return HighScores[pos];
    }
}
