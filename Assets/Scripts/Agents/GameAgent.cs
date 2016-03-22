using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameAgent : MonoBehaviour {

    public enum Mode
    {
        Yes = 0,
        No = 1,
    }
    private Mode currentMode = Mode.No;

    public static float TimerBaseDuration = 10f;
    public static float TimerDecreaseAmount = 0.5f;
    public static float TimerMinAmount = 1f;

    public Transform target;

    public GameObject canvasPrefab;

    private float beginSightHeight = 0f;

    private FeedbackController feedbackController = null;

    private int score = 0;
    private int highScore = 0;
    private string highScoreString = "highScore";

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

            Transform child = go.transform.GetChild( 0 );

            if( child )
            {
                child.localPosition = new Vector3( 0f, 0f, Camera.main.farClipPlane - 2f );
            }

            Canvas canvas = go.GetComponent<Canvas>();

            if( canvas )
            {
                canvas.worldCamera = Camera.main;
            }

            CanvasController canvasController = go.GetComponent<CanvasController>();

            if( canvasController )
            {
                canvasController.target = Camera.main.transform;
            }

            feedbackController = go.GetComponent<FeedbackController>();

            if( feedbackController )
            {
                feedbackController.Reset();
            }
        }

        if( PlayerPrefs.HasKey( highScoreString ) )
        {
            highScore = PlayerPrefs.GetInt( highScoreString );
        }
        else
        {
            highScore = 0;
        }
    }

    void Start()
    {
        ScreenAgent.ChangeToScreen( ScreenAgent.ScreenType.Start );
    }

    void OnEnable()
    {
        HeadGestureRecognizer.OnGestureBegan += GestureBegan;
        HeadGestureRecognizer.OnGestureMoved += GestureMoved;
        HeadGestureRecognizer.OnGestureEnded += GestureEnded;
    }

    void OnDisable()
    {
        HeadGestureRecognizer.OnGestureBegan -= GestureBegan;
        HeadGestureRecognizer.OnGestureMoved -= GestureMoved;
        HeadGestureRecognizer.OnGestureEnded -= GestureEnded;
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

    private void GestureBegan( HeadGestureRecognizer.GestureType type )
    {
        if( target )
        {
            beginSightHeight = Mathf.Abs( target.forward.y );
        }

        //Debug.Log( "" + type + " Gesture Began" );
    }

    private void GestureMoved( HeadGestureRecognizer.GestureType type )
    {
        if( !IsValid() )
            return;

        //Debug.Log( "" + type + " Gesture Moved" );

        if( feedbackController )
            feedbackController.UpdateFeedback( type );
    }

    private void GestureEnded( HeadGestureRecognizer.GestureType type )
    {
        if( !IsValid() )
            return;

        //Debug.Log( "" + type + " Gesture Ended" );

        if( feedbackController )
            feedbackController.FinishFeedback( type );
    }

    private bool IsValid()
    {
        return ( ( target == null ) || ( beginSightHeight < 0.75f && Mathf.Abs( target.forward.y ) < 0.75f ) );
    }

    public static Mode GetMode()
    {
        if( instance )
            return instance.internalGetMode();

        return Mode.No;
    }

    private Mode internalGetMode()
    {
        return currentMode;
    }

    public static void SetMode( Mode mode )
    {
        if( instance )
            instance.internalSetMode( mode );
    }

    private void internalSetMode( Mode mode )
    {
        currentMode = mode;
    }

    public static int GetScore()
    {
        if( instance )
            return instance.internalGetScore();

        return 0;
    }

    private int internalGetScore()
    {
        return score;
    }

    public static int GetHighScore()
    {
        if( instance )
            return instance.internalGetHighScore();

        return 0;
    }

    private int internalGetHighScore()
    {
        return highScore;
    }

    public static void ResetScore()
    {
        if( instance )
            instance.internalResetScore();
    }

    private void internalResetScore()
    {
        score = 0;
    }

    public static void IncrementScore()
    {
        if( instance )
            instance.internalIncrementScore();
    }

    private void internalIncrementScore()
    {
        score++;
    }

    public static bool EvaluateScore()
    {
        if( instance )
            return instance.internalEvaluateScore();

        return false;
    }

    private bool internalEvaluateScore()
    {
        if( score > highScore )
        {
            highScore = score;
            PlayerPrefs.SetInt( highScoreString, highScore );

            return true;
        }

        return false;
    }
}
