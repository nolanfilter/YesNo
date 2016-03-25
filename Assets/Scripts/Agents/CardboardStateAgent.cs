using UnityEngine;
using System.Collections;

public class CardboardStateAgent : MonoBehaviour {

    private static CardboardStateAgent mInstance = null;
    public static CardboardStateAgent instance
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
            Debug.LogError( string.Format( "Only one instance of CardboardStateAgent allowed! Destroying:" + gameObject.name + ", Other:" + mInstance.gameObject.name ) );
            Destroy( gameObject );
            return;
        }

        mInstance = this;
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
}
