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
    public Button Speedup;
    public Button Missile;
    public Button AltFire;
    public Button Laser;
    public Button Shield;
    public Button SuperBomb;
    public double speedmultiplier;

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
}
