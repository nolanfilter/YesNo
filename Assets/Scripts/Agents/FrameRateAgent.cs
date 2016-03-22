using UnityEngine;
using System.Collections;

public class FrameRateAgent : MonoBehaviour {

    private static FrameRateAgent mInstance = null;
    public static FrameRateAgent instance
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
            Debug.LogError( string.Format( "Only one instance of FrameRateAgent allowed! Destroying:" + gameObject.name + ", Other:" + mInstance.gameObject.name ) );
            Destroy( gameObject );
            return;
        }

        mInstance = this;

        Application.targetFrameRate = 60;
    }
}
