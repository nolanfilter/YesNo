using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MovieController : MonoBehaviour {

    public bool loop;

    private MovieTexture movieTexture = null;

    void Awake()
    {
        RawImage rawImage = GetComponent<RawImage>();

        if( rawImage )
        {
            movieTexture = (MovieTexture)rawImage.mainTexture;
        }
    }

    void Start()
    {
        StartCoroutine( "DoMovie" );
    }

    private IEnumerator DoMovie()
    {
        if( movieTexture == null )
            yield break;

        while( loop )
        {
            movieTexture.Stop();
            movieTexture.Play();

            yield return new WaitForSeconds( movieTexture.duration );
        }
    }
}
