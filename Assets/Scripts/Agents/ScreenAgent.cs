using UnityEngine;
using System.Collections;

public class ScreenAgent : MonoBehaviour {

    public enum ScreenType
    {
        Start = 0,
        Play = 1,
        End = 2,
        Options = 3,
        Invalid = 4
    }

    public GameObject[] screenPrefabs;

    private GameObject currentScreenObject;

    private static ScreenAgent mInstance = null;
    public static ScreenAgent instance
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
            Debug.LogError( string.Format( "Only one instance of ScreenAgent allowed! Destroying:" + gameObject.name + ", Other:" + mInstance.gameObject.name ) );
            Destroy( gameObject );
            return;
        }

        mInstance = this;
    }

    public static void ChangeToScreen( ScreenType type )
    {
        if( instance )
            instance.internalChangeToScreen( type );
    }

    private void internalChangeToScreen( ScreenType type )
    {
        Destroy( currentScreenObject );

        int index = (int)type;

        if( index < screenPrefabs.Length && screenPrefabs[ index ] != null )
        {
            currentScreenObject = Instantiate( screenPrefabs[ index ] ) as GameObject;

            Canvas canvas = currentScreenObject.GetComponent<Canvas>();

            if( canvas )
            {
                canvas.worldCamera = Camera.main;
            }

            CanvasController canvasController = currentScreenObject.GetComponent<CanvasController>();

            if( canvasController )
            {
                canvasController.target = Camera.main.transform;
            }
        }
    }
}
