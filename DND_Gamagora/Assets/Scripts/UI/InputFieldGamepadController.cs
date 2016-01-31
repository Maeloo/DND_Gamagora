using UnityEngine;
using UnityEngine.UI;

public class InputFieldGamepadController : MonoBehaviour {
    public Text text;
    private float lastVertical;
    public float deltaSensibilityAxis = 1f;
    public ScoreBoard scoreBoard;
    private int pos;
	// Use this for initialization
	void Start () {
        lastVertical = Time.time;
        pos = 0;
        text.text = "";

    }
	
    private void AddCurrentLetter(int incr)
    {
        if (pos < text.text.Length)
        {
            char[] textCopy = text.text.ToCharArray();
            textCopy[pos] = System.Convert.ToChar(System.Convert.ToInt32(textCopy[pos]) + incr);
            if (textCopy[pos] > 'Z')
                textCopy[pos] = 'A';
            else if (textCopy[pos] < 'A')
                textCopy[pos] = 'Z';
            text.text = new string(textCopy);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastVertical > deltaSensibilityAxis)
        {
            if (InputManager.Instance.IsUp)
            {
                if (text.text == "")
                {
                    text.text = "Z";
                }
                else
                {
                    AddCurrentLetter(-1);
                }

                lastVertical = Time.time;
            }
            if (InputManager.Instance.IsDown)
            {
                if (text.text == "")
                {
                    text.text = "A";
                    
                }
                else
                {
                    AddCurrentLetter(1);
                }

                lastVertical = Time.time;
            }
        }
        if (InputManager.Instance.IsNext)
        {
            pos += 1;
            text.text += " ";
        }
        if (InputManager.Instance.IsCancel)
        {


            pos -= 1;
            if (text.text.Length > 1)
                text.text = text.text.Substring(0, text.text.Length - 1);
            else
            {

                text.text = "";
                pos = 0;
            }
        }
        if (InputManager.Instance.IsValidate)
        {
            scoreBoard.ValidateName();
        }
    }
}
