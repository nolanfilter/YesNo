using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScenarioAgent : MonoBehaviour {
    
    public enum ScenarioType
    {
        Home = 0,
        YesNo = 1,
        YesNothing = 2,
        MediaGallery = 3,
        CinemaDome = 4,
        BinaryChoice = 5,
        NoseNinja = 6,
        Invalid = 7,
    }
    private ScenarioType currentScenario = ScenarioType.Invalid;

    public GameObject[] scenarioPrefabs;
    private GameObject currentScenarioObject = null;

    private static ScenarioAgent mInstance = null;
    public static ScenarioAgent instance
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
            Debug.LogError( string.Format( "Only one instance of ScenarioAgent allowed! Destroying:" + gameObject.name + ", Other:" + mInstance.gameObject.name ) );
            Destroy( gameObject );
            return;
        }

        mInstance = this;

        ChangeScenario( ScenarioType.Home );
    }

    public static void ChangeScenario( ScenarioType newScenario )
    {
        if( instance )
            instance.internalChangeScenario( newScenario );
    }

    private void internalChangeScenario( ScenarioType newScenario )
    {
        if( newScenario == currentScenario )
            return;

        Destroy( currentScenarioObject );

        currentScenario = newScenario;

        int scenarioIndex = (int)currentScenario;

        if( scenarioIndex < scenarioPrefabs.Length && scenarioPrefabs[ scenarioIndex ] != null )
            currentScenarioObject = Instantiate( scenarioPrefabs[ scenarioIndex ] ) as GameObject;

        StopCoroutine( "SetCardboardEnabled" );
        StartCoroutine( "SetCardboardEnabled", ( currentScenario != ScenarioType.Home && currentScenario != ScenarioType.Invalid ) );
    }

    private IEnumerator SetCardboardEnabled( bool enabled )
    {
        while( Cardboard.SDK == null )
            yield return null;

        Cardboard.SDK.VRModeEnabled = enabled;
    }
}
