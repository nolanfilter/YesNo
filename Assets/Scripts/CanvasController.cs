using UnityEngine;
using System.Collections;

public class CanvasController : MonoBehaviour {

    public Transform target;
    public float offset;

    void LateUpdate()
    {
        if( target )
        {
            transform.eulerAngles = new Vector3( 0f, target.eulerAngles.y + offset, 0f );
        }
    }
}
