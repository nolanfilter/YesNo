using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class YesNothingController : MonoBehaviour {

    public GameObject sectorPrefab;
    public GameObject heroPrefab;

    //hardcoded
    private SectorController[] sectorControllers = new SectorController[ 20 ];

    private GameObject heroObject;

    private float rotationalVelocity = -10f;

    private float currentOffset = 0f;

    void Awake()
    {
        if( sectorPrefab )
        {
            GameObject go;

            for( int i = 0; i < sectorControllers.Length; i++ )
            {
                go = Instantiate( sectorPrefab, Vector3.up * -3.5f, Quaternion.AngleAxis( i * 18f, Vector3.up ) ) as GameObject;

                sectorControllers[ i ] = go.GetComponent<SectorController>();
            }
        }

        if( heroPrefab )
        {
            heroObject = Instantiate( heroPrefab, Vector3.up * -1.85f, Quaternion.identity ) as GameObject;;
        }

        StartCoroutine( "DoSteppedRotation" );
    }

    private void SetOffset( float offset )
    {
        currentOffset = offset;

        for( int i = 0; i < sectorControllers.Length; i++ )
        {
            if( sectorControllers[ i ] )
            {
                //hardcoded
                sectorControllers[ i ].transform.rotation = Quaternion.AngleAxis( ( i + offset ) * 18f, Vector3.up );
            }
        }
    }

    private IEnumerator DoSteppedRotation()
    {
        while( true )
        {
            yield return new WaitForSeconds( 0.75f );

            float duration = 0.25f;
            float currentTime = 0f;
            float lerp;

            float fromOffset = currentOffset;
            float toOffset = fromOffset - 1f;

            do
            {
                currentTime += Time.deltaTime;
                lerp = Mathf.Clamp01( currentTime / duration );
                lerp = Mathf.Clamp01( lerp * lerp * 3f - lerp * lerp * lerp * 2f );

                SetOffset( Mathf.Lerp( fromOffset, toOffset, lerp ) );

                yield return null;

            } while( currentTime < duration );

            SetOffset( toOffset );
        }
    }
}
