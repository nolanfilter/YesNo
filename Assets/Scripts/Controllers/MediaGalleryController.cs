using UnityEngine;
using System.Collections;

public class MediaGalleryController : MonoBehaviour {

    public Callback resetCallback;

    public GameObject worldCanvasObject;
    public GameObject screenCanvasObject;

    public float interval = 2000f;

    public int minX = -1;
    public int maxX = 2;

    private float yOffset = 90f;

    private float duration = 40f;
    private float inverseDuration;

    private float halfRange;

    private float beginTime;

    private float baseYAngle = 0f;
    private int currentIndex;
    private float currentOffset;

    private Transform target;

    private float movementScale = 15f;

    private float navigationDuration = 0.3f;

    private float beginThreshold = 20f;
    private float endThreshold = 5f;

    private bool isMoving;
    private bool isNavigating;

    void Awake()
    {
        halfRange = ( maxX * interval - minX * interval ) * 0.5f;

        inverseDuration = 1f / duration;

        target = ScenarioAgent.GetTarget();

        if( worldCanvasObject )
        {
            if( target )
            {
                worldCanvasObject.transform.parent = target;
                worldCanvasObject.transform.localEulerAngles = Vector3.up * yOffset;
            }
        }

        if( screenCanvasObject )
        {
            screenCanvasObject.transform.parent = null;
        }

        OnResetTouch();
    }
 
    void OnEnable()
    {
        if( resetCallback != null )
            resetCallback.OnAreaTouch += OnResetTouch;
    }

    void OnDisable()
    {
        if( resetCallback != null )
            resetCallback.OnAreaTouch -= OnResetTouch;
    }

    void OnDestroy()
    {
        Destroy( worldCanvasObject );
        Destroy( screenCanvasObject );
    }

    void Update()
    {
        if( target )
        {
            float deltaAngle = Mathf.DeltaAngle( target.eulerAngles.y, baseYAngle );

            float newOffset = Mathf.Clamp( currentOffset + deltaAngle * movementScale * Time.deltaTime, minX * interval, maxX * interval );

            if( !isMoving )
            {
                if( Mathf.Abs( deltaAngle ) >= beginThreshold )
                {
                    if( isNavigating )
                    {
                        StopCoroutine( "DoNavigation" );
                        isNavigating = false;
                    }

                    SetOffset( newOffset );

                    isMoving = true;
                }
            }
            else
            {
                if( Mathf.Abs( deltaAngle ) > endThreshold )
                {
                    SetOffset( newOffset );
                }
                else
                {
                    if( !isNavigating )
                    {
                        StartCoroutine( "DoNavigation", Mathf.Round( newOffset / interval ) * interval );
                    }

                    isMoving = false;
                }
            }
        }
        else
        {
            SetOffset( minX * interval + Mathf.Sin( ( Time.time - beginTime ) * 2f * Mathf.PI * inverseDuration ) * halfRange + halfRange );
        }
    }

    private void SetOffset( float offset )
    {
        currentOffset = offset;

        if( worldCanvasObject )
        {
            worldCanvasObject.transform.localPosition = new Vector3( offset, 0f, 0f );
        }
    }
       
    private void OnResetTouch()
    {
        if( target )
        {
            baseYAngle = target.eulerAngles.y;
        }

        if( worldCanvasObject )
        {
            worldCanvasObject.transform.localPosition = Vector3.zero;
        }

        beginTime = Time.time;

        currentIndex = 0;
        currentOffset = 0f;

        isMoving = false;

        StopCoroutine( "DoNavigation" );
        isNavigating = false;
    }

    private IEnumerator DoNavigation( float toOffset )
    {
        if( currentOffset == toOffset )
            yield break;

        isNavigating = true;

        float fromOffset = currentOffset;
        float currentTime = 0f;
        float lerp;

        do
        {
            currentTime += Time.deltaTime;
            lerp = Mathf.Clamp01( currentTime / navigationDuration );

            lerp = Mathf.Pow( lerp, 0.5f );

            SetOffset( Mathf.Lerp( fromOffset, toOffset, lerp ) );

            yield return null;

        } while( currentTime < navigationDuration );

        SetOffset( toOffset );

        isNavigating = false;
    }
}
