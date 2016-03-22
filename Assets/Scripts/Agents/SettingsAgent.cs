using UnityEngine;
using System.Collections;

public class SettingsAgent : MonoBehaviour {

    private bool matchPositive;
    private string matchPositiveString = "matchPositive";

    private static SettingsAgent mInstance = null;
    public static SettingsAgent instance
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
            Debug.LogError( string.Format( "Only one instance of SettingsAgent allowed! Destroying:" + gameObject.name + ", Other:" + mInstance.gameObject.name ) );
            Destroy( gameObject );
            return;
        }

        mInstance = this;

        if( PlayerPrefs.HasKey( matchPositiveString ) )
            matchPositive = ( PlayerPrefs.GetInt( matchPositiveString ) == 1 );
        else
            SetMatchPositive( true );
    }

    public static void ToggleMatchPositive()
    {
        if( instance )
            instance.internalToggleMatchPositive();
    }

    private void internalToggleMatchPositive()
    {
        SetMatchPositive( !matchPositive );
    }

    private void SetMatchPositive( bool newMatchPositive )
    {
        matchPositive = newMatchPositive;
        PlayerPrefs.SetInt( matchPositiveString, ( matchPositive ? 1 : 0 ) );
    }
}
