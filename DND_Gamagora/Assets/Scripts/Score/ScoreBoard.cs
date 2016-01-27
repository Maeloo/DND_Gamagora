using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreBoard : MonoBehaviour {
    [System.Serializable]
    public struct PositionText
    {
        public Text name;
        public Text score;
    }
    public PositionText[] scoreBoardText;
    public GameObject inputField;
    public Text nameText;
    private int currentPos;

	// Use this for initialization
	void Start () {
        currentPos = ScoreManager.Instance.SaveScore();
        LoadScoreBoardText();
        if (currentPos == -1)
        {
            inputField.SetActive(false);
        }
	}
    void LoadScoreBoardText()
    {
        int[] scores = ScoreManager.Instance.GetClassement();
        string[] names = ScoreManager.Instance.GetClassementName();
        for (int i = 0; i < 10; ++i)
        {
            scoreBoardText[i].score.text = scores[i].ToString();
            scoreBoardText[i].name.text = names[i];
        }
    }
    public void ValidateName()
    {
        ScoreManager.Instance.AddName(currentPos, nameText.text);
        inputField.SetActive(false);
        LoadScoreBoardText();
    }

}
