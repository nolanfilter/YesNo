using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContentAgent : MonoBehaviour {

    private string[] positiveContent;
    private string[] negativeContent;

    private List<int> contentDeck = new List<int>();
    private string currentContent = "";
    private bool isCurrentContentPositive = false;

    private static ContentAgent mInstance = null;
    public static ContentAgent instance
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
            Debug.LogError( string.Format( "Only one instance of ContentAgent allowed! Destroying:" + gameObject.name + ", Other:" + mInstance.gameObject.name ) );
            Destroy( gameObject );
            return;
        }

        mInstance = this;

        Import();
    }

    private void Import()
    {
        TextAsset contentText = Resources.Load<TextAsset>( "content" );

        string[] contentLines = contentText.text.Split( '\n' );

        positiveContent = new string[ contentLines.Length ];
        negativeContent = new string[ contentLines.Length ];

        string[] line;

        for( int i = 0; i < contentLines.Length; i++ )
        {
            line = contentLines[ i ].Split( ',' );

            positiveContent[ i ] = line[ 0 ];
            negativeContent[ i ] = line[ 1 ];
        }
    }

    public static void SetNextContent()
    {
        if( instance )
            instance.internalSetNextContent();
    }

    private void internalSetNextContent()
    {
        if( contentDeck.Count == 0 )
        {
            for( int i = 0; i < positiveContent.Length + negativeContent.Length; i++ )
            {
                contentDeck.Add( i );
            }
        }

        int randomIndex = Random.Range( 0, contentDeck.Count );

        int contentIndex = contentDeck[ randomIndex ];

        if( contentIndex > positiveContent.Length - 1 )
        {
            currentContent = negativeContent[ contentIndex - positiveContent.Length ];
            //currentContent = "No";
            isCurrentContentPositive = false;
        }
        else
        {
            currentContent = positiveContent[ contentIndex ];
            //currentContent = "Yes";
            isCurrentContentPositive = true;
        }

        contentDeck.RemoveAt( randomIndex );
    }

    public static string GetCurrentContent()
    {
        if( instance )
            return instance.internalGetCurrentContent();

        return "";
    }

    private string internalGetCurrentContent()
    {
        return currentContent;
    }

    public static bool GetIsCurrentContentPositive()
    {
        if( instance )
            return instance.internalGetIsCurrentContentPositive();

        return false;
    }

    private bool internalGetIsCurrentContentPositive()
    {
        return isCurrentContentPositive;
    }
}
