using UnityEngine;
using System.Collections;

public class UIManagerHowToPlay : MonoBehaviour
{

    [SerializeField]
    PanelUI _joystickPanel;

    [SerializeField]
    PanelUI _keyboardPanel;

    [SerializeField]
    PanelUI _itemEnemyPanel;



    public void awake()
    {
        _joystickPanel.display();
        _keyboardPanel.hide();
        _itemEnemyPanel.hide();
    }


    public void onArrowClick()
    {
        if (_joystickPanel && _joystickPanel.isDisplayed)
        {
            _joystickPanel.hide();
            _keyboardPanel.display();
        }
        else if (_keyboardPanel && _keyboardPanel.isDisplayed)
        {
            _keyboardPanel.hide();
            _itemEnemyPanel.display();
        }
        else if (_itemEnemyPanel && _itemEnemyPanel.isDisplayed)
        {
            _itemEnemyPanel.hide();
            _joystickPanel.display();
        }
    }
}

