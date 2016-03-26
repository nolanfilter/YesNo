using UnityEngine;
using System.Collections;

public class HomeScenarioCanvas : MonoBehaviour {

    public Callback yesNoCallback;
    public Callback yesNothingCallback;
    public Callback mediaGalleryCallback;
    public Callback cinemaDomeCallback;
    public Callback binarychoiceCallback;
    public Callback noseNinjaCallback;


    void OnEnable()
    {
        if( yesNoCallback != null )
            yesNoCallback.OnAreaTouch += OnYesNoTouch;

        if( yesNothingCallback != null )
            yesNothingCallback.OnAreaTouch += OnYesNoTouch;

        if( mediaGalleryCallback != null )
            mediaGalleryCallback.OnAreaTouch += OnMediaGalleryTouch;

        if( cinemaDomeCallback != null )
            cinemaDomeCallback.OnAreaTouch += OnCinemaDomeTouch;

        if( binarychoiceCallback != null )
            binarychoiceCallback.OnAreaTouch += OnBinaryChoiceTouch;

        if( noseNinjaCallback != null )
            noseNinjaCallback.OnAreaTouch += OnNoseNinjaTouch;
    }

    void OnDisable()
    {
        if( yesNoCallback != null )
            yesNoCallback.OnAreaTouch -= OnYesNoTouch;

        if( yesNothingCallback != null )
            yesNothingCallback.OnAreaTouch -= OnYesNoTouch;

        if( mediaGalleryCallback != null )
            mediaGalleryCallback.OnAreaTouch -= OnMediaGalleryTouch;

        if( cinemaDomeCallback != null )
            cinemaDomeCallback.OnAreaTouch -= OnCinemaDomeTouch;

        if( binarychoiceCallback != null )
            binarychoiceCallback.OnAreaTouch -= OnBinaryChoiceTouch;

        if( noseNinjaCallback != null )
            noseNinjaCallback.OnAreaTouch -= OnNoseNinjaTouch;
    }

    private void OnYesNoTouch()
    {
        ScenarioAgent.ChangeScenario( ScenarioAgent.ScenarioType.YesNo );
    }

    private void OnYesNothingTouch()
    {
        ScenarioAgent.ChangeScenario( ScenarioAgent.ScenarioType.YesNothing );
    }

    private void OnMediaGalleryTouch()
    {
        ScenarioAgent.ChangeScenario( ScenarioAgent.ScenarioType.MediaGallery );
    }

    private void OnCinemaDomeTouch()
    {
        ScenarioAgent.ChangeScenario( ScenarioAgent.ScenarioType.CinemaDome );
    }

    private void OnBinaryChoiceTouch()
    {
        ScenarioAgent.ChangeScenario( ScenarioAgent.ScenarioType.BinaryChoice );
    }

    private void OnNoseNinjaTouch()
    {
        ScenarioAgent.ChangeScenario( ScenarioAgent.ScenarioType.NoseNinja );
    }
}
