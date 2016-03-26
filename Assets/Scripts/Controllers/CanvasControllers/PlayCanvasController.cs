using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayCanvasController : MonoBehaviour {

    public Text descriptionText;
    public Text contentText;

    private float timerDuration;
    private Color whiteClear = new Color( 1f, 1f, 1f, 0f );

    void Awake()
    {
        if( descriptionText )
        {
            descriptionText.text = YesNoAgent.GetMode().ToString().ToUpper() + "?";
        }

        timerDuration = YesNoAgent.TimerBaseDuration;

        YesNoAgent.ResetScore();
        UpdateContent();
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
        if( type == HeadGestureRecognizer.GestureType.Invalid )
            return;

        if( YesNoAgent.GetMode() == YesNoAgent.Mode.Yes )
        {
            if( ( type == HeadGestureRecognizer.GestureType.Nod && ContentAgent.GetIsCurrentContentPositive() )
                || ( type == HeadGestureRecognizer.GestureType.Shake && !ContentAgent.GetIsCurrentContentPositive() ) )
            {
                Advance();
            }
            else
            {
                End();
            }
        }
        else
        {
            if( ( type == HeadGestureRecognizer.GestureType.Shake && ContentAgent.GetIsCurrentContentPositive() )
                || ( type == HeadGestureRecognizer.GestureType.Nod && !ContentAgent.GetIsCurrentContentPositive() ) )
            {
                Advance();
            }
            else
            {
                End();
            }
        }

    }

    private void UpdateContent()
    {
        StopCoroutine( "DoTimer" );

        ContentAgent.SetNextContent();

        if( contentText )
        {
            contentText.text = ContentAgent.GetCurrentContent();
        }

        StartCoroutine( "DoTimer" );
    }

    private void Advance()
    {
        timerDuration = Mathf.Clamp( timerDuration - YesNoAgent.TimerDecreaseAmount, YesNoAgent.TimerMinAmount, YesNoAgent.TimerBaseDuration );

        YesNoAgent.IncrementScore();
        UpdateContent();
    }

    private void End()
    {
        ScreenAgent.ChangeToScreen( ScreenAgent.ScreenType.End );
    }

    private IEnumerator DoTimer()
    {
        float currentTime = 0f;
        float lerp;

        Color color;

        if( descriptionText )
        {
            descriptionText.color = Color.white;
        }

        if( contentText )
        {
            contentText.color = Color.white;
        }

        do
        {
            currentTime += Time.deltaTime;
            lerp = Mathf.Clamp01( currentTime / timerDuration );

            color = Color.Lerp( Color.white, whiteClear, lerp );

            if( descriptionText )
            {
                descriptionText.color = color;
            }

            if( contentText )
            {
                contentText.color = color;
            }

            yield return null;

        } while( currentTime < timerDuration );

        if( descriptionText )
        {
            descriptionText.color = whiteClear;
        }

        if( contentText )
        {
            contentText.color = whiteClear;
        }

        End();
    }
}
