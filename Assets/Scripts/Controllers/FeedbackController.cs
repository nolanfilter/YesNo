using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FeedbackController : MonoBehaviour {

    public Image shakeShadowImage;
    public Image nodShadowImage;
    public Image shakeImage;
    public Image nodImage;

    public Color shakeColor;
    public Color nodColor;
    private Color shakeBlankColor;
    private Color nodBlankColor;

    private float speed = 5f;
    private float duration = 0.3f;

    void Awake()
    {
        shakeBlankColor = new Color( shakeColor.r, shakeColor.g, shakeColor.b, 0f );
        nodBlankColor = new Color( nodColor.r, nodColor.g, nodColor.b, 0f );

        Reset();
    }

    public void Reset()
    {
        if( shakeShadowImage )
        {
            shakeShadowImage.transform.localScale = Vector2.one;
            shakeShadowImage.color = shakeBlankColor;
        }

        if( nodShadowImage )
        {
            nodShadowImage.transform.localScale = Vector2.one;
            nodShadowImage.color = nodBlankColor;
        }

        if( shakeImage )
        {
            shakeImage.transform.localScale = Vector2.right;
            shakeImage.color = shakeColor;
        }

        if( nodImage )
        {
            nodImage.transform.localScale = Vector2.up;
            nodImage.color = nodColor;
        }
    }

    public void UpdateFeedback( HeadGestureRecognizer.GestureType gestureType )
    {
        if( shakeImage )
        {
            if( gestureType == HeadGestureRecognizer.GestureType.Shake )
            {
                shakeImage.transform.localScale = Vector2.MoveTowards( shakeImage.transform.localScale, Vector2.one, speed * Time.deltaTime );
            }
            else
            {
                shakeImage.transform.localScale = Vector2.MoveTowards( shakeImage.transform.localScale, Vector2.right, speed * Time.deltaTime );
            }
        }

        if( nodImage )
        {
            if( gestureType == HeadGestureRecognizer.GestureType.Nod )
            {
                nodImage.transform.localScale = Vector2.MoveTowards( nodImage.transform.localScale, Vector2.one, speed * Time.deltaTime );
            }
            else
            {
                nodImage.transform.localScale = Vector2.MoveTowards( nodImage.transform.localScale, Vector2.up, speed * Time.deltaTime );
            }
        }
    }

    public void FinishFeedback( HeadGestureRecognizer.GestureType gestureType )
    {
        StopCoroutine( "DoFeedbackFinish" );

        Reset();

        StartCoroutine( "DoFeedbackFinish", gestureType );
    }

    private IEnumerator DoFeedbackFinish(  HeadGestureRecognizer.GestureType gestureType  )
    {
        Image shadowImage = null;
        Color fromColor = Color.clear;
        Color toColor = Color.clear;

        switch( gestureType )
        {
            case HeadGestureRecognizer.GestureType.Shake:
            {
                shadowImage = shakeShadowImage;
                fromColor = shakeColor;
                toColor = shakeBlankColor;
            } break;

            case HeadGestureRecognizer.GestureType.Nod:
            {
                shadowImage = nodShadowImage;
                fromColor = nodColor;
                toColor = nodBlankColor;
            } break;
        }

        if( shadowImage == null )
            yield break;

        float lerp;
        float currentTime = 0f;

        shadowImage.color = fromColor;
        shadowImage.transform.localScale = Vector2.one;

        do
        {
            currentTime += Time.deltaTime;
            lerp = Mathf.Clamp01( currentTime / duration );

            shadowImage.color = Color.Lerp( fromColor, toColor, lerp );
            shadowImage.transform.localScale = Vector2.Lerp( Vector2.one, Vector2.one * 15f, lerp );

            yield return null;

        } while( currentTime < duration );

        shadowImage.color = toColor;
        shadowImage.transform.localScale = Vector2.one;
    }
}
