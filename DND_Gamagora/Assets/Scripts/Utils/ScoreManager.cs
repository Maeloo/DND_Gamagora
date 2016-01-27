using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : Singleton<ScoreManager>
{
    private const int SIZE_CLASSEMENT = 10;
    private int currentScore;
    protected ScoreManager() { }
	// Use this for initialization
	void Awake () {
        currentScore = 0;
	}
	
    public void AddPoint(int points)
    {
        currentScore += points;
    }

    public int [] GetClassement()
    {
        int[] res = new int[SIZE_CLASSEMENT];
        for(int i = 0; i < SIZE_CLASSEMENT; ++i)
        {
            string name = "HighScore" + (i + 1);

            res[i] = PlayerPrefs.GetInt(name, 0);

          //  Debug.logger.Log(name + " " + res[i]);
        }
        return res;
    }

    void SaveClassement(int [] classement)
    {
        for(int i = 0; i < SIZE_CLASSEMENT; ++i)
        {
            string name = "HighScore" + (i + 1);

            Debug.logger.Log(name + " " + classement[i]);
            PlayerPrefs.SetInt(name, classement[i]);
        }
    }
    void SaveScore()
    {
        int[] res = GetClassement();
        int i = 0;
        for(; i < SIZE_CLASSEMENT; ++i)
        {
            if(res[i] < currentScore)
            {
                break;
            }
        }
        if(i < SIZE_CLASSEMENT)
        {
            int previousScore = res[i];
            int tmp;
            res[i] = currentScore;
            Debug.logger.Log(" current score " + res[i]);
            ++i;
            for(; i < SIZE_CLASSEMENT; ++i)
            {
                tmp = res[i];
                res[i] = previousScore;
                previousScore = tmp;
            }
            SaveClassement(res);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddPoint(10);

            Debug.logger.Log("Score : " + currentScore);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.logger.Log("SaveScore");
            SaveScore();
        }
    }

}
