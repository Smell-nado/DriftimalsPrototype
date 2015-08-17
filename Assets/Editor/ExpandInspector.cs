using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(TriggerScript)), CanEditMultipleObjects]
public class ExpandInspector : Editor {

    public SerializedProperty trigger_type;
    public SerializedProperty turnDirection;
    public SerializedProperty cornerMagnitude;
    public SerializedProperty launchForceValue;
    public SerializedProperty launchAngle;
    public SerializedProperty _nextTrigger;

    void OnEnable()
    {
        // Setup the SerializedProperties
        trigger_type = serializedObject.FindProperty("_type");
        turnDirection = serializedObject.FindProperty("_turnDirection");
        cornerMagnitude = serializedObject.FindProperty("_cornerMagnitude");
        launchForceValue = serializedObject.FindProperty("_launchForceMagnitude");
        launchAngle = serializedObject.FindProperty("_launchAngle");
        _nextTrigger = serializedObject.FindProperty("_nextTrigger");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(trigger_type);
        TriggerScript.TriggerType ts = (TriggerScript.TriggerType)trigger_type.enumValueIndex;
        TriggerScript trigger = (TriggerScript)target;
        bool allowSceneObjects = !EditorUtility.IsPersistent(target);
        switch (ts)
        {
            case TriggerScript.TriggerType.startDrift:
                EditorGUILayout.PropertyField(turnDirection, new GUIContent("Direction"));
                EditorGUILayout.IntSlider(cornerMagnitude, 0, 90, new GUIContent("Turn Magnitude"));
                trigger._endTrigger = (GameObject)EditorGUILayout.ObjectField("End Trigger", trigger._endTrigger, typeof(GameObject), allowSceneObjects);
                break;

            case TriggerScript.TriggerType.endDrfit:
                EditorGUILayout.PropertyField(_nextTrigger, new GUIContent("Next Trigger"));
                break;

            case TriggerScript.TriggerType.startRace:
                EditorGUILayout.PropertyField(_nextTrigger, new GUIContent("Next Trigger"));
                break;

            case TriggerScript.TriggerType.launch:
                //bring up force amount
                //bring up dir vector 3
                EditorGUILayout.PropertyField(launchForceValue, new GUIContent("Launch Force"));
                EditorGUILayout.IntSlider(launchAngle, 0, 90, new GUIContent("Launch Angle"));
                break;

        }


        serializedObject.ApplyModifiedProperties();
    }
}