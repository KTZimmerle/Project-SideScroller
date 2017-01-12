using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleUI : MonoBehaviour {

    public Image razorIcon;
    public Image swooperIcon;
    public Image powershipIcon;
    public Image blasterIcon;
    public Image hunterIcon;
    public Image surpriseIcon;
    public Text Title;
    public Text Instructions;
    //public Image TitleScreen; - Not yet available

    public void DisplayTitle()
    {
        Title.gameObject.SetActive(true);
    }

    public void DisplayEnemyValues()
    {
        razorIcon.gameObject.SetActive(true);
        swooperIcon.gameObject.SetActive(true);
        powershipIcon.gameObject.SetActive(true);
        blasterIcon.gameObject.SetActive(true);
        hunterIcon.gameObject.SetActive(true);
        surpriseIcon.gameObject.SetActive(true);
    }

    public void DisplayControls()
    {
        Instructions.gameObject.SetActive(true);
    }

    public void HideTitle()
    {
        Title.gameObject.SetActive(false);
    }

    public void HideEnemyValues()
    {
        razorIcon.gameObject.SetActive(false);
        swooperIcon.gameObject.SetActive(false);
        powershipIcon.gameObject.SetActive(false);
        blasterIcon.gameObject.SetActive(false);
        hunterIcon.gameObject.SetActive(false);
        surpriseIcon.gameObject.SetActive(false);
    }

    public void HideControls()
    {
        Instructions.gameObject.SetActive(false);
    }
}
