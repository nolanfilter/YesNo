using UnityEngine;
using System.Collections;

public class MediaGalleryController : MonoBehaviour {

    private float yOffset = 90f;

    private float duration = 30f;
    private float inverseDuration;

    private float range = 2000f;

    private float beginTime;

    void Awake()
    {
        beginTime = Time.time;

        inverseDuration = 1f / duration;

        Transform target = ScenarioAgent.GetTarget();

        if( target )
        {
            transform.parent = target;
            transform.localEulerAngles = Vector3.up * yOffset;
        }
    }

    void Update()
    {
        float xOffset = Mathf.Sin( ( Time.time - beginTime ) * 2f * Mathf.PI * inverseDuration ) * range;

        transform.localPosition = new Vector3( xOffset, 0f, 0f );
    }
}
