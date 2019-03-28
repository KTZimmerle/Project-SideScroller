using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InstructionsScript : MonoBehaviour {
    	
	// Update is called once per frame
	void Update () {

        if (Input.anyKeyDown)
        {
            SwitchPage();
        }
    }

    void SwitchPage()
    {
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
            SceneManager.LoadScene(2);
        }
    }
}
