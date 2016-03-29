using UnityEngine;
using System.Collections;

public class BackCanvasController : MonoBehaviour {

    public Callback backCallback;

    void OnEnable()
    {
        if( backCallback != null )
            backCallback.OnAreaTouch += OnBackTouch;
    }

    void OnDisable()
    {
        if( backCallback != null )
            backCallback.OnAreaTouch -= OnBackTouch;
    }

    private void OnBackTouch()
    {
        ScenarioAgent.ChangeScenario( ScenarioAgent.ScenarioType.Home );
    }
}
