using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(LearnerPack))]
public class LearnerPack_Inspector : Editor
{
    //public override VisualElement CreateInspectorGUI()
    //{
    //    // Create a new VisualElement to be the root of our Inspector UI.
    //    VisualElement myInspector = new();

    //    // Add a simple label.
    //    myInspector.Add(new Label("This is a custom Inspector"));

    //    // Return the finished Inspector UI.
    //    return myInspector;
    //}
}
