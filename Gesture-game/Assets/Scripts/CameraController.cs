using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Scripting.Python;
using UnityEditor;

//using Python.Runtime;
//using Python.Runtime;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        PythonRunner.RunString(@"
        import UnityEngine;
        UnityEngine.Debug.Log('hello world')
        ");
        PythonRunner.RunFile("Assets\\Scripts\\face_position_recognition.py");
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
