using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreen : MonoBehaviour {

    float switchTimer = 5.0f;

	// Update is called once per frame
	void Update ()
    {
        //Press esc button to quit the game
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        //Press any button to start game
        if (Input.anyKey)
        {
            //start the game
            //Application.LoadLevel("Space Battle");
            SceneManager.LoadScene(1);
        }

        /*switchTimer -= Time.deltaTime;
        if (switchTimer < 0.0f)
        {
            switchTimer = 5.0f;
            if (GetComponent<TitleUI>().Title.IsActive() && !GetComponent<TitleUI>().razorIcon.IsActive())
            {
                GetComponent<TitleUI>().DisplayControls();
                GetComponent<TitleUI>().HideTitle();
            }
            else if (GetComponent<TitleUI>().Instructions.IsActive() && !GetComponent<TitleUI>().Title.IsActive())
            {
                GetComponent<TitleUI>().DisplayEnemyValues();
                GetComponent<TitleUI>().HideControls();
            }
            else if (GetComponent<TitleUI>().razorIcon.IsActive() && !GetComponent<TitleUI>().Instructions.IsActive())
            {
                GetComponent<TitleUI>().DisplayTitle();
                GetComponent<TitleUI>().HideEnemyValues();
            }
        }*/
	}
}
