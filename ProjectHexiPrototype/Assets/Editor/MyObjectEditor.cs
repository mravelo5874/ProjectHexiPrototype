using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MyObject))]
[CanEditMultipleObjects]
public class MyObjectEditor : Editor
{
    MyObject myObject;

    // non-editor properties (belong to MyObject.cs)
    SerializedProperty obj_wiggleBoolean;
    SerializedProperty obj_wiggleCurve;
    SerializedProperty obj_wiggleDuration;
    SerializedProperty obj_wiggleMultiplier;

    // local editor variables
    bool edit_wiggleBoolean;
    AnimationCurve edit_wiggleCurve;
    float edit_wiggleDuration;
    float edit_wiggleMultiplier;

#if UNITY_EDITOR
    void OnEnable()
    {
        myObject = (MyObject)target;

        // wiggle variables
        obj_wiggleBoolean = serializedObject.FindProperty("wiggleBoolean");
        obj_wiggleCurve = serializedObject.FindProperty("wiggleCurve");
        obj_wiggleDuration = serializedObject.FindProperty("wiggleDuration");
        obj_wiggleMultiplier = serializedObject.FindProperty("wiggleMultiplier");

        edit_wiggleBoolean = obj_wiggleBoolean.boolValue;
        edit_wiggleCurve = obj_wiggleCurve.animationCurveValue; 
        edit_wiggleDuration = obj_wiggleDuration.floatValue;
        edit_wiggleMultiplier = obj_wiggleMultiplier.floatValue;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        edit_wiggleBoolean = EditorGUILayout.Toggle("Use Wiggle Functionality", edit_wiggleBoolean);
        EditorGUILayout.Space();

        if (edit_wiggleBoolean)
        {
            edit_wiggleCurve = EditorGUILayout.CurveField("Animation Curve: ", edit_wiggleCurve);
            edit_wiggleDuration = EditorGUILayout.FloatField("Duration: ", edit_wiggleDuration);
            edit_wiggleMultiplier = EditorGUILayout.FloatField("Multiplier: ", edit_wiggleMultiplier);
        }

        obj_wiggleBoolean.boolValue = edit_wiggleBoolean;
        obj_wiggleCurve.animationCurveValue = edit_wiggleCurve;
        obj_wiggleDuration.floatValue = edit_wiggleDuration;
        obj_wiggleMultiplier.floatValue = edit_wiggleMultiplier;
        
        serializedObject.ApplyModifiedProperties();
    }
#endif
}
