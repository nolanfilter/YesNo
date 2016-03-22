using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartCanvasController : MonoBehaviour {

    public Text titleText;

    void Awake()
    {
        if( titleText )
        {
            titleText.text = GameAgent.GetMode().ToString().ToUpper() + "?";
        }
    }

    void OnEnable()
    {
        HeadGestureRecognizer.OnGestureEnded += GestureEnded;
    }

    void OnDisable()
    {
        HeadGestureRecognizer.OnGestureEnded -= GestureEnded;
    }

    private void GestureEnded( HeadGestureRecognizer.GestureType type )
    {
        switch( type )
        {
            case HeadGestureRecognizer.GestureType.Nod: ScreenAgent.ChangeToScreen( ScreenAgent.ScreenType.Play ); break;
            case HeadGestureRecognizer.GestureType.Shake: ScreenAgent.ChangeToScreen( ScreenAgent.ScreenType.Options ); break;
        }
    }
}
