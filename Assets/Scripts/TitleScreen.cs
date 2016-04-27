using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {

	// Update is called once per frame
	void Update ()
    {
        //Press any button to start game
        if (Input.GetKey(KeyCode.Escape))
        {
            //start the game
            Application.Quit();
        }

        //Press any button to start game
        if (Input.anyKey)
        {
           //start the game
            Application.LoadLevel("Space Battle");
        }
	}
}
