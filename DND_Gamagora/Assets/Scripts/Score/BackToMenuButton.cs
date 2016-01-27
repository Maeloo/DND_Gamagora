using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class BackToMenuButton : MonoBehaviour {
    public void BackToMenu()
    {
        GameManager.Instance.LoadScene("Menu");
    }
	
}
