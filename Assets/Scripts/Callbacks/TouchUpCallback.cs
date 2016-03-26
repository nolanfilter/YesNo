using UnityEngine;
using System.Collections;

public class TouchUpCallback : Callback {

	private Vector2 beginFingerPos;
	
	private float dragMinimumDistance;

    void Start()
    {
        dragMinimumDistance = Screen.width * 0.05f;
    }

	void OnEnable()
	{
		FingerGestures.OnFingerDown += OnTouchDown;
		FingerGestures.OnFingerUp += OnTouchUp;
	}
	
	void OnDisable()
	{
		FingerGestures.OnFingerDown -= OnTouchDown;
		FingerGestures.OnFingerUp -= OnTouchUp;
	}
	
	private void OnTouchDown( int fingerIndex, Vector2 fingerPos )
	{
		beginFingerPos = fingerPos;
	}
	
	private void OnTouchUp( int fingerIndex, Vector2 fingerPos, float timeHeldDown )
	{
		if( Vector2.Distance( fingerPos, beginFingerPos ) > dragMinimumDistance )
			return;
		
		if( RectTransformUtility.RectangleContainsScreenPoint( rectTransform, fingerPos, null ) )
		{	
            RaiseTouch();
		}
	}
}
