#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate Map"))
        {
            ((MapGenerator)target).GenerateMapInEditor();
        }
        if (GUILayout.Button("Generate Objects"))
        {
            ((MapGenerator)target).GenerateObjectsInEditor();
        }
    }
}

#endif