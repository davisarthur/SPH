using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Main2D))]
public class Main2DEditor : Editor
{
    

    public override void OnInspectorGUI() {

        Main2D main2D = (Main2D) target;

        DrawDefaultInspector();

        if (GUILayout.Button("SpawnParticles")) {
            main2D.SpawnParticles();
        }

        if (GUILayout.Button("ComputeDensity")) {
            main2D.ComputeDensity();
        }

        if (GUILayout.Button("ComputeHash")) {
            main2D.ComputeHash();
        }
    }
}
