using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class BackToMenuButton : MonoBehaviour {
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
