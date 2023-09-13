using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Scripting.Python;
using UnityEditor;
using System;
//using Python.Runtime;
public class CameraController : MonoBehaviour
{
    void Start()
    {
/*        PythonRunner.RunString(@"
            import UnityEngine;
            UnityEngine.Debug.Log('hello world')
        ");*/

    }

    void Update()
    {
        PythonRunner.RunFile("Assets\\Scripts\\face_position_recognition.py");
    }
}
