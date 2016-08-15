using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreen : MonoBehaviour {

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
	}
}
