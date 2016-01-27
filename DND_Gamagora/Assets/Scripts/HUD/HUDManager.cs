using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Game;

public class HUDManager : MonoBehaviour {

    #region Singleton Stuff
    private static HUDManager		_instance		= null;
    private static readonly object	singletonLock	= new object ( );
    #endregion

    Dictionary<Type_HUD, HUDElement> elements;

    public static HUDManager instance {
        get {
            lock ( singletonLock ) {
                if ( _instance == null ) {
                    _instance = ( HUDManager ) GameObject.Find ( "HUD" ).GetComponent<HUDManager> ( );
                }
                return _instance;
            }
        }
    }


    public void registerElement (Type_HUD key, HUDElement element ) {
        if ( elements == null )
            elements = new Dictionary<Type_HUD, HUDElement> ( );

        elements.Add ( key, element );
    }


    public void setLife(float value) // [0 , 1]
    {
        elements[Type_HUD.Life].setFillAmount(value);
    }


    public void setStamina(float value) // [0 , 1]
    {
        elements[Type_HUD.Stamina].setFillAmount(value);
    }


    public void setSpecial(float value) // [0 , 1]
    {
        elements[Type_HUD.Special].setFillAmount(value);
    }


}
