using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DomeController : MonoBehaviour {

    public enum DomeAngle
    {
        Dome125 = 0,
        Dome135 = 1,
        Dome180 = 2,
        Invalid = 3,
    }
    public DomeAngle currentDomeAngle = DomeAngle.Invalid;
    private DomeAngle oldDomeAngle = DomeAngle.Invalid;

    public Mesh dome125Mesh;
    public Mesh dome135Mesh;
    public Mesh dome180Mesh;

    void Update()
    {
        if( currentDomeAngle != oldDomeAngle )
        {
            UpdateMesh();
        }
    }

    private void UpdateMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if( meshFilter )
        {
            switch( currentDomeAngle )
            {
                case DomeAngle.Dome125:
                    meshFilter.sharedMesh = dome125Mesh;
                    break;

                case DomeAngle.Dome135:
                    meshFilter.sharedMesh = dome135Mesh;
                    break;

                case DomeAngle.Dome180:
                    meshFilter.sharedMesh = dome180Mesh;
                    break;
            }
        }

        oldDomeAngle = currentDomeAngle;
    }
}
