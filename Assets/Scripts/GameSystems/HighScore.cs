using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class HighScore : MonoBehaviour {

    List<int> highScores;
    List<string> initials;

    UnityEngine.UI.InputField newInitials;

    int posToChange;

    void WriteToFile()
    {
        //TODO: get this function to overwrite existing file!
        TextWriter tw = new StreamWriter("highscore.zw");
        for (int i = 0; i < 10; i++)
        {
            tw.WriteLine(initials[i] + "." + highScores[i]);
        }
        tw.Close();
    }

    void ReadFromFile()
    {
        TextReader tr = new StreamReader("highscore.zw");
        string buffer;
        while ((buffer = tr.ReadLine()) != null)
        {
            string[] components = buffer.Split('.');
            initials.Add(components[0]);
            int score;
            if (int.TryParse(components[1], out score))
                highScores.Add(score);
            else
            {
                Debug.LogError("Unexpected input error! Fix highscore.zw! Closing stream.");
                tr.Close();
                Debug.Break();
            }
        }
        tr.Close();
    }

	// Use this for initialization
	void Awake () {

        //DontDestroyOnLoad(transform.gameObject);

        if (!File.Exists("highscore.zw"))
        {
            highScores = new List<int> { 50000, 25000, 12500, 7500, 6000, 5000, 4000, 3000, 2000, 1000};
            initials = new List<string> { "KTZ", "SF", "HBS", "HNT", "BLS", "SWP", "BS", "RZR", "PWS", "AAA" };
            WriteToFile();
        }
        else
        {
            highScores = new List<int>(10);
            initials = new List<string>(10);
            ReadFromFile();
        }
	}

    void Start()
    {
        newInitials = GetComponent<GameUI>().InitialsField;
        newInitials.gameObject.SetActive(false);
    }

    /*void OnApplicationQuit()
    {
        WriteToFile();
    }*/

    void OnDestroy()
    {
        WriteToFile();
    }

    public void loadScores(UnityEngine.UI.Text t, UnityEngine.UI.Text[] hs)
    {
        t.text = "Top Aces\n";
        for (int i = 0; i < 10; i++)
        {
            //hs[i].text = (i + 1) + ".".PadRight(5) + initials[i] + "".PadRight(15) + highScores[i];
            int position = i + 1;
            hs[i].text = string.Format("{0, 0} {1, 4} {2, 10}", position.ToString() + ".", initials[i], highScores[i].ToString());

        }
    }

    void UpdateHighScores(int newHighScore, int pos)
    {
        int prevScore = highScores[pos];
        string prevInitials = initials[pos];
        highScores[pos] = newHighScore;
        initials[pos] = "   ";
        for (int i = pos + 1; i < 10; i++)
        {
            int tempInt = highScores[i];
            string tempString = initials[i];
            highScores[i] = prevScore;
            initials[i] = prevInitials;
            prevScore = tempInt;
            prevInitials = tempString;
        }
    }

    public void CompareHighScores(int score)
    {
        newInitials.gameObject.SetActive(true);
        for(int i = 0; i < 10; i++)
        {
            //Debug.Log(i);
            if (highScores[i] < score)
            {
                UpdateHighScores(score, i);
                posToChange = i;
                newInitials.Select();
                return;
            }
        }

        GetComponent<GameController>().isScoreCalced = true;
    }


    public void UpdateInitials()
    {
        initials[posToChange] = newInitials.text;
        UnityEngine.UI.Text t = GetComponent<GameUI>().RequestPlacementText(posToChange);
        int position = (posToChange + 1);
        t.text =  string.Format("{0, 0} {1, 4} {2, 10}", position.ToString() + ".", initials[posToChange], highScores[posToChange].ToString());
        newInitials.text = "";
        GetComponent<GameController>().isScoreCalced = true;
        newInitials.gameObject.SetActive(false);
    }
}
