using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour {

    public Text totalScore;
    public Text gameOverText;
    public Text playerLivesText;
    public Text waveText;
    
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

    public void GameOver(bool GameOverFlag)
    {
        GameOverFlag = true;
        gameOverText.text = "Game Over\nPress Esc to exit";
    }

    public void Initialize(int playerLives, int score, int waveCount)
    {
        gameOverText.text = "";
        playerLivesText.text = "Lives: " + (playerLives);
        totalScore.text = "Score: " + score;
        waveText.text = "Wave: " + waveCount;
    }

}
