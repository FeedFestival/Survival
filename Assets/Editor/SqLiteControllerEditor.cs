using System;
using UnityEngine;
using System.Collections;
using System.Configuration;
using System.Linq;
using Assets.Scripts.Utils;
using UnityEditor;

[CustomEditor(typeof(SqLiteController))]
public class SqLiteControllerEditor : Editor
{
    private SqLiteController _script;

    private bool _setupConfirm;
    public enum InspectorButton
    {
        RecreateDataBase, CleanUpUsers, CreateMap, UpdateMap
    }
    private InspectorButton _actionTool;
    private InspectorButton _action
    {
        get { return _actionTool; }
        set
        {
            _actionTool = value;
            _setupConfirm = true;
        }
    }

    public override void OnInspectorGUI()
    {
        if (_script == null)
        {
            _script = (SqLiteController)target;
        }

        GUILayout.Space(10);
        
        EditorGUILayout.BeginVertical();

        GUILayout.Space(5);

        if (GUILayout.Button("Recreate Database"))
            _action = InspectorButton.RecreateDataBase;
        
        GUILayout.Space(10);

        if (GUILayout.Button("Clean Up Users"))
            _action = InspectorButton.CleanUpUsers;

        GUILayout.Space(5);

        EditorGUILayout.EndVertical();

        //--------------------------------------------------------------------------------------------------------------------------------------------------------
        GUILayout.Space(20);    // CONFIRM
        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        if (_setupConfirm)
        {
            EditorGUILayout.BeginVertical("box");
            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Confirm", GUILayout.Width(style_base.GetPercent(Screen.width, 25)), GUILayout.Height(50)))
                ConfirmAccepted();

            if (GUILayout.Button("Cancel", GUILayout.Width(style_base.GetPercent(Screen.width, 25)), GUILayout.Height(50)))
                _setupConfirm = false;

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            EditorGUILayout.BeginVertical();
        }

        // Show default inspector property editor
        DrawDefaultInspector();
    }

    private void ConfirmAccepted()
    {
        switch (_action)
        {
            case InspectorButton.RecreateDataBase:

                _script.RecreateDataBase();
                break;

            case InspectorButton.CleanUpUsers:

                _script.CleanUpUsers();
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
        _setupConfirm = false;
    }
}