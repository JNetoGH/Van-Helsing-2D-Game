using System;
using UnityEditor;


[CustomEditor(typeof(SmoothSpringCamera))]
public class SmoothSpringCameraEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        
        string text = "\n" +
                      "When the target is out of the Smoothening Area, " +
                      "the camera will simply follow it in a constant high " +
                      "speed without smoothening, in a attempt to catch it." +
                      "\n\n" +
                      "If the Smoothening Area is turned off, " +
                      "the camera will always use the smoothening " +
                      "unless it's individually turned off for each axis " +
                      "at the 'Smooth Follow (X/Y)'" +
                      "\n";
        
        EditorGUILayout.HelpBox(text, MessageType.None);
        DrawDefaultInspector();
        
    }
}
