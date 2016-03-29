using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CinemaDomeController : MonoBehaviour {

    public Callback resetCallback;
    public Callback domeAngleCallback;
    public Text domeAngleText;
    public DomeController domeController;

    public GameObject canvasRootObject;

    private int numDomeAngleTypes;

    void Awake()
    {
        numDomeAngleTypes = Enum.GetNames( typeof( DomeController.DomeAngle ) ).Length - 1;

        if( canvasRootObject )
        {
            canvasRootObject.transform.parent = null;
        }

        OnResetTouch();
    }

    void OnEnable()
    {
        if( resetCallback != null )
            resetCallback.OnAreaTouch += OnResetTouch;

        if( domeAngleCallback != null )
            domeAngleCallback.OnAreaTouch += OnDomeAngleTouch;
    }

    void OnDisable()
    {
        if( resetCallback != null )
            resetCallback.OnAreaTouch -= OnResetTouch;

        if( domeAngleCallback != null )
            domeAngleCallback.OnAreaTouch -= OnDomeAngleTouch;
    }

    void OnDestroy()
    {
        Destroy( canvasRootObject );
    }

    private void OnDomeAngleTouch()
    {
        if( domeController != null )
        {
            DomeController.DomeAngle newDomeAngle =  (DomeController.DomeAngle)( ( (int)domeController.currentDomeAngle + 1 )%numDomeAngleTypes );

            domeController.currentDomeAngle = newDomeAngle;

            if( domeAngleText )
            {
                domeAngleText.text = "" + newDomeAngle;
            }
        }
    }

    private void OnResetTouch()
    {
        Transform target = ScenarioAgent.GetTarget();

        if( target )
        {
            transform.eulerAngles = new Vector3( 0f, target.eulerAngles.y, 0f );
        }
    }
}
