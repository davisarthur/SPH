using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RectangularBoundary2D))]
public class RectangularBoundary2DEditor : Editor {
    public override void OnInspectorGUI() {

        RectangularBoundary2D rectangularBoundary = (RectangularBoundary2D)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate")) {
            rectangularBoundary.GenerateBoundary();
        }

    }
}
