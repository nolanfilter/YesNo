using UnityEngine;
using System.Collections;

[RequireComponent( typeof( RectTransform ) )]
public class Callback : MonoBehaviour {

    public delegate void AreaTouch();
    public event AreaTouch OnAreaTouch;

    public delegate void AreaTouchWithCallback( Callback callback );
    public event AreaTouchWithCallback OnAreaTouchWithCallback;

    [HideInInspector]
    public RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void RaiseTouch()
    {
        if( OnAreaTouch != null )
            OnAreaTouch();

        if( OnAreaTouchWithCallback != null )
            OnAreaTouchWithCallback( this );
    }
}