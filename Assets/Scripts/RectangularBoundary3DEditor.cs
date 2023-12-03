using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RectangularBoundary3D))]
public class RectangularBoundary3DEditor : Editor
{
    public override void OnInspectorGUI() {

        RectangularBoundary3D rectangularBoundary = (RectangularBoundary3D) target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate")) {
            rectangularBoundary.GenerateBoundary();
        }

    }
}
