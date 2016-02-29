using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameAgent : MonoBehaviour {

    public Transform target;

    public GameObject canvasPrefab;

    private Image feedbackImage = null;
    private float oldSightHeight = 0f;

    private static GameAgent mInstance = null;
    public static GameAgent instance
    {
        get
        {
            return mInstance;
        }
    }

    void Awake()
    {
        if( mInstance != null )
        {
            Debug.LogError( string.Format( "Only one instance of GameAgent allowed! Destroying:" + gameObject.name + ", Other:" + mInstance.gameObject.name ) );
            Destroy( gameObject );
            return;
        }

        mInstance = this;

        if( canvasPrefab )
        {
            GameObject go = Instantiate( canvasPrefab ) as GameObject;

            go.transform.parent = Camera.main.transform;
            go.transform.localPosition = new Vector3( 0f, 0f, Camera.main.nearClipPlane + 0.1f );
            go.transform.localRotation = Quaternion.identity;

            Canvas canvas = go.GetComponent<Canvas>();

            if( canvas )
            {
                canvas.worldCamera = Camera.main;
            }

            feedbackImage = go.GetComponentInChildren<Image>();

            if( feedbackImage )
            {
                feedbackImage.color = Color.black;
            }
        }
    }

    void OnEnable()
    {
        GestureRecognizer.OnGestureBegan += GestureBegan;
        GestureRecognizer.OnGestureMoved += GestureMoved;
        GestureRecognizer.OnGestureEnded += GestureEnded;
    }

    void OnDisable()
    {
        GestureRecognizer.OnGestureBegan -= GestureBegan;
        GestureRecognizer.OnGestureMoved -= GestureMoved;
        GestureRecognizer.OnGestureEnded -= GestureEnded;
    }

    void Update()
    {
        bool toggle = false;

        if( Application.isEditor )
        {
            if( Input.GetKeyDown( KeyCode.Space ) )
            {
                toggle = true;
            }
        }
        else
        {
            for( int i = 0; i < Input.touches.Length; i++ )
            {
                if( Input.touches[ i ].phase == TouchPhase.Began )
                {
                    toggle = true;
                }
            }
        }

        if( toggle )
        {
            Cardboard.SDK.VRModeEnabled = !Cardboard.SDK.VRModeEnabled;
        }
    }

    private void GestureBegan( GestureRecognizer.GestureType type )
    {
        //Debug.Log( "" + type + " Gesture Began" );
    }

    private void GestureMoved( GestureRecognizer.GestureType type )
    {
        //Debug.Log( "" + type + " Gesture Moved" );
    }

    private void GestureEnded( GestureRecognizer.GestureType type )
    {
        if( target )
        {
            float currentSightHeight = Mathf.Abs( target.forward.y );

            bool isValid = true;

            if( oldSightHeight > 0.75f || currentSightHeight > 0.75f )
            {
                isValid = false;
            }

            oldSightHeight = currentSightHeight;

            if( !isValid )
                return;
        }

        //Debug.Log( "" + type + " Gesture Ended" );

        StopCoroutine( "DoFeedbackColor" );

        Color color;

        switch( type )
        {
            case GestureRecognizer.GestureType.Shake: color = Color.red; break;
            case GestureRecognizer.GestureType.Nod: color = Color.green; break;
            default: color = Color.white; break;
        }

        StartCoroutine( "DoFeedbackColor", color );
    }

    private IEnumerator DoFeedbackColor( Color color )
    {
        if( feedbackImage == null )
            yield break;

        feedbackImage.color = color;

        yield return new WaitForSeconds( 0.5f );

        feedbackImage.color = Color.black;
    }
}
