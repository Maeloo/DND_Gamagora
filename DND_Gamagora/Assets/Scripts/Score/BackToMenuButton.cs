using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class BackToMenuButton : MonoBehaviour {
    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.LoadScene("Menu");
    }
}
