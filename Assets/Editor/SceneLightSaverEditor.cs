using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Assets.Scripts.Utils;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;

[CustomEditor(typeof(SceneLightSaver))]
public class SceneLightSaverEditor : Editor
{
    private SceneLightSaver _script;

    public override void OnInspectorGUI()
    {
        if (_script == null)
        {
            _script = (SceneLightSaver)target;
        }

        // Show default inspector property editor
        DrawDefaultInspector();

        GUILayout.Space(5);

        GUILayout.Space(10);
        if (GUILayout.Button("Generate Lightning", GUILayout.Width(style_utils.GetPercent(Screen.width, 90)), GUILayout.Height(50)))
        {
            _script.GenerateLighting();
        }
        
        GUILayout.Space(10);
        if (GUILayout.Button("Generate Variations Lightning", GUILayout.Width(style_utils.GetPercent(Screen.width, 90)), GUILayout.Height(50)))
        {
            _script.GenerateVariationsLighting();
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Save The Different Scenes", GUILayout.Width(style_utils.GetPercent(Screen.width, 90)), GUILayout.Height(50)))
        {

        }

        GUILayout.Space(5);

    }

}
