using UnityEngine;
using System.Collections;

public class TouchDownCallback : Callback {
	
    void OnEnable()
    {
		FingerGestures.OnFingerDown += OnTouchDown;
	}

	void OnDisable()
	{
		FingerGestures.OnFingerDown -= OnTouchDown;
	}

	private void OnTouchDown( int fingerIndex, Vector2 fingerPos )
	{
		if( RectTransformUtility.RectangleContainsScreenPoint( rectTransform, fingerPos, null ) )
		{			
            RaiseTouch();
		}
	}
}
