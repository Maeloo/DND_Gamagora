using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : Singleton<ScoreManager>
{
    internal const int SIZE_CLASSEMENT = 10;

    private int currentScore;

    protected ScoreManager() { }
	
    
	void Awake () {
        currentScore = 0;
        DontDestroyOnLoad(gameObject);
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

            //Debug.logger.Log(name + " " + res[i]);
            res[i] = PlayerPrefs.GetInt(name, 0);
        }
        return res;
    }
    public string[] GetClassementName()
    {
        string[] res = new string[SIZE_CLASSEMENT];
        for (int i = 0; i < SIZE_CLASSEMENT; ++i)
        {
            string name = "HighScore" + (i + 1) + "name";

            //Debug.logger.Log(name + " " + res[i]);
            res[i] = PlayerPrefs.GetString(name);
        }
        return res;
    }

    public void SaveClassement(int [] classement)
    {
        for(int i = 0; i < SIZE_CLASSEMENT; ++i)
        {
            string name = "HighScore" + (i + 1);

            //Debug.logger.Log(name + " " + classement[i]);
            PlayerPrefs.SetInt(name, classement[i]);
        }
    }

    public int SaveScore()
    {
        int[] res = GetClassement();
        string [] resString = GetClassementName();
        int i = 0;
        for(; i < SIZE_CLASSEMENT; ++i)
        {
            if(res[i] <= currentScore)
            {
                break;
            }
        }
        int ret = i;
        if(i < SIZE_CLASSEMENT)
        {
            int previousScore = res[i];
            int tmp;
            string previousName = resString[i];
            string tmpStr;
            res[i] = currentScore;
            ++i;
            for(; i < SIZE_CLASSEMENT; ++i)
            {
                tmp = res[i];
                res[i] = previousScore;
                previousScore = tmp;

                tmpStr = resString[i];
                resString[i] = previousName;
                previousName = tmpStr;
            }
            SaveClassement(res);
            AddName(ret, "");
            return ret;
        }
        return -1;
    }

    public void AddName(int indice, string name)
    {
        string nameIndice = "HighScore" + (indice + 1) + "name";
        PlayerPrefs.SetString(nameIndice, name);
    }
    void Update()
    {
    }

}
