using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndCanvasController : MonoBehaviour {

    public Text descriptionText;
    public Text scoreText;
    public Text highScoreText;

    private float beginTime;
    private float bufferTime = 1f;

    void Awake()
    {
        if( GameAgent.EvaluateScore() )
        {
            if( descriptionText )
            {
                descriptionText.text = "HIGH SCORE";
            }
        }
        else
        {
            if( descriptionText )
            {
                descriptionText.text = "GAME OVER";
            }
        }

        if( scoreText )
        {
            scoreText.text = "" + GameAgent.GetScore();
        }

        if( highScoreText )
        {
            highScoreText.text = "" + GameAgent.GetHighScore();
        }

        beginTime = Time.time;
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
        if( Time.time - beginTime < bufferTime )
            return;

        switch( type )
        {
            case HeadGestureRecognizer.GestureType.Nod: ScreenAgent.ChangeToScreen( ScreenAgent.ScreenType.Play ); break;
            case HeadGestureRecognizer.GestureType.Shake: ScreenAgent.ChangeToScreen( ScreenAgent.ScreenType.Start ); break;
        }
    }
}
