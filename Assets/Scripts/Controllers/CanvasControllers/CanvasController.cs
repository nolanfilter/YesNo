using UnityEngine;
using System.Collections;

public class CanvasController : MonoBehaviour {

    public Transform target;
    public float offset;

    public bool constrainX;
    public bool constrainZ;

    void LateUpdate()
    {
        if( target )
        {
            transform.eulerAngles = new Vector3( ( constrainX ? 0f : target.eulerAngles.x ), target.eulerAngles.y + offset, ( constrainZ ? 0f : target.eulerAngles.z ) );
        }
    }
}
