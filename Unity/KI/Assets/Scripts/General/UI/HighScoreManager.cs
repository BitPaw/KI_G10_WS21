using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour {

    private static HighScoreManager instance;

    public Transform HighScorePanel;
    public GameObject OriginalText;

    private List<UnityEngine.UI.Text> highScores;
	// Use this for initialization
	void Awake () {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        highScores = new List<UnityEngine.UI.Text>();
        for (int i = 0; i < 10; i++)
        {
            highScores.Add((Instantiate(OriginalText)).GetComponent<UnityEngine.UI.Text>());
            highScores[i].transform.SetParent(HighScorePanel);
            highScores[i].text = i + 1 + ": Unknown";

        }

    }
	

    public static void SetHighScores(List<Individual> topIndividuals)
    {
        if (topIndividuals == null)
            return;
        Debug.Log("Setting High Scores");
        int length = Mathf.Min(10, topIndividuals[0].GeneSequence.Length);
        for(int i = 0; i < 10 && i<topIndividuals.Count; i++)
        {
            instance.highScores[i].text = i + 1 + ": " + topIndividuals[i].geneSequence.Substring(0, length) + " - " + topIndividuals[i].fitnessValue;
        }
    }

    public static void Reset()
    {
        int i = 0;
        foreach(UnityEngine.UI.Text text in instance.highScores)
        {
            text.text = ++i + ": Unknown";
        }
    }
}
