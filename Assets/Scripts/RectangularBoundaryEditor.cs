using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RectangularBoundary))]
public class RectangularBoundaryEditor : Editor
{
    public override void OnInspectorGUI() {

        RectangularBoundary rectangularBoundary = (RectangularBoundary) target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate")) {
            rectangularBoundary.GenerateBoundary();
        }

    }
}
