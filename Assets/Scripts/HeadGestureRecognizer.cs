using UnityEngine;
using System.Collections;

public class HeadGestureRecognizer : MonoBehaviour {

    public enum GestureType
    {
        Shake = 0,
        Nod = 1,
        Invalid = 2,
    }
        
    public delegate void GestureBegan( GestureType type );
    public static event GestureBegan OnGestureBegan;

    public delegate void GestureMoved( GestureType type );
    public static event GestureMoved OnGestureMoved;

    public delegate void GestureEnded( GestureType type );
    public static event GestureEnded OnGestureEnded;

    //public delegate void GestureCancelled( GestureType type );
    //public static event GestureCancelled OnGestureCancelled;

    public Transform target;

    private Quaternion oldRotation = Quaternion.identity;
    private float beginThreshold = 1.5f;

    private float endThreshold = 0.5f;
    private float endTime = 0f;
    private float endDuration = 0.15f;

    private Vector2 absoluteMovement = Vector2.zero;

    private bool gestureActive = false;

    private static HeadGestureRecognizer mInstance = null;
    public static HeadGestureRecognizer instance
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
            Debug.LogError( string.Format( "Only one instance of HeadGestureRecognizer allowed! Destroying:" + gameObject.name + ", Other:" + mInstance.gameObject.name ) );
            Destroy( gameObject );
            return;
        }

        mInstance = this;
    }

    void Update()
    {
        Quaternion currentRotation = Quaternion.identity;

        if( target )
        {
            currentRotation = target.rotation;
        }
        else
        {
            currentRotation = transform.rotation;
        }

        float angle = Quaternion.Angle( currentRotation, oldRotation );

        if( !gestureActive )
        {
            if( angle > beginThreshold )
            {
                gestureActive = true;

                endTime = 0f;

                absoluteMovement = new Vector2( Mathf.Abs( currentRotation.eulerAngles.x - oldRotation.eulerAngles.x ), Mathf.Abs( currentRotation.eulerAngles.y - oldRotation.eulerAngles.y ) );

                if( OnGestureBegan != null )
                    OnGestureBegan( GetTypeFromMovement( absoluteMovement ) );
            }
        }
        else
        {                            
            absoluteMovement += new Vector2( Mathf.Abs( currentRotation.eulerAngles.x - oldRotation.eulerAngles.x ), Mathf.Abs( currentRotation.eulerAngles.y - oldRotation.eulerAngles.y ) );

            if( endTime > endDuration )
            {
                gestureActive = false;

                if( OnGestureEnded != null )
                    OnGestureEnded( GetTypeFromMovement( absoluteMovement ) );

                absoluteMovement = Vector2.zero;
            }
            else
            {
                if( OnGestureMoved != null )
                    OnGestureMoved( GetTypeFromMovement( absoluteMovement ) );

                if( angle < endThreshold )
                {
                    endTime += Time.deltaTime;
                }
                else
                {
                    endTime = 0f;
                }
            }
        }
            
        oldRotation = currentRotation;
    }

    private GestureType GetTypeFromMovement( Vector2 movement )
    {
        GestureType type = GestureType.Invalid;

        if( Mathf.Abs( movement.x - movement.y ) > movement.magnitude * 0.75f )
        {
            if( movement.x > movement.y )
            {
                type = GestureType.Nod;
            }
            else
            {
                type = GestureType.Shake;
            }
        }

        return type;
    }
}
