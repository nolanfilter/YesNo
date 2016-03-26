using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsCanvasController : MonoBehaviour {

    public Text modeText;

    void Awake()
    {
        if( modeText )
        {
            modeText.text = YesNoAgent.GetMode().ToString();
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
                switch( YesNoAgent.GetMode() )
                {
                    case YesNoAgent.Mode.Yes: YesNoAgent.SetMode( YesNoAgent.Mode.No ); break;
                    case YesNoAgent.Mode.No: YesNoAgent.SetMode( YesNoAgent.Mode.Yes ); break;
                }

                if( modeText )
                {
                    modeText.text = YesNoAgent.GetMode().ToString();
                }
            } break;
        }
    }
}
