using UnityEngine;
using System.Collections;

public class PixelateEffectController : MonoBehaviour {

  public float baseValue = 0.0075f;

  void Start()
    {
      transform.parent = Camera.main.transform;
      transform.localPosition = new Vector3( 0f, 0f, Camera.main.nearClipPlane );

      MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

      if( meshRenderer )
        {
          meshRenderer.sharedMaterial.SetVector( "_CellSize", new Vector4( baseValue / (float)Screen.width * (float)Screen.height, baseValue, 0f, 0f ) );
        }
    }
}
