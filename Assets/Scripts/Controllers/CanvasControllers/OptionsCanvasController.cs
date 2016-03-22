using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsCanvasController : MonoBehaviour {

    public Text modeText;

    void Awake()
    {
        if( modeText )
        {
            modeText.text = GameAgent.GetMode().ToString();
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
            case HeadGestureRecognizer.GestureType.Nod: ScreenAgent.ChangeToScreen( ScreenAgent.ScreenType.Start ); break;
            case HeadGestureRecognizer.GestureType.Shake:
            {
                switch( GameAgent.GetMode() )
                {
                    case GameAgent.Mode.Yes: GameAgent.SetMode( GameAgent.Mode.No ); break;
                    case GameAgent.Mode.No: GameAgent.SetMode( GameAgent.Mode.Yes ); break;
                }

                if( modeText )
                {
                        modeText.text = GameAgent.GetMode().ToString();
                }
            } break;
        }
    }
}
