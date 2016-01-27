using UnityEngine;
using System.Collections;

public class Back : MonoBehaviour {

	public void backToHome()
    {
        GameManager.Instance.LoadScene("Menu");
    }
}
