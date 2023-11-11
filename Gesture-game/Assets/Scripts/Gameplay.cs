using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    public Transform targetObject;
    public float maxAngle = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check if camera is looking at the object
        Vector3 directionToTarget = targetObject.position - Camera.main.transform.position;
        float angle = Vector3.Angle(directionToTarget, Camera.main.transform.forward);

        Debug.DrawLine(transform.position, targetObject.position, Color.blue, 2.5f);
        Debug.DrawLine(Camera.main.transform.forward * 100, transform.position, Color.red, 2.5f);

        if (angle < maxAngle)
        {
            Debug.Log("Object is detected");
        }
        else
        {
            Debug.Log("Object is not detected");
        }
    }
}
