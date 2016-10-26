using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {

    bool isPaused;

	// Use this for initialization
	void Start ()
    {
        isPaused = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Pause();
	}

    void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0;
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1;
        }
    }

    public bool isGamePaused()
    {
        return isPaused;
    }

}
